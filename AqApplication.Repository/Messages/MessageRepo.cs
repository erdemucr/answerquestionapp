using AqApplication.Core.Type;
using AqApplication.Entity.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System;

namespace AnswerQuestionApp.Repository.Messages
{
   public class MessageRepo: IMessage
    {
        private readonly int PageSize = 20;
        private ApplicationDbContext context;
        private bool disposedValue = false;
        private readonly UserManager<ApplicationUser> UserManager;
        public MessageRepo(ApplicationDbContext _context, UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
            context = _context;
        }

        public Result AddChatHistory(Entity.Message.ChatHistory model, string userId)
        {
            try
            {
                context.ChatHistory.Add(model);
                context.SaveChanges();

                return new Result { Success = true,  MessageType= MessageType.InsertSuccess };
            }
            catch (Exception ex)
            {
               return new Result( ex);
            }
        }
    }
}
