using AnswerQuestionApp.Repository.Messages;
using AnswerQuestionApp.WebSocket.Types;
using AqApplication.Entity.Constants;
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

namespace AnswerQuestionApp.WebSocket.Methods
{
    public class MessagingOperations
    {
        private IUser _iUser;
        private IMessage _iMessage;
        private int ChallengePeriodSecond = 0;
        private int MinimumChallegneSecond = 0;

        private static IList<SocketClientModel> _clients = new List<SocketClientModel>();
        public async Task SendMessage(HttpContext hContext, System.Net.WebSockets.WebSocket socket)
        {
            _iUser = hContext.RequestServices.GetRequiredService<IUser>();
            _iMessage = hContext.RequestServices.GetRequiredService<IMessage>();

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


                        string message = string.Empty;
                        bool success = true;

                        var user = _iUser.GetUserInfo(socketRequestModel.userId);

                        if (!user.Success)
                        {
                            message = JsonConvert.SerializeObject(new MessagingClientResult
                            {
                                Message = "Sistemsel bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.",
                                MessageCode = Result.MessageCode.ServerError
                            });
                            success = false;
                        }

                        if (!success) // send error message or action
                        {
                            byte[] outgoingMessage = System.Text.Encoding.UTF8.GetBytes(message);

                            await socket.SendAsync(new ArraySegment<byte>(outgoingMessage, 0, outgoingMessage.Length),
                              WebSocketMessageType.Text, true, CancellationToken.None);
                        }
                        else
                        {
                            if (!_clients.Any(x => x.UserId == socketRequestModel.userId))
                                _clients.Add(new SocketClientModel
                                {
                                    Socket = socket,
                                    UserId = socketRequestModel.userId,
                                    FullName = user.Data.FirstName + " " + user.Data.LastName,
                                    UserName = user.Data.Email,
                                    Image = user.Data.ProfilImage
                                });
                            if (socketRequestModel.action == RequestAction.join)
                            {
                                await SendClientListMessenger();
                            }
                            if (socketRequestModel.action == RequestAction.push)
                            {
                                await SendMessageToDestination(socketRequestModel.userId, user.Data.FirstName + " " + user.Data.LastName, socketRequestModel.userId, socketRequestModel.message);
                                await SendMessageToDestination(socketRequestModel.userId, user.Data.FirstName + " " + user.Data.LastName, socketRequestModel.to, socketRequestModel.message);
                                await AddMessageHistory(new Entity.Message.ChatHistory
                                {

                                    CreatedDate = DateTime.Now,
                                    Sender= socketRequestModel.userId,
                                    Receiver= socketRequestModel.to,
                                    Message= socketRequestModel.message
                                });

                            }
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
            {
                var client = _clients.FirstOrDefault(x => x.Socket == socket);
                if (client != null)
                {
                    _clients.Remove(client);
                    if (_clients.Any())
                        await SendClientListMessenger();
                }

                await socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                                        statusDescription: "Closed by the WebSocketManager",
                                        cancellationToken: CancellationToken.None);

                return 0;
            }
        }
        private async Task<int> SendClientListMessenger()
        {
            await Task.Run(() =>
            {
                var clients = _clients.ToList();

                string message = JsonConvert.SerializeObject(new MessagingClientResult
                {
                    SocketClientModelList = clients,
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
        private async Task<int> SendMessageToDestination(string from, string fullName, string to, string msg)
        {
            await Task.Run(() =>
            {
                var clients = _clients.ToList();

                string message = JsonConvert.SerializeObject(new MessagingClientResult
                {
                    SocketClientModelList = null,
                    Message = msg,
                    MessageCode = Result.MessageCode.Success,
                    FromIdendity = from,
                    FromFullName = fullName
                });

                byte[] outgoingMessage = System.Text.Encoding.UTF8.GetBytes(message);

                var client = _clients.FirstOrDefault(x => x.UserId == to);
                if (client != null)
                {
                    client.Socket.SendAsync(
                        new ArraySegment<byte>(outgoingMessage, 0, outgoingMessage.Length),
                            WebSocketMessageType.Text, true, CancellationToken.None);
                }
            });

            return 0;
        }

        private async Task<int> AddMessageHistory(AnswerQuestionApp.Entity.Message.ChatHistory model)
        {
            await Task.Run(() =>
            {
                _iMessage.AddChatHistory(model, model.Sender);
            });
            return 0;
        }
    }
}