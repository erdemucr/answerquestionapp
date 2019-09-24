
namespace AnswerQuestionApp.WebSocket.Types
{
    internal class ChallengeClientResult: SocketClientResult
    {
        public int ChallengeId { get; set; }
        public int LeftSecond { get; set; }
        public int Next { get; set; }
        public int QuizDuration { get; set; }

        public ChallengeSocketResult ChallengeSocketResult { get; set; }

    }
}
