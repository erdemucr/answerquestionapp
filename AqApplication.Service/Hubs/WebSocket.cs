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

namespace AqApplication.Service.Hubs
{
    public class WebSockets
    {
        public WebSockets()
        {

        }
        //public WebSockets(AqApplication.Repository.Session.IUser iUser)
        //{
        //    _iUser = iUser;
        //}
        private readonly AqApplication.Repository.Session.IUser _iUser;
        private static List<SocketClientModel> _clients { get; set; }

        public async Task Talk(HttpContext hContext, WebSocket socket)
        {
            while (socket.State == WebSocketState.Open)
            {
                //   _clients.AddSocket(socket);
                WebSocketReceiveResult result = null;
                try
                {


                    var bag = new byte[4096];
                    result = await socket.ReceiveAsync(new ArraySegment<byte>(bag), CancellationToken.None);

                    while (!result.CloseStatus.HasValue)
                    {
                        var incomingMessage = System.Text.Encoding.UTF8.GetString(bag, 0, result.Count);
                        var socketRequestModel = Newtonsoft.Json.JsonConvert.DeserializeObject<SocketRequestModel>(incomingMessage);


                        if (socketRequestModel != null)
                        {
                            var user = _iUser.GetUserInfo(socketRequestModel.userId);
                            _clients.Add(new SocketClientModel
                            {
                                Socket = socket,
                                UserId = socketRequestModel.userId,
                                ChallengeId = 1,
                                FullName = user.Data.FirstName + " " + user.Data.LastName,
                                UserName = user.Data.Email
                            });
                        }

                        Console.WriteLine("\nClient says that '{0}'\n", incomingMessage);

                        string message = "ok";
                        byte[] outgoingMessage = System.Text.Encoding.UTF8.GetBytes(message);
                        await socket.SendAsync(new ArraySegment<byte>(outgoingMessage, 0, outgoingMessage.Length), result.MessageType, result.EndOfMessage, CancellationToken.None);
                        result = await socket.ReceiveAsync(new ArraySegment<byte>(bag), CancellationToken.None);
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
                      await  HandleClose(socket);
                        break;

                    case WebSocketMessageType.Text:
                        // Handle text message 
                        break;
                }
            }

            if (socket.State == WebSocketState.Aborted)
            {
                // Handle aborted
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
                var client = _clients.FirstOrDefault(x => x.Socket == socket);
                if (client != null)
                    _clients.Remove(client);
            });

            await socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                                    statusDescription: "Closed by the WebSocketManager",
                                    cancellationToken: CancellationToken.None);

            return 0;
        }

        private async Task<int> SendClientListByChallengeId(int challengeId)
        {
            await Task.Run(() =>
            {

                var clients = _clients.Where(x => x.ChallengeId == challengeId).ToList();

                string message = Newtonsoft.Json.JsonConvert.SerializeObject(clients);
                byte[] outgoingMessage = System.Text.Encoding.UTF8.GetBytes(message);

                Parallel.ForEach(clients, client =>
                {
                    client.Socket.SendAsync(
                       new ArraySegment<byte>(outgoingMessage, 0, outgoingMessage.Length),
                       WebSocketMessageType.Text, false, CancellationToken.None);
                });


            });

            return 0;
        }
    }
}
public class SocketClientModel
{
    public string UserId { get; set; }

    public int ChallengeId { get; set; }

    public WebSocket Socket { get; set; }

    public string UserName { get; set; }

    public string FullName { get; set; }

}
public class SocketRequestModel
{
    public string userId { get; set; }
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


