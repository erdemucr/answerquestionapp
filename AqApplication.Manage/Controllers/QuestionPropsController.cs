using AqApplication.Entity.Question;
using AqApplication.Manage.Models;
using AqApplication.Manage.Utilities;
using AqApplication.Repository.Question;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AqApplication.Manage.Controllers
{

    [Authorize]
    public class QuestionPropsController : BaseController
    {

        private readonly IQuestion _iquestion;
        public QuestionPropsController(IQuestion iQuestion)
        {
            _iquestion = iQuestion;

        }
        // GET: QuestionProps
        public IActionResult Index()
        {
            return View();
        }

        #region Lecture
        public IActionResult Lectures()
        {
            var result = _iquestion.GetLectures();

            var exams = _iquestion.GetExams();

            result.Data = result.Data.Select(x => { x.Exams = x.ExamLectures.Select(y => exams.Data.First(z => z.Id == y.ExamId).Name).ToArray(); return x; });


            return View(result.Data);
        }
        public IActionResult SetLectureStatus(int id)
        {
            var result = _iquestion.SetLectureStatus(id, User.GetUserId());
            TempData["success"] = result.Success;
            TempData["message"] = result.Message;
            return RedirectToAction("Lectures");
        }
        public IActionResult AddLecuture()
        {
            ViewBag.examSelectList = _iquestion.GetExams().Data.Select(x =>
             new SelectListItem
             {
                 Text = x.Name,
                 Value = x.Id.ToString()
             }).AsEnumerable();
            return View();
        }
        [HttpPost]
        public IActionResult AddLecuture(Lecture model)
        {
            ViewBag.examSelectList = _iquestion.GetExams().Data.Select(x =>
             new SelectListItem
             {
                 Text = x.Name,
                 Value = x.Id.ToString()
             }).AsEnumerable();

            if (model.Exams != null)
                model.ExamLectures = model.Exams.Select(x => new ExamLecture { ExamId = Convert.ToInt32(x) }).ToList();

            if (!ModelState.IsValid)
            {
                TempData["success"] = false;
                TempData["message"] = "Lütfen alanları kontrol ediniz";
                return View(model);
            }
            model.CreatedDate = DateTime.Now;
            var result = _iquestion.AddLecture(model, User.GetUserId());
            TempData["success"] = result.Success;
            TempData["message"] = result.Message;
            return RedirectToAction("Lectures");
        }
        public IActionResult EditLecture(int id)
        {
            ViewBag.examSelectList = _iquestion.GetExams().Data.Select(x =>
             new SelectListItem
             {
                 Text = x.Name,
                 Value = x.Id.ToString()
             }).AsEnumerable();
            var result = _iquestion.GetLectureByKey(id);
            if (!result.Success)
            {
                TempData["success"] = result.Success;
                TempData["message"] = result.Message;
                return RedirectToAction("Lectures");
            }
            if (result.Data.ExamLectures != null)
                result.Data.Exams = result.Data.ExamLectures.ToList().Select(x => x.ExamId.ToString()).ToArray();

            return View(result.Data);
        }
        [HttpPost]
        public IActionResult EditLecture(Lecture model)
        {
            ViewBag.examSelectList = _iquestion.GetExams().Data.Select(x =>
             new SelectListItem
             {
                 Text = x.Name,
                 Value = x.Id.ToString()
             }).AsEnumerable();
            if (!ModelState.IsValid)
            {
                TempData["success"] = false;
                TempData["message"] = "Lütfen alanları kontrol ediniz";
                return View(model);
            }
            if (model.Exams != null)
                model.ExamLectures = model.Exams.Select(x => new ExamLecture { ExamId = Convert.ToInt32(x) }).ToList();

            var result = _iquestion.EditLecture(model, User.GetUserId());
            TempData["success"] = result.Success;
            TempData["message"] = result.Message;
            return RedirectToAction("Lectures");
        }

        public IActionResult DeleteLecture(int id)
        {
            var result = _iquestion.DeleteLecture(id);
            TempData["success"] = result.Success;
            TempData["message"] = result.Message;
            return RedirectToAction("Lectures");
        }

        #endregion

        #region Subject
        public IActionResult Subjects()
        {
            var result = _iquestion.GetSubjects();
            return View(result.Data);
        }
        public IActionResult SetSubjectStatus(int id)
        {
            var result = _iquestion.SetSubjectStatus(id, User.GetUserId());
            TempData["success"] = result.Success;
            TempData["message"] = result.Message;
            return RedirectToAction("Subjects");
        }
        public IActionResult AddSubject()
        {
            ViewBag.LectureSelectList = _iquestion.GetLectures()
              .Data.Where(x => x.IsActive).Select(
              x => new SelectListItem
              {
                  Text = x.Name,
                  Value = x.Id.ToString()
              }
              ).AsEnumerable();
            return View();
        }
        [HttpPost]
        public IActionResult AddSubject(Subject model)
        {
            ViewBag.LectureSelectList = _iquestion.GetLectures()
                .Data.Where(x => x.IsActive).Select(
                x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }
                ).AsEnumerable();


            if (!ModelState.IsValid)
            {
                TempData["success"] = false;
                TempData["message"] = "Lütfen alanları kontrol ediniz";
                return View(model);
            }
            model.CreatedDate = DateTime.Now;
            var result = _iquestion.AddSubject(model, User.GetUserId());
            TempData["success"] = result.Success;
            TempData["message"] = result.Message;
            return RedirectToAction("Subjects");
        }
        public IActionResult EditSubject(int id)
        {

            var result = _iquestion.GetSubjectByKey(id);
            if (!result.Success)
            {
                TempData["success"] = result.Success;
                TempData["message"] = result.Message;
                return RedirectToAction("Subjects");
            }
            ViewBag.LectureSelectList = _iquestion.GetLectures()
              .Data.Where(x => x.IsActive).Select(
              x => new SelectListItem
              {
                  Text = x.Name,
                  Value = x.Id.ToString()
              }
              ).AsEnumerable();
            return View(result.Data);
        }
        [HttpPost]
        public IActionResult EditSubject(Subject model)
        {
            ViewBag.LectureSelectList = _iquestion.GetLectures()
              .Data.Where(x => x.IsActive).Select(
              x => new SelectListItem
              {
                  Text = x.Name,
                  Value = x.Id.ToString()
              }
              ).AsEnumerable();
            if (!ModelState.IsValid)
            {
                TempData["success"] = false;
                TempData["message"] = "Lütfen alanları kontrol ediniz";
                return View(model);
            }
            var result = _iquestion.EditSubject(model, User.GetUserId());
            TempData["success"] = result.Success;
            TempData["message"] = result.Message;
            return RedirectToAction("Subjects");
        }

        public IActionResult DeleteSubject(int id)
        {
            var result = _iquestion.DeleteSubject(id);
            TempData["success"] = result.Success;
            TempData["message"] = result.Message;
            return RedirectToAction("Subjects");
        }

        #endregion

        #region SubSubject
        public IActionResult SubSubjects(int id)
        {
            var subject = _iquestion.GetSubjectByKey(id);

            if (!subject.Success)
            {
                TempData["success"] = false;
                TempData["message"] = "Konu bulunamadı";
                return RedirectToAction("Subjects");
            }

            ViewBag.subjectId = id;
            ViewBag.subjectname = subject.Data.Name;
            var result = _iquestion.GetSubSubjects(id);

            return View(result.Data);
        }
        public IActionResult SetSubSubjectStatus(int id)
        {
            var result = _iquestion.SetSubSubjectStatus(id, User.GetUserId());
            TempData["success"] = result.Success;
            TempData["message"] = result.Message;
            return RedirectToAction("Subjects");
        }
        public IActionResult AddSubSubject(int id)
        {
            var subject = _iquestion.GetSubjectByKey(id);

            if (!subject.Success)
            {
                TempData["success"] = false;
                TempData["message"] = "Konu bulunamadı";
                return RedirectToAction("Subjects");
            }
            ViewBag.subjectId = id;
            ViewBag.subjectname = subject.Data.Name;
            ViewBag.SubjectSelectList = _iquestion.GetSubjects()
              .Data.Where(x => x.IsActive).Select(
              x => new SelectListItem
              {
                  Text = x.Name,
                  Value = x.Id.ToString()
              }
              ).AsEnumerable();
            var model = new SubSubject();
            model.SubjectId = id;
            return View(model);
        }
        [HttpPost]
        public IActionResult AddSubSubject(SubSubject model)
        {

            var subject = _iquestion.GetSubjectByKey(model.Id);

            if (!subject.Success)
            {
                TempData["success"] = false;
                TempData["message"] = "Konu bulunamadı";
                return RedirectToAction("Subjects");
            }
            ViewBag.subjectId = model.Id;
            ViewBag.subjectname = subject.Data.Name;
            ViewBag.SubjectSelectList = _iquestion.GetSubjects()
                .Data.Where(x => x.IsActive).Select(
                x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }
                ).AsEnumerable();


            if (!ModelState.IsValid)
            {
                TempData["success"] = false;
                TempData["message"] = "Lütfen alanları kontrol ediniz";
                return View(model);
            }
            model.CreatedDate = DateTime.Now;
            var result = _iquestion.AddSubSubject(model, User.GetUserId());
            TempData["success"] = result.Success;
            TempData["message"] = result.Message;
            return RedirectToAction("SubSubjects", new { id = model.SubjectId });
        }
        public IActionResult EditSubSubject(int id, int subjectId)
        {
            var subject = _iquestion.GetSubjectByKey(subjectId);

            if (!subject.Success)
            {
                TempData["success"] = false;
                TempData["message"] = "Konu bulunamadı";
                return RedirectToAction("Subjects");
            }
            ViewBag.subjectId = id;
            ViewBag.subjectname = subject.Data.Name;
            var result = _iquestion.GetSubSubjectByKey(id);
            if (!result.Success)
            {
                TempData["success"] = result.Success;
                TempData["message"] = result.Message;
                return RedirectToAction("SubSubjects", new { id = id });
            }
            ViewBag.SubjectSelectList = _iquestion.GetSubjects()
              .Data.Where(x => x.IsActive).Select(
              x => new SelectListItem
              {
                  Text = x.Name,
                  Value = x.Id.ToString()
              }
              ).AsEnumerable();
            return View(result.Data);
        }
        [HttpPost]
        public IActionResult EditSubSubject(SubSubject model)
        {
            var subject = _iquestion.GetSubjectByKey(model.SubjectId);

            if (!subject.Success)
            {
                TempData["success"] = false;
                TempData["message"] = "Konu bulunamadı";
                return RedirectToAction("Subjects");
            }
            ViewBag.subjectId = model.SubjectId;
            ViewBag.subjectname = subject.Data.Name;
            ViewBag.SubjectSelectList = _iquestion.GetSubjects()
              .Data.Where(x => x.IsActive).Select(
              x => new SelectListItem
              {
                  Text = x.Name,
                  Value = x.Id.ToString()
              }
              ).AsEnumerable();
            if (!ModelState.IsValid)
            {
                TempData["success"] = false;
                TempData["message"] = "Lütfen alanları kontrol ediniz";
                return View(model);
            }
            var result = _iquestion.EditSubSubject(model, User.GetUserId());
            TempData["success"] = result.Success;
            TempData["message"] = result.Message;
            return RedirectToAction("SubSubjects", new { id = model.SubjectId });
        }

        public IActionResult DeleteSubSubject(int id)
        {
            var result = _iquestion.DeleteSubSubject(id);
            TempData["success"] = result.Success;
            TempData["message"] = result.Message;
            return RedirectToAction("Subjects");
        }

        #endregion

        #region Exam
        public IActionResult Exams()
        {
            var result = _iquestion.GetExams();

            return View(result.Data);
        }
        public IActionResult SetExamStatus(int id)
        {
            var result = _iquestion.SetExamStatus(id, User.GetUserId());
            TempData["success"] = result.Success;
            TempData["message"] = result.Message;
            return RedirectToAction("Exams");
        }
        public IActionResult AddExam()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddExam(Exam model)
        {
            if (!ModelState.IsValid)
            {
                TempData["success"] = false;
                TempData["message"] = "Lütfen alanları kontrol ediniz";
                return View(model);
            }
            model.CreatedDate = DateTime.Now;
            var result = _iquestion.AddExam(model, User.GetUserId());
            TempData["success"] = result.Success;
            TempData["message"] = result.Message;
            return RedirectToAction("Exams");
        }
        public IActionResult EditExam(int id)
        {
            var result = _iquestion.GetExamByKey(id);
            if (!result.Success)
            {
                TempData["success"] = result.Success;
                TempData["message"] = result.Message;
                return RedirectToAction("Exams");
            }

            return View(result.Data);
        }
        [HttpPost]
        public IActionResult EditExam(Exam model)
        {

            if (!ModelState.IsValid)
            {
                TempData["success"] = false;
                TempData["message"] = "Lütfen alanları kontrol ediniz";
                return View(model);
            }
            var result = _iquestion.EditExam(model, User.GetUserId());
            TempData["success"] = result.Success;
            TempData["message"] = result.Message;
            return RedirectToAction("Exams");
        }

        public IActionResult DeleteExam(int id)
        {
            var result = _iquestion.DeleteExam(id);
            TempData["success"] = result.Success;
            TempData["message"] = result.Message;
            return RedirectToAction("Exams");
        }

        #endregion

        #region Class

        public IActionResult Classes()
        {
            var result = _iquestion.GetClass();

            return View(result.Data);
        }
        public IActionResult SetClassStatus(int id)
        {
            var result = _iquestion.SetLectureStatus(id, User.GetUserId());
            TempData["success"] = result.Success;
            TempData["message"] = result.Message;
            return RedirectToAction("Classes");
        }
        public IActionResult AddClass()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddClass(Exam model)
        {
            if (!ModelState.IsValid)
            {
                TempData["success"] = false;
                TempData["message"] = "Lütfen alanları kontrol ediniz";
                return View(model);
            }
            model.CreatedDate = DateTime.Now;
            var result = _iquestion.AddExam(model, User.GetUserId());
            TempData["success"] = result.Success;
            TempData["message"] = result.Message;
            return RedirectToAction("Classes");
        }
        public IActionResult EditClass(int id)
        {
            var result = _iquestion.GetClassByKey(id);
            if (!result.Success)
            {
                TempData["success"] = result.Success;
                TempData["message"] = result.Message;
                return RedirectToAction("Classes");
            }

            return View(result.Data);
        }
        [HttpPost]
        public IActionResult EditClass(Class model)
        {
            if (!ModelState.IsValid)
            {
                TempData["success"] = false;
                TempData["message"] = "Lütfen alanları kontrol ediniz";
                return View(model);
            }
            var result = _iquestion.EditClass(model, User.GetUserId());
            TempData["success"] = result.Success;
            TempData["message"] = result.Message;
            return RedirectToAction("Classes");
        }

        public IActionResult DeleteClass(int id)
        {
            var result = _iquestion.DeleteClass(id);
            TempData["success"] = result.Success;
            TempData["message"] = result.Message;
            return RedirectToAction("Classes");
        }

        #endregion



        #region Difficulty

        public IActionResult Difficulties()
        {
            var result = _iquestion.Difficulty();

            return View(result.Data);
        }
        public IActionResult SetDifficultyStatus(int id)
        {
            var result = _iquestion.SetDifficultyStatus(id, User.GetUserId());
            TempData["success"] = result.Success;
            TempData["message"] = result.Message;
            return RedirectToAction("Difficulties");
        }
        public IActionResult AddDifficulty()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddDifficulty(Difficulty model)
        {
            if (!ModelState.IsValid)
            {
                TempData["success"] = false;
                TempData["message"] = "Lütfen alanları kontrol ediniz";
                return View(model);
            }
            model.CreatedDate = DateTime.Now;
            var result = _iquestion.AddDifficulty(model, User.GetUserId());
            TempData["success"] = result.Success;
            TempData["message"] = result.Message;
            return RedirectToAction("Difficulties");
        }
        public IActionResult EditDifficulty(int id)
        {
            var result = _iquestion.GetDifficultyByKey(id);
            if (!result.Success)
            {
                TempData["success"] = result.Success;
                TempData["message"] = result.Message;
                return RedirectToAction("Difficulties");
            }

            return View(result.Data);
        }
        [HttpPost]
        public IActionResult EditDifficulty(Difficulty model)
        {
            if (!ModelState.IsValid)
            {
                TempData["success"] = false;
                TempData["message"] = "Lütfen alanları kontrol ediniz";
                return View(model);
            }
            var result = _iquestion.EditDifficulty(model, User.GetUserId());
            TempData["success"] = result.Success;
            TempData["message"] = result.Message;
            return RedirectToAction("Difficulties");
        }

        public IActionResult DeleteDifficulty(int id)
        {
            var result = _iquestion.DeleteDifficulty(id);
            TempData["success"] = result.Success;
            TempData["message"] = result.Message;
            return RedirectToAction("Difficulties");
        }

        #endregion

    }
}