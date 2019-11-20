using AnswerQuestionApp.Entity.Message;
using AqApplication.Core.Type;
using AqApplication.Entity.Identity.Data;
using System.Collections.Generic;

namespace AnswerQuestionApp.Repository.Messages
{
    public interface IMessage
    {
        Result AddChatHistory(Entity.Message.ChatHistory model, string userId);

        Result<List<ApplicationUser>> GetChatHistoryUserList(string receiver);

        Result<List<ChatHistory>> GetChatHistories(string receiver);
    }
}
