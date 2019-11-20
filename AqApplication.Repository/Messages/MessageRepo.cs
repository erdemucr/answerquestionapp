using AnswerQuestionApp.Entity.Message;
using AqApplication.Core.Type;
using AqApplication.Entity.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnswerQuestionApp.Repository.Messages
{
    public class MessageRepo : IMessage
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

                return new Result { Success = true, MessageType = MessageType.InsertSuccess };
            }
            catch (Exception ex)
            {
                return new Result(ex);
            }
        }

        public Result<List<ApplicationUser>> GetChatHistoryUserList(string receiver)
        {
            try
            {
                var list = context.ChatHistory
                      .Include(x => x.SenderUser)
                      .Where(x => x.Receiver == receiver)
                      .OrderByDescending(x => x.CreatedDate)
                      .Select(x => x.SenderUser)
                      .Distinct().ToList();

                return new Result<List<ApplicationUser>>
                {
                    Success = true,
                    MessageType = MessageType.OperationCompleted,
                    Data = list
                };
            }
            catch (Exception ex)
            {
                return new Result<List<ApplicationUser>>(ex);
            }
        }

        public Result<List<ChatHistory>> GetChatHistories(string receiver)
        {
            try
            {
                var list = context.ChatHistory
                      .Include(x => x.SenderUser)
                      .Where(x => receiver != null ? x.Receiver == receiver : true)
                      .OrderByDescending(x => x.CreatedDate)
                      .ToList();

                return new Result<List<ChatHistory>>
                {
                    Success = true,
                    MessageType = MessageType.OperationCompleted,
                    Data = list
                };
            }
            catch (Exception ex)
            {
                return new Result<List<ChatHistory>>(ex);
            }
        }
    }
}
