using Newtonsoft.Json;

namespace AnswerQuestionApp.Socket.Types
{
    public class SocketClientModel
    {
        public string UserId { get; set; }
        [JsonIgnore]
        public int ChallengeId { get; set; }
        [JsonIgnore]
        public System.Net.WebSockets.WebSocket Socket { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public string TotalMark { get; set; }

        public int Correct { get; set; }

        public string Image { get; set; }
    }
}
