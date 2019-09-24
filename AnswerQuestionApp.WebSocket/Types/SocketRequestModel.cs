namespace AnswerQuestionApp.WebSocket.Types
{
    public class SocketRequestModel
    {
        public string userId { get; set; }
        public int? challengeId { get; set; } // when true it means result request

        public string to { get; set; }
        public string message { get; set; }
        public RequestAction action { get; set; }
    }
    public enum RequestAction
    {
        join,
        push
    }
}
