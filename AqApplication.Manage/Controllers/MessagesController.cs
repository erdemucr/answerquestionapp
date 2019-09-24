using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AnswerQuestionApp.Manage.Controllers
{
    public class MessagesController : Controller
    {
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
    }
}