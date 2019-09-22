using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnswerQuestionApp.Manage.Models;
using AnswerQuestionApp.Manage.Utilities;
using AnswerQuestionApp.Repository.FilterModels;
using AqApplication.Entity.Challenge;
using AqApplication.Entity.Identity.Data;
using AqApplication.Manage.Controllers;
using AqApplication.Manage.Models;
using AqApplication.Manage.Utilities;
using AqApplication.Repository.Challenge;
using AqApplication.Repository.Question;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AnswerQuestionApp.Manage.Controllers
{
    public class QuizController : BaseController
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly IQuestion _iQuestion;
        private readonly IChallenge _iChallenge;
        private readonly SharedViewLocalizer _iLocalizer;
        public QuizController(SharedViewLocalizer iLocalizer, IChallenge iChallenge, IQuestion iQuestion) : base(iQuestion)
        {
            _iChallenge = iChallenge;
            _iQuestion = iQuestion;
            _iLocalizer = iLocalizer;
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
                AttemptCount = x.ChallengeSessions.Count(),
                CompletedCount = x.ChallengeSessions.Count(y => y.IsCompleted),
                QuestionCount = x.ChallengeQuestions.Count()
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
            searhmodel.Position = SearchModelPosition.Horizontal;
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

            ViewBag.pagination = result.Paginition;

            return View(result.Data);
        }

        public IActionResult AddChallengeTemplate()
        {
            ViewBag.lectureSelectList = _iQuestion.GetLectures().Data.Select(
            x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }
            ).AsEnumerable();
            return View();
        }
        [HttpPost]
        public IActionResult AddChallengeTemplate(ChallengeTemplate model)
        {
            ViewBag.lectureSelectList = _iQuestion.GetLectures().Data.Select(
                x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }
                ).AsEnumerable();
            if (!ModelState.IsValid)
            {
                TempData["success"] = false;
                TempData["message"] = _iLocalizer["Error.ControlFields"];
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
            ViewBag.lectureSelectList = _iQuestion.GetLectures().Data.Select(
            x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }
            ).AsEnumerable();
            var result = _iQuestion.GetChallengeTemplateByKey(id);
            if (!result.Success)
            {
                TempData["success"] = result.Success;
                TempData["message"] = result.Message;
                return RedirectToAction("ChallengeTemplates");
            }
            if (result.Data.StartDate.HasValue)
                result.Data.StartDateTime = result.Data.StartDate.Value.ToString("HH:mm");
            if (result.Data.EndDate.HasValue)
                result.Data.EndDateTime = result.Data.EndDate.Value.ToString("HH:mm");
            return View(result.Data);
        }
        [HttpPost]
        public IActionResult EditChallengeTemplate(ChallengeTemplate model)
        {
            ViewBag.lectureSelectList = _iQuestion.GetLectures().Data.Select(
            x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }
            ).AsEnumerable();
            if (!ModelState.IsValid)
            {
                TempData["success"] = false;
                TempData["message"] = _iLocalizer["Error.ControlFields"];
                return View(model);
            }
            if (model.StartDate.HasValue && !string.IsNullOrEmpty(model.StartDateTime))
                model.StartDate = new DateTime(model.StartDate.Value.Year, model.StartDate.Value.Month, model.StartDate.Value.Day, Convert.ToInt32(model.StartDateTime.Substring(0, 2)), Convert.ToInt32(model.StartDateTime.Substring(3, 2)), 0);
            if (model.EndDate.HasValue && !string.IsNullOrEmpty(model.EndDateTime))
                model.EndDate = new DateTime(model.EndDate.Value.Year, model.EndDate.Value.Month, model.EndDate.Value.Day, Convert.ToInt32(model.EndDateTime.Substring(0, 2)), Convert.ToInt32(model.EndDateTime.Substring(3, 2)), 0);

            var result = _iQuestion.EditChallengeTemplate(model, User.GetUserId());
            TempData["success"] = result.Success;
            TempData["message"] = result.Message;
            return RedirectToAction("ChallengeTemplates");
        }

        public IActionResult ChallengeTemplatesItems(int id)
        {
            var result = _iQuestion.GetChallengeTemplateItems(id);
            EditorLists();
            if (!result.Success)
            {
                TempData["success"] = result.Success;
                TempData["message"] = result.Message;
                return View(new ChallengeTemplateModel { ChallengeItem = new ChallengeTemplateItems { ChallengeTemplateId = id } });
            }

            var challengeTemplate = _iQuestion.GetChallengeTemplateByKey(id);

            ViewBag.templateName = challengeTemplate.Data.Name;
            ViewBag.templateId = challengeTemplate.Data.Id;

            return View(new ChallengeTemplateModel { ChallengeList = result.Data.OrderBy(x=>x.Seo).AsEnumerable(), ChallengeItem = new ChallengeTemplateItems { ChallengeTemplateId = id } });
        }
        public IActionResult AddChallengeTemplatesItems(int challengeTemplateId)
        {
            EditorLists();
            return View(new ChallengeTemplateItems { ChallengeTemplateId = challengeTemplateId });
        }

        [HttpPost]
        public IActionResult AddChallengeTemplatesItems(ChallengeTemplateItems model)
        {
            var result = _iQuestion.AddChallengeTemplateItem(model, User.GetUserId());

            if (!model.ExamIdCheck)
                model.ExamId = null;
            if (!model.LectureIdCheck)
                model.LectureId = null;
            if (!model.SubjectIdCheck)
                model.SubjectId = null;
            if (!model.SubSubjectIdCheck)
                model.SubSubjectId = null;
            if (!model.DifficultyCheck)
                model.Difficulty = null;

            TempData["success"] = result.Success;
            TempData["message"] = result.Message;
            return RedirectToAction("ChallengeTemplatesItems", new { id = model.ChallengeTemplateId });
        }

        public IActionResult EditChallengeTemplatesItems(int challengeTemplateId)
        {
            EditorLists();
            var result = _iQuestion.GetChallengeTemplateItemByKey(challengeTemplateId);
            if (!result.Success)
            {
                return Content(result.Message);
            }

            result.Data.ExamIdCheck = result.Data.ExamId.HasValue;
            result.Data.LectureIdCheck = result.Data.LectureId.HasValue;
            result.Data.SubjectIdCheck = result.Data.SubjectId.HasValue;
            result.Data.SubSubjectIdCheck = result.Data.SubSubjectId.HasValue;
            result.Data.DifficultyCheck = result.Data.Difficulty.HasValue;

            return View(result.Data);
        }
        [HttpPost]
        public IActionResult EditChallengeTemplatesItems(ChallengeTemplateItems model)
        {
            if (!model.ExamIdCheck)
                model.ExamId = null;
            if (!model.LectureIdCheck)
                model.LectureId = null;
            if (!model.SubjectIdCheck)
                model.SubjectId = null;
            if (!model.SubSubjectIdCheck)
                model.SubSubjectId = null;
            if (!model.DifficultyCheck)
                model.Difficulty = null;

            var result = _iQuestion.EditChallengeTemplateItem(model, User.GetUserId());
            TempData["success"] = result.Success;
            TempData["message"] = result.Message;
            return RedirectToAction("ChallengeTemplatesItems", new { id = model.ChallengeTemplateId });
        }


        [HttpPost]
        public JsonResult UpdateOrder(string idOrderMatch)
        {
            var dataResult = _iQuestion.UpdateOrdersChallengeTemplateItem(idOrderMatch);
            return Json(new { success = dataResult.Success });
        }
    }
}