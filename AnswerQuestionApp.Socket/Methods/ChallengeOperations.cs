using AnswerQuestionApp.Socket.Types;
using AqApplication.Entity.Constants;
using AqApplication.Repository.Challenge;
using AqApplication.Repository.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace AnswerQuestionApp.Socket.Methods
{
    public class ChallengeOperations
    {

        private AqApplication.Repository.Session.IUser _iUser;
        private AqApplication.Repository.Challenge.IChallenge _iChallenge;
        private int ChallengePeriodSecond = 0;
        private int MinimumChallegneSecond = 0;

        private static IList<SocketClientModel> _clientsChallengeStart = new List<SocketClientModel>();
        private static IList<SocketClientModel> _clientsChallengeEnd = new List<SocketClientModel>();
        public async Task ChallengeStart(HttpContext hContext, System.Net.WebSockets.WebSocket socket)
        {
            _iUser = hContext.RequestServices.GetRequiredService<IUser>();
            _iChallenge = hContext.RequestServices.GetRequiredService<IChallenge>();

            bool isChallengeServiceOpen = _iChallenge.challengeServiceIsOpen();
            MinimumChallegneSecond = _iChallenge.minimumSecondEntryChallenge();
            ChallengePeriodSecond = _iChallenge.challengeAttemptSecond();

            while (socket.State == WebSocketState.Open)
            {
                //   _clientsChallengeStart.AddSocket(socket);
                WebSocketReceiveResult result = null;
                try
                {
                    var bag = new byte[4096];
                    result = await socket.ReceiveAsync(new ArraySegment<byte>(bag), CancellationToken.None);
                    while (!result.CloseStatus.HasValue)
                    {
                        var incomingMessage = System.Text.Encoding.UTF8.GetString(bag, 0, result.Count);
                        var socketRequestModel = Newtonsoft.Json.JsonConvert.DeserializeObject<SocketRequestModel>(incomingMessage);

                        var lastRandomChallenge = _iChallenge.GetLastChallengeByType(ChallengeTypeEnum.RandomMode);

                        int leftSecond = (int)(lastRandomChallenge.Data.CreatedDate.AddSeconds(ChallengePeriodSecond) - DateTime.Now).TotalSeconds;
                        string message = string.Empty;
                        bool success = true;

                        if (lastRandomChallenge.Success)
                        {
                            if (socketRequestModel == null)
                            {
                                message = JsonConvert.SerializeObject(new ChallengeClientResult
                                {
                                    LeftSecond = leftSecond,
                                    ChallengeSocketResult = ChallengeSocketResult.Error,
                                    Message = "Sistemsel bir hata oluştu. Lütfen daha sonra tekrar deneyiniz."
                                });
                                success = false;
                            }
                            else if (!isChallengeServiceOpen)
                            {
                                message = JsonConvert.SerializeObject(new ChallengeClientResult
                                {
                                    LeftSecond = leftSecond,
                                    ChallengeSocketResult = ChallengeSocketResult.Error,
                                    Message = "Sunucu şuan bakım aşamasındadır. Lütfen daha sonra tekrar deneyiniz."
                                });
                                success = false;
                            }
                            else if (leftSecond < MinimumChallegneSecond)
                            {
                                message = JsonConvert.SerializeObject(new ChallengeClientResult
                                {
                                    LeftSecond = _iChallenge.challengeNextSecond() + MinimumChallegneSecond,
                                    ChallengeSocketResult = ChallengeSocketResult.Next,
                                    Next = MinimumChallegneSecond
                                });

                                success = false;
                            }
                            else if (_clientsChallengeStart.FirstOrDefault(x => x.ChallengeId == lastRandomChallenge.Data.Id && x.UserId == socketRequestModel.userId) != null) // eğer mevcut kullanıcı ile başka bir oturumda giriş yapmış ise
                            {
                                message = JsonConvert.SerializeObject(new ChallengeClientResult
                                {
                                    LeftSecond = leftSecond,
                                    ChallengeSocketResult = ChallengeSocketResult.Error,
                                    Message = "Mevcut kullanıcı ile başka bir aygıtta giriş yaptınız."
                                });
                                success = false;
                            }
                        }
                        else
                        {
                            message = JsonConvert.SerializeObject(new ChallengeClientResult
                            {
                                LeftSecond = leftSecond,
                                ChallengeSocketResult = ChallengeSocketResult.Error,
                                Message = "Sistemsel bir hata oluştu. Lütfen daha sonra tekrar deneyiniz."
                            });
                            success = false;
                        }

                        if (success)
                        {
                            var user = _iUser.GetUserInfo(socketRequestModel.userId);
                            if (user.Success
                                && _clientsChallengeStart.FirstOrDefault(x => x.UserId == socketRequestModel.userId
                                && x.ChallengeId == lastRandomChallenge.Data.Id // eğer ensonki
                                ) == null
                                )
                            {
                                _clientsChallengeStart.Add(new SocketClientModel
                                {
                                    Socket = socket,
                                    UserId = socketRequestModel.userId,
                                    ChallengeId = lastRandomChallenge.Data.Id,
                                    FullName = user.Data.FirstName + " " + user.Data.LastName,
                                    UserName = user.Data.Email
                                });
                                await AddChallengeSession(lastRandomChallenge.Data.Id, socketRequestModel.userId, lastRandomChallenge.Data.CreatedDate); // random challange de başlangı tarih ve saati statikdir
                            }

                            await SendClientListByChallengeId(lastRandomChallenge.Data.Id,
                                (int)((TimeSpan)(lastRandomChallenge.Data.EndDate - lastRandomChallenge.Data.StartDate)).TotalSeconds,
                                leftSecond);
                        }
                        else
                        {
                            byte[] outgoingMessage = System.Text.Encoding.UTF8.GetBytes(message);

                            await socket.SendAsync(new ArraySegment<byte>(outgoingMessage, 0, outgoingMessage.Length),
                              WebSocketMessageType.Text, true, CancellationToken.None);
                        }
                        result = await socket.ReceiveAsync(new ArraySegment<byte>(bag), CancellationToken.None);
                    }
                    await socket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
                }
                catch (AggregateException e)
                {
                    continue;
                }
            }

            if (socket.State == WebSocketState.Aborted)
            {
                await HandleClose(socket);
            }
            else if (socket.State == WebSocketState.Closed)
            {
                await HandleClose(socket);
            }
            else if (socket.State == WebSocketState.CloseReceived)
            {

            }
            else if (socket.State == WebSocketState.CloseSent)
            {
            }
        }
        private async Task<int> HandleClose(System.Net.WebSockets.WebSocket socket)
        {
            await Task.Run(() =>
            {
                var client = _clientsChallengeStart.FirstOrDefault(x => x.Socket == socket);
                if (client != null)
                {
                    _clientsChallengeStart.Remove(client);
                    if (_clientsChallengeStart.Any())
                        if (client.ChallengeId == _clientsChallengeStart.Max(x => x.ChallengeId)) // eğer en sonki challenge ise
                        {
                            var result = SendClientListByChallengeId(client.ChallengeId);
                        }
                }
            });

            await socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                                    statusDescription: "Closed by the WebSocketManager",
                                    cancellationToken: CancellationToken.None);

            return 0;
        }

        private async Task<int> SendClientListByChallengeId(int challengeId, int quizDuration = -1, int leftSecond = -1)
        {
            await Task.Run(() =>
            {
                var clients = _clientsChallengeStart.Where(x => x.ChallengeId == challengeId).ToList();

                string message = JsonConvert.SerializeObject(new ChallengeClientResult
                {
                    LeftSecond = leftSecond,
                    SocketClientModelList = clients,
                    ChallengeId = challengeId,
                    QuizDuration = quizDuration,
                    ChallengeSocketResult = ChallengeSocketResult.Success
                });

                byte[] outgoingMessage = System.Text.Encoding.UTF8.GetBytes(message);

                Parallel.ForEach(clients, client =>
                {
                    client.Socket.SendAsync(
                       new ArraySegment<byte>(outgoingMessage, 0, outgoingMessage.Length),
                       WebSocketMessageType.Text, true, CancellationToken.None);
                });
            });

            return 0;
        }

        public async Task<int> AddChallengeSession(int challengeId, string userId, DateTime startDate)
        {
            await Task.Run(() =>
            {
                _iChallenge.AddChallengeSession(userId, challengeId, startDate);
            });
            return 0;
        }


        public async Task ChallengeEnd(HttpContext hContext, System.Net.WebSockets.WebSocket socket)
        {
            _iUser = hContext.RequestServices.GetRequiredService<IUser>();
            _iChallenge = hContext.RequestServices.GetRequiredService<IChallenge>();

            while (socket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = null;
                try
                {
                    var bag = new byte[4096];
                    result = await socket.ReceiveAsync(new ArraySegment<byte>(bag), CancellationToken.None);

                    var incomingMessage = System.Text.Encoding.UTF8.GetString(bag, 0, result.Count);
                    var socketRequestModel = Newtonsoft.Json.JsonConvert.DeserializeObject<SocketRequestModel>(incomingMessage);

                    if (socketRequestModel.challengeId.HasValue)
                    {

                        var challengeResult = _iChallenge.GetResultChallenge(socketRequestModel.challengeId.Value, socketRequestModel.userId);

                        while (!result.CloseStatus.HasValue)
                        {
                            if (socketRequestModel != null)
                            {
                                var user = _iUser.GetUserInfo(socketRequestModel.userId);
                                if (challengeResult.Success
                                    && _clientsChallengeEnd.FirstOrDefault(x => x.UserId == socketRequestModel.userId
                                    && x.ChallengeId == socketRequestModel.challengeId.Value // eğer ensonki
                                    ) == null
                                  )
                                {
                                    _clientsChallengeEnd.Add(new SocketClientModel
                                    {
                                        Socket = socket,
                                        UserId = socketRequestModel.userId,
                                        ChallengeId = socketRequestModel.challengeId.Value,
                                        FullName = user.Data.FirstName + " " + user.Data.LastName,
                                        UserName = user.Data.Email,
                                        TotalMark = challengeResult.Data.ChallengeUserViewModel.Mark,
                                        Correct = challengeResult.Data.ChallengeUserViewModel.correct
                                    });


                                }
                            }

                            await SendResultList(socketRequestModel.challengeId.Value, _clientsChallengeEnd);
                            result = await socket.ReceiveAsync(new ArraySegment<byte>(bag), CancellationToken.None);
                        }
                    }

                    await socket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);

                }
                catch (AggregateException e)
                {
                    continue;
                }
                switch (result.MessageType)
                {
                    case WebSocketMessageType.Close:
                        await HandleCloseEnd(socket);
                        break;

                    case WebSocketMessageType.Text:
                        // Handle text message 
                        break;
                }
            }

            if (socket.State == WebSocketState.Aborted)
            {
                await HandleCloseEnd(socket);
            }
            else if (socket.State == WebSocketState.Closed)
            {
                await HandleCloseEnd(socket);
            }
            else if (socket.State == WebSocketState.CloseReceived)
            {

            }
            else if (socket.State == WebSocketState.CloseSent)
            {
            }
        }
        private async Task<int> HandleCloseEnd(System.Net.WebSockets.WebSocket socket)
        {
            await Task.Run(() =>
            {
                var client = _clientsChallengeEnd.FirstOrDefault(x => x.Socket == socket);
                if (client != null)
                {
                    _clientsChallengeEnd.Remove(client);
                    client.Socket = null;
                    _clientsChallengeEnd.Add(client);
                }
            });

            await socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                                    statusDescription: "Closed by the WebSocketManager",
                                    cancellationToken: CancellationToken.None);

            return 0;
        }

        private async Task<int> SendResultList(int challengeId, IList<SocketClientModel> _clientsChallengeEnd)
        {
            await Task.Run(() =>
            {
                var clients = _clientsChallengeEnd.Where(x => x.ChallengeId == challengeId && x.Socket != null).ToList();

                string message = JsonConvert.SerializeObject(new SocketResultList
                {
                    ResultList = clients
                });

                byte[] outgoingMessage = System.Text.Encoding.UTF8.GetBytes(message);

                Parallel.ForEach(clients, client =>
                {
                    client.Socket.SendAsync(
                              new ArraySegment<byte>(outgoingMessage, 0, outgoingMessage.Length),
                              WebSocketMessageType.Text, true, CancellationToken.None);
                });
            });

            return 0;
        }
    }
}
