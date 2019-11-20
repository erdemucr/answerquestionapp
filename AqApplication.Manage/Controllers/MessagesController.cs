using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnswerQuestionApp.Manage.Utilities;
using AnswerQuestionApp.Repository.Messages;
using AqApplication.Entity.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AnswerQuestionApp.Manage.Controllers
{
    public class MessagesController : Controller
    {
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly SharedViewLocalizer _iLocalizer;
        private readonly IMessage _iMessage;
        public MessagesController(SharedViewLocalizer iLocalizer, UserManager<ApplicationUser> userManager, IMessage iMessage)
        {
            UserManager = userManager;
            _iLocalizer = iLocalizer;
            _iMessage = iMessage;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Inbox()
        {
            return View();
        }
        public IActionResult Messenger()
        {
            return View();
        }
        public JsonResult GetPastMesagesClients(string userId)
        {
            var result = _iMessage.GetChatHistoryUserList(userId);
            return Json(new { success = result.Success, data = result.Data });
        }
    }
}