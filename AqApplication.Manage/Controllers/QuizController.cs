using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnswerQuestionApp.Manage.Models;
using AnswerQuestionApp.Repository.FilterModels;
using AqApplication.Entity.Challenge;
using AqApplication.Entity.Identity.Data;
using AqApplication.Manage.Models;
using AqApplication.Manage.Utilities;
using AqApplication.Repository.Challenge;
using AqApplication.Repository.Question;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AnswerQuestionApp.Manage.Controllers
{
    public class QuizController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly IQuestion _iQuestion;
        private readonly IChallenge _iChallenge;
        public QuizController(IChallenge iChallenge, IQuestion iQuestion)
        {
            _iChallenge = iChallenge;
            _iQuestion = iQuestion;
        }

        public IActionResult ChallengeList(ChallengeFilterModel model)
        {
            #region SearchModel
            var searhmodel = new SearchModel();
            searhmodel.Controller = "Quiz";
            searhmodel.Action = "ChallengeList";
            searhmodel.SearchInput = new List<SearchInput>();
            searhmodel.SearchInput.Add(new SearchInput(AqApplication.Repository.Enums.InputType.Date, "StartDate", "startdateSearchTxt", "Başlangıç Tarihi"));
            searhmodel.SearchInput.Add(new SearchInput(AqApplication.Repository.Enums.InputType.Date, "EndDate", "enddateSearchTxt", "Bitiş Tarihi"));
            ViewBag.searchModel = searhmodel;
            #endregion

            var result = _iChallenge.GetChallengesPaginated(model);

            if (!result.Success)
            {
                TempData["success"] = result.Success;
                TempData["message"] = result.Message;
                return View(new List<ChallengeListModel>());
            }

            var list = result.Data.Select(x => new ChallengeListModel
            {
                Id = x.Id,
                CreatedDate = x.CreatedDate,
                ModifiedDate = x.ModifiedDate.HasValue ? x.ModifiedDate.Value : new DateTime(),
            }).AsEnumerable();


            ViewBag.pagination = result.Paginition;

            return View(list);
        }
        public IActionResult ChallengeTemplates(ChallengeFilterModel model)
        {
            #region SearchModel
            var searhmodel = new SearchModel();
            searhmodel.Controller = "Quiz";
            searhmodel.Action = "ChallengeList";
            searhmodel.SearchInput = new List<SearchInput>();
            searhmodel.SearchInput.Add(new SearchInput(AqApplication.Repository.Enums.InputType.Text, "Name", "nameSearchTxt", "Arama Anahtarı"));
            searhmodel.SearchInput.Add(new SearchInput(AqApplication.Repository.Enums.InputType.Date, "StartDate", "startdateSearchTxt", "Başlangıç Tarihi"));
            searhmodel.SearchInput.Add(new SearchInput(AqApplication.Repository.Enums.InputType.Date, "EndDate", "enddateSearchTxt", "Bitiş Tarihi"));
            ViewBag.searchModel = searhmodel;
            #endregion
            var result = _iQuestion.GetChallengeTemplates(model);

            if (!result.Success)
            {
                TempData["success"] = result.Success;
                TempData["message"] = result.Message;
                return View(new List<ChallengeTemplateListModel>());
            }

            var list = result.Data.Select(x => new ChallengeTemplateListModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Creator = x.AppUserCreator.FirstName + " " + x.AppUserCreator.LastName,
                CreatedDate = x.CreatedDate,
                ModifiedDate = x.ModifiedDate.HasValue ? x.ModifiedDate.Value : new DateTime(),
                Editor = x.AppUserCreator.FirstName + " " + x.AppUserCreator.LastName,
                StartDate=x.StartDate,
                EndDate=x.EndDate,
                IsActive=x.IsActive
            }).AsEnumerable();


            ViewBag.pagination = result.Paginition;

            return View(list);
        }

        public IActionResult AddChallengeTemplate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddChallengeTemplate(ChallengeTemplate model)
        {
            if (!ModelState.IsValid)
            {
                TempData["success"] = false;
                TempData["message"] = "Lütfen alanları kontrol ediniz";
                return View(model);
            }
            if (model.StartDate.HasValue && !string.IsNullOrEmpty(model.StartDateTime))
                model.StartDate = new DateTime(model.StartDate.Value.Year, model.StartDate.Value.Month, model.StartDate.Value.Day, Convert.ToInt32(model.StartDateTime.Substring(0, 2)), Convert.ToInt32(model.StartDateTime.Substring(3, 2)), 0);
            if (model.EndDate.HasValue && !string.IsNullOrEmpty(model.EndDateTime))
                model.EndDate = new DateTime(model.EndDate.Value.Year, model.EndDate.Value.Month, model.EndDate.Value.Day, Convert.ToInt32(model.EndDateTime.Substring(0, 2)), Convert.ToInt32(model.EndDateTime.Substring(3, 2)), 0);
            model.CreatedDate = DateTime.Now;
            var result = _iQuestion.AddChallengeTemplate(model, User.GetUserId());
            TempData["success"] = result.Success;
            TempData["message"] = result.Message;
            return RedirectToAction("ChallengeTemplates");
        }
        public IActionResult EditChallengeTemplate(int id)
        {
            var result = _iQuestion.GetChallengeTemplateByKey(id);
            if (!result.Success)
            {
                TempData["success"] = result.Success;
                TempData["message"] = result.Message;
                return RedirectToAction("ChallengeTemplates");
            }
            if (result.Data.StartDate.HasValue)
                result.Data.StartDateTime = result.Data.StartDate.Value.ToString("HH:mm");
            if (result.Data.EndDate.HasValue )
                result.Data.EndDateTime = result.Data.EndDate.Value.ToString("HH:mm");
            return View(result.Data);
        }
        [HttpPost]
        public IActionResult EditChallengeTemplate(ChallengeTemplate model)
        {
            if (!ModelState.IsValid)
            {
                TempData["success"] = false;
                TempData["message"] = "Lütfen alanları kontrol ediniz";
                return View(model);
            }
            if (model.StartDate.HasValue && !string.IsNullOrEmpty(model.StartDateTime))
                model.StartDate = new DateTime(model.StartDate.Value.Year, model.StartDate.Value.Month, model.StartDate.Value.Day, Convert.ToInt32(model.StartDateTime.Substring(0, 2)), Convert.ToInt32(model.StartDateTime.Substring(3, 2)), 0);
            if (model.EndDate.HasValue && !string.IsNullOrEmpty(model.EndDateTime))
                model.EndDate = new DateTime(model.EndDate.Value.Year, model.EndDate.Value.Month, model.EndDate.Value.Day, Convert.ToInt32(model.EndDateTime.Substring(0, 2)), Convert.ToInt32(model.EndDateTime.Substring(3, 2)), 0);

            var result = _iQuestion.EditChallengeTemplateItem(model, User.GetUserId());
            TempData["success"] = result.Success;
            TempData["message"] = result.Message;
            return RedirectToAction("ChallengeTemplates");
        }



        public IActionResult ChallengeTemplatesItems(int id)
        {
            var result = _iQuestion.GetChallengeTemplateItems(id);

            if (!result.Success)
            {
                TempData["success"] = result.Success;
                TempData["message"] = result.Message;
                return View(new List<ChallengeTemplateItems>());
            }

            var challengeTemplate = _iQuestion.GetChallengeTemplateByKey(id);

            ViewBag.templateName = challengeTemplate.Data.Name;
            ViewBag.templateId = challengeTemplate.Data.Id;

            return View(result.Data);
        }

        public IActionResult AddChallengeTemplatesItems()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddChallengeTemplatesItems(ChallengeTemplateItems model)
        {
            if (!ModelState.IsValid)
            {
                TempData["success"] = false;
                TempData["message"] = "Lütfen alanları kontrol ediniz";
                return View(model);
            }
          
            return RedirectToAction("ChallengeTemplates");
        }

        public IActionResult EditChallengeTemplatesItems(int id)
        {
            //var result = _iQuestion.GetChallengeTemplateByKey(id);
            //if (!result.Success)
            //{
            //    TempData["success"] = result.Success;
            //    TempData["message"] = result.Message;
            //    return RedirectToAction("ChallengeTemplates");
            //}
            //if (result.Data.StartDate.HasValue)
            //    result.Data.StartDateTime = result.Data.StartDate.Value.ToString("HH:mm");
            //if (result.Data.EndDate.HasValue)
            //    result.Data.EndDateTime = result.Data.EndDate.Value.ToString("HH:mm");
            return View();
        }
        [HttpPost]
        public IActionResult EditChallengeTemplatesItems(ChallengeTemplate model)
        {
            //if (!ModelState.IsValid)
            //{
            //    TempData["success"] = false;
            //    TempData["message"] = "Lütfen alanları kontrol ediniz";
            //    return View(model);
            //}
            //if (model.StartDate.HasValue && !string.IsNullOrEmpty(model.StartDateTime))
            //    model.StartDate = new DateTime(model.StartDate.Value.Year, model.StartDate.Value.Month, model.StartDate.Value.Day, Convert.ToInt32(model.StartDateTime.Substring(0, 2)), Convert.ToInt32(model.StartDateTime.Substring(3, 2)), 0);
            //if (model.EndDate.HasValue && !string.IsNullOrEmpty(model.EndDateTime))
            //    model.EndDate = new DateTime(model.EndDate.Value.Year, model.EndDate.Value.Month, model.EndDate.Value.Day, Convert.ToInt32(model.EndDateTime.Substring(0, 2)), Convert.ToInt32(model.EndDateTime.Substring(3, 2)), 0);
            //var result = _iQuestion.EditChallengeTemplateItem(model, User.GetUserId());
            //TempData["success"] = result.Success;
            //TempData["message"] = result.Message;
            return RedirectToAction("ChallengeTemplates");
        }

    }
}