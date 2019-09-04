using DotNetify;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.Net.WebSockets;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using AqApplication.Repository.Session;
using AqApplication.Repository.Challenge;
using AqApplication.Entity.Constants;
using Newtonsoft.Json;

namespace AqApplication.Service.Hubs
{
    public class WebSockets
    {

        private AqApplication.Repository.Session.IUser _iUser;
        private AqApplication.Repository.Challenge.IChallenge _iChallenge;
        private int ChallengePeriodSecond = 0;
        private int MinimumChallegneSecond = 0;

        private static IList<SocketClientModel> _clientsChallengeStart = new List<SocketClientModel>();
        private static IList<SocketClientModel> _clientsChallengeEnd = new List<SocketClientModel>();
        public async Task ChallengeStart(HttpContext hContext, WebSocket socket)
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
                                message = JsonConvert.SerializeObject(new SocketClientResult
                                {
                                    LeftSecond = leftSecond,
                                    ChallengeSocketResult = ChallengeSocketResult.Error,
                                    Message = "Sistemsel bir hata oluştu. Lütfen daha sonra tekrar deneyiniz."
                                });
                                success = false;
                            }
                            else if (!isChallengeServiceOpen)
                            {
                                message = JsonConvert.SerializeObject(new SocketClientResult
                                {
                                    LeftSecond = leftSecond,
                                    ChallengeSocketResult = ChallengeSocketResult.Error,
                                    Message = "Sunucu şuan bakım aşamasındadır. Lütfen daha sonra tekrar deneyiniz."
                                });
                                success = false;
                            }
                            else if (leftSecond < MinimumChallegneSecond)
                            {
                                message = JsonConvert.SerializeObject(new SocketClientResult
                                {
                                    LeftSecond = _iChallenge.challengeNextSecond() + MinimumChallegneSecond,
                                    ChallengeSocketResult = ChallengeSocketResult.Next,
                                    Next = MinimumChallegneSecond
                                });

                                success = false;
                            }
                            else if (_clientsChallengeStart.FirstOrDefault(x => x.ChallengeId == lastRandomChallenge.Data.Id && x.UserId == socketRequestModel.userId) != null) // eğer mevcut kullanıcı ile başka bir oturumda giriş yapmış ise
                            {
                                message = JsonConvert.SerializeObject(new SocketClientResult
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
                            message = JsonConvert.SerializeObject(new SocketClientResult
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
                                await AddChallengeSession(lastRandomChallenge.Data.Id, socketRequestModel.userId);
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
        private async Task<int> HandleClose(WebSocket socket)
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

                string message = JsonConvert.SerializeObject(new SocketClientResult
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

        public async Task<int> AddChallengeSession(int challengeId, string userId)
        {
            await Task.Run(() =>
            {
                _iChallenge.AddChallengeSession(userId, challengeId);
            });
            return 0;
        }
        public async Task<int> ChallengeSessionCompleted(int challengeId, string userId, string totalMark, int correctCount)
        {
            await Task.Run(() =>
            {
                _iChallenge.UpdateChallengeSessionCompleted(userId, challengeId, totalMark, correctCount);
            });
            return 0;
        }

        public async Task ChallengeEnd(HttpContext hContext, WebSocket socket)
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
                                        TotalMark = challengeResult.Data.Mark,
                                        Correct = challengeResult.Data.correct
                                    });

                                    await ChallengeSessionCompleted(socketRequestModel.challengeId.Value, socketRequestModel.userId, challengeResult.Data.Mark, challengeResult.Data.correct);

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
        private async Task<int> HandleCloseEnd(WebSocket socket)
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
public class SocketClientModel
{
    public string UserId { get; set; }
    [JsonIgnore]
    public int ChallengeId { get; set; }
    [JsonIgnore]
    public WebSocket Socket { get; set; }

    public string UserName { get; set; }

    public string FullName { get; set; }

    public string TotalMark { get; set; }

    public int Correct { get; set; }
}

public class SocketClientResult
{
    public int ChallengeId { get; set; }
    public int LeftSecond { get; set; }
    public List<SocketClientModel> SocketClientModelList { get; set; }

    public int QuizDuration { get; set; }

    public string Message { get; set; }

    public int Next { get; set; }
    public ChallengeSocketResult ChallengeSocketResult { get; set; }
}

public enum ChallengeSocketResult
{
    Success = 0,
    Error = 1,
    Next = 2,
}
public class SocketResultList
{
    public List<SocketClientModel> ResultList { get; set; }
}
public class SocketRequestModel
{
    public string userId { get; set; }
    public int? challengeId { get; set; } // when true it means result request
}
public static class WebSocketConnectionManager
{
    public static WebSocket GetSocketUserId(this List<SocketClientModel> _sockets, string id)
    {
        return _sockets.FirstOrDefault(p => p.UserId == id).Socket;
    }

    public static List<WebSocket> GetAll(this List<SocketClientModel> _sockets)
    {
        return _sockets.Select(x => x.Socket).ToList();
    }

    public static string GetId(this List<SocketClientModel> _sockets, WebSocket socket)
    {
        return _sockets.FirstOrDefault(p => p.Socket == socket).UserId;
    }
    //public static void AddSocket(this List<SocketClientModel> _sockets, WebSocket socket)
    //{
    //    _sockets.Add(CreateConnectionId(), socket);
    //}

    //public static async Task RemoveSocket(this List<SocketClientModel> _sockets, WebSocket socket)
    //{
    //    WebSocket socket;
    //    _sockets.Remove(id, out socket);

    //    await socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
    //                            statusDescription: "Closed by the WebSocketManager",
    //                            cancellationToken: CancellationToken.None);
    //}

    private static string CreateConnectionId()
    {
        return Guid.NewGuid().ToString();
    }
}


