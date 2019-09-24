using AnswerQuestionApp.WebSocket.Result;
using System.Collections.Generic;

namespace AnswerQuestionApp.WebSocket.Types
{
    internal class SocketClientResult
    {

        public List<SocketClientModel> SocketClientModelList { get; set; }

        public string Message { get; set; }

        public MessageCode MessageCode { get; set; }

    }
}
