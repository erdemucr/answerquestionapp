using System;
using System.Collections.Generic;
using System.Linq;
using AnswerQuestionApp.Entity.Configuration;
using AnswerQuestionApp.Repository.Configuration;
using AqApplication.Manage.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace AnswerQuestionApp.Manage.Controllers
{
    public class ConfigurationController : Controller
    {
        private readonly IConfigurationValues _iConfigurationValues;
        public ConfigurationController(IConfigurationValues iConfigurationValues)
        {
            _iConfigurationValues = iConfigurationValues;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Values()
        {
            var result = _iConfigurationValues.GetAll();
            if (!result.Success)
            {
                TempData["success"] = result.Success;
                TempData["message"] = result.Message;
                return View(new List<ConfigurationValues>());
            }

            return View(result.Data);
        }
        [HttpPost]
        public IActionResult Values(string[] values, int[] keyIds)
        {
            if (values == null || keyIds == null)
            {
                TempData["success"] = false;
                TempData["message"] = "Kayıt bulunamadı";
            }
            var list = values.Select(x => new ConfigurationValues { Id = keyIds[Array.IndexOf(values, x)], Values = x }).ToList();

            var result = _iConfigurationValues.Edit(list, User.GetUserId());

            TempData["success"] = result.Success;
            TempData["message"] = result.Message;

            return RedirectToAction("Values");
        }
    }
    public class ConfigValuesPost
    {
        public List<Entity.Configuration.ConfigurationValues> ConfigValues { get; set; }
    }
}