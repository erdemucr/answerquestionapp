using AqApplication.Repository.Question;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace AqApplication.Manage.Controllers
{
    public class BaseController : Controller
    {
        private readonly IQuestion _iquestion;
        public BaseController()
        {
        }
        public BaseController(IQuestion iQuestion)
        {
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

            ViewBag.difficultySelectList = Models.Utilities.SelectLists.Difficult();
            ViewBag.licenceSelectList = Models.Utilities.SelectLists.Licence();
        }
    }
}