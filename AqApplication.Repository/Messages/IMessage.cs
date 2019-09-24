using AqApplication.Core.Type;

namespace AnswerQuestionApp.Repository.Messages
{
    public interface IMessage
    {
        Result AddChatHistory(Entity.Message.ChatHistory model, string userId);
    }
}
