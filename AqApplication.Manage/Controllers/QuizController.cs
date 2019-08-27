using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AnswerQuestionApp.Manage.Controllers
{
    public class QuizController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ChallengeList()
        {
            return View();
        }
    }
}