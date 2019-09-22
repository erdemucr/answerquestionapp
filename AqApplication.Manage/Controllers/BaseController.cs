using AnswerQuestionApp.Manage;
using AnswerQuestionApp.Repository.Lang;
using AqApplication.Repository.Question;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AqApplication.Manage.Controllers
{
    public class BaseController : Controller
    {
        private readonly IQuestion _iquestion;
        private readonly ILang _iLang;
        private readonly IStringLocalizer _localizer;
        public BaseController()
        {

        }
        public BaseController(IQuestion iQuestion)
        {
            _iquestion = iQuestion;
        }
        public BaseController(ILang iLang,IQuestion iQuestion)
        {
            _iLang = iLang;
            _iquestion = iQuestion;
        }
        // GET: Base
        public void EditorLists()
        {
            ViewBag.subsubjectSelectList = new List<SelectListItem>();
            ViewBag.classSelectList = _iquestion.GetClass().Data.Select(x =>
            new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).AsEnumerable();

            ViewBag.examSelectList = _iquestion.GetExams().Data.Select(x =>
            new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).AsEnumerable();

            ViewBag.lectureSelectList = _iquestion.GetLectures().Data.Select(
                x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }
                ).AsEnumerable();

            ViewBag.difficultySelectList = _iquestion.Difficulty().Data.Select(
                x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }
                ).AsEnumerable();
            ViewBag.licenceSelectList = Models.Utilities.SelectLists.Licence();
        }

    }
}