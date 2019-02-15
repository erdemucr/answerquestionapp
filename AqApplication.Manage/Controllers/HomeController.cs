﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AqApplication.Manage.Models;
using AqApplication.Entity.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using AqApplication.Manage.Utilities;

namespace AqApplication.Manage.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> UserManager;
        public HomeController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public PartialViewResult Breadcrumb(string Pagename, string Backurl, string Backpagename, string Message, bool? isSuccess)
        {
            return PartialView("_Breadcrumb", new CommonBreadcrumbModel { Pagename = Pagename, Backurl = Backurl, Backpagename = Backpagename, Message = Message, isSuccess = isSuccess ?? true });
        }

        public PartialViewResult Header()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.GetUserId();
                var appuser = UserManager.FindByIdAsync(userId);
                return PartialView("_Header", appuser.Result);
            }
            return PartialView("_Header", new ApplicationUser());
        }
    }
}
