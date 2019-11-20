namespace AnswerQuestionApp.Socket.Types
{
    internal class MessagingClientResult: SocketClientResult
    {
        public string FromIdendity { get; set; }

        public string FromFullName { get; set; }

        public string MessageDate { get; set; }

        public bool? Read { get; set; }

        public string ToIdentity { get; set; }

    }
}
