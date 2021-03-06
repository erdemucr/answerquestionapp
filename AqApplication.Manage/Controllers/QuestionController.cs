﻿using AnswerQuestionApp.Manage.Utilities;
using AqApplication.Entity.Identity.Data;
using AqApplication.Entity.Question;
using AqApplication.Manage.Models;
using AqApplication.Manage.Utilities;
using AqApplication.Repository.File;
using AqApplication.Repository.FilterModels;
using AqApplication.Repository.Question;
using IronOcr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Spire.Pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AqApplication.Manage.Controllers
{
    [Authorize]
    public class QuestionController : BaseController
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly IQuestion _iquestion;
        private readonly IFile _ifile;
        private readonly IHostingEnvironment _hostingEnvironment;
        private const string LastSessionQuestionAddModel = "QuestionAddModel";
        private readonly SharedViewLocalizer _iLocalizer;
        public QuestionController(SharedViewLocalizer iLocalizer, IQuestion iQuestion, IFile iFile, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment) : base(iQuestion)
        {
            _iquestion = iQuestion;
            _ifile = iFile;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
            _iLocalizer = iLocalizer;
        }
        // GET: Question
        public ActionResult Index(QuestionFilterModel model)
        {
            #region SearchModel
            var searhmodel = new SearchModel();
            searhmodel.Controller = "Question";
            searhmodel.Action = "Index";
            searhmodel.Position = SearchModelPosition.Vertical;
            searhmodel.SearchInput = new List<SearchInput>();
            searhmodel.SearchInput.Add(new SearchInput(AqApplication.Repository.Enums.InputType.Text, "Name", "nameSearchTxt", _iLocalizer["Name"]));
            searhmodel.SearchInput.Add(new SearchInput(AqApplication.Repository.Enums.InputType.Date, "StartDate", "startdateSearchTxt", _iLocalizer["StartDate"]));
            searhmodel.SearchInput.Add(new SearchInput(AqApplication.Repository.Enums.InputType.Date, "EndDate", "enddateSearchTxt", _iLocalizer["EndDate"]));
            ViewBag.searchModel = searhmodel;
            #endregion


            var result = _iquestion.GetQuestion(model);

            var list = result.Data.Select(x => new QuestionListModel
            {
                Id = x.Id,
                MainTitle = x.MainTitle,
                MainImage = x.MainImage,
                Creator = x.AppUserCreator.FirstName + " " + x.AppUserCreator.LastName,
                CreatedDate = x.CreatedDate,
                ModifiedDate = x.ModifiedDate.HasValue ? x.ModifiedDate.Value : new DateTime(),
                Editor = x.AppUserCreator.FirstName + " " + x.AppUserCreator.LastName,
                CorrectAnswer = x.CorrectAnswer == 0 ? "A" : x.CorrectAnswer == 1 ? "B" : x.CorrectAnswer == 2 ? "C" :
                x.CorrectAnswer == 3 ? "D" : x.CorrectAnswer == 4 ? "E" : ""


            }).AsEnumerable();

            ViewBag.pagination = result.Paginition;


            return View(list);
        }

        public ActionResult Answers(int id)
        {
            var result = _iquestion.GetAnswers(id);


            var list = result.Data.Select(x => new AnswerListModel
            {
                ImageUrl = x.ImageUrl,
                Title = x.Title,
                IsTrue = x.IsTrue,
                Seo= x.Seo??0
            }).AsEnumerable();
            return View(list);

        }
        [HttpGet]
        public ActionResult Editor()
        {
            ClearQuestionSessionKeys();
            EditorLists();
            if (HttpContext.Session.GetString(LastSessionQuestionAddModel) == null)
            {
                return View(new QuestionAddModel
                {
                    MainImage = " ",
                    MainTitle = " ",
                    Licence = false,
                    Option4 = true,
                    QuestionAnswer = new List<AnswerAddModel>{
                   new AnswerAddModel{},
                   new AnswerAddModel{},
                   new AnswerAddModel{},
                   new AnswerAddModel{},
                   new AnswerAddModel{},
               }
                });
            }
            else
            {
                var model = HttpContext.Session.GetObject<QuestionAddModel>(LastSessionQuestionAddModel);
                return View(new QuestionAddModel
                {
                    MainImage = " ",
                    MainTitle = " ",
                    LectureId = model.LectureId,
                    SubjectId = model.SubjectId,
                    SubSubjectId = model.SubSubjectId,
                    Exams = model.Exams,
                    Difficulty = model.Difficulty,
                    Licence = model.Licence,
                    QuestionPdfId = model.QuestionPdfId,
                    CurrentPage = model.CurrentPage,
                    Option4 = model.Option4,
                    QuestionAnswer = new List<AnswerAddModel>{
                   new AnswerAddModel{},
                   new AnswerAddModel{},
                   new AnswerAddModel{},
                   new AnswerAddModel{},
                   new AnswerAddModel{},
               }
                });
            }

        }
        [HttpPost]
        public ActionResult SaveQuestion(QuestionAddModel model)
        {
            if (!ModelState.IsValid)
            {
                EditorLists();
                TempData["success"] = false;
                TempData["message"] = _iLocalizer["Error.ControlFields"].ToString();
                return View("Editor", model);
            }

            var addModel = new QuestionMain
            {
                MainImage = model.MainImage,
                MainTitle = model.MainTitle,
                SubjectId = model.SubjectId,
                SubSubjectId = model.SubSubjectId,
                LectureId = model.LectureId,
                IsActive = false,
                ModifiedDate = DateTime.Now,
                CreatedDate = DateTime.Now,
                Licence = model.Licence,
                Difficulty = model.Difficulty,
                Editor = User.GetUserId(),
                Creator = User.GetUserId(),
                Offer = false,
                Seo = 0,
                QuestionPdfId = model.QuestionPdfId,
                CorrectAnswer = model.TrueOption,
                AnswerCount = model.Option4 ? 4 : 5,
                WidthImage = model.WidthImage,
                HeightImage = model.HeightImage

            };

            var questionExams = new List<QuestionExam>();

            if (model.Exams != null)
            {
                int i = 0;
                foreach (var item in model.Exams)
                {
                    i++;
                    questionExams.Add(new QuestionExam
                    {
                        ExamId = Convert.ToInt32(item),
                        Seo = i,
                        CreatedDate = DateTime.Now,
                        Creator = User.GetUserId(),
                        IsActive = false,
                    });
                }
            }

            var questionAnswers = new List<QuestionAnswer>();

            if (model.QuestionAnswer != null)
            {
                foreach (var item in model.QuestionAnswer)
                {
                    if (addModel.AnswerCount < model.QuestionAnswer.IndexOf(item))
                    {
                        questionAnswers.Add(new QuestionAnswer
                        {
                            Title = item.Title,
                            IsTrue = model.QuestionAnswer.IndexOf(item) == model.TrueOption,
                        });
                    }
                }
            }

            addModel.QuestionExams = questionExams.Any() ? questionExams : null;
            addModel.QuestionAnswers = questionAnswers.Any() ? questionAnswers : null;

            var result = _iquestion.SaveQuestion(addModel);
            TempData["success"] = result.Success;
            TempData["message"] = result.Message;
            if (result.Success)
                HttpContext.Session.SetObject(LastSessionQuestionAddModel, model);

            return RedirectToAction("Editor");

        }


        public ActionResult DeleteQuestion(int id)
        {
            var result = _iquestion.DeleteQuestion(id);
            TempData["success"] = result.Success;
            TempData["message"] = result.Message;
            return RedirectToAction("Index");
        }

        private void ClearQuestionSessionKeys()
        {
            HttpContext.Session.Remove("questionimage");
            HttpContext.Session.Remove("optionImage1");
            HttpContext.Session.Remove("optionImage2");
            HttpContext.Session.Remove("optionImage3");
            HttpContext.Session.Remove("optionImage4");
            HttpContext.Session.Remove("optionImage5");
            return;
        }

        public JsonResult GetSubjects(int lectureId)
        {
            var list = _iquestion.GetSubjectsByLectureId(lectureId);
            var data = list.Data.Select(x => new CustomSelectModel
            {
                Key = x.Id.ToString(),
                Value = x.Name
            }).ToList()
            ;

            return Json(new { data = data });

        }

        public JsonResult GetSubSubjects(int subjectId)
        {
            var list = _iquestion.GetSubSubjects(subjectId);
            var data = list.Data.Select(x => new CustomSelectModel
            {
                Key = x.Id.ToString(),
                Value = x.Name
            }).ToList();
            return Json(new { data = data });
        }

        public JsonResult GetPdfDocuments(string name)
        {
            var list = _ifile.GetQuestionPdf(new Repository.FilterModels.DocumentFilterModel { Name = name });
            var viewList = list.Data.Select(x => new QuestionPdfViewModel { Id = x.Id, Name = x.Name, TotalPage = x.TotalPage }).ToList();
            return Json(new { data = viewList });
        }
        public JsonResult SetPdfDocument(int id)
        {
            var model = _ifile.GetQuestionPdf(id);
            if (model.Success)
            {
                var viewList = new QuestionPdfViewModel { Name = model.Data.Name, TotalPage = model.Data.TotalPage, Id = model.Data.Id, PdfUrl = model.Data.PdfUrl };
                return Json(new { data = viewList });
            }
            return Json(false);

        }


        public ActionResult Visulation(QuestionAddModel model)
        {
            var modelV = new QuestionAddModel
            {
                MainImage = HttpContext.Session.GetString("questionimage"),
                MainTitle = " ",
                Licence = false,
                QuestionAnswer = new List<AnswerAddModel>{
                   new AnswerAddModel{ ImageUrl = HttpContext.Session.GetString("optionImage1") },
                   new AnswerAddModel{ ImageUrl = HttpContext.Session.GetString("optionImage2") },
                   new AnswerAddModel{ ImageUrl = HttpContext.Session.GetString("optionImage3") },
                   new AnswerAddModel{ ImageUrl = HttpContext.Session.GetString("optionImage4") },
                   new AnswerAddModel{ ImageUrl = HttpContext.Session.GetString("optionImage5") },
               }
            };
            return View(modelV);
        }

        public JsonResult SetImageSession(string id, int type, string content)
        {
            HttpContext.Session.SetString(id, content);
            return Json(true);
        }

        #region QuestionPdf

        public ActionResult QuestionPdf()
        {
            var list = _ifile.GetQuestionPdf(new Repository.FilterModels.DocumentFilterModel { });
            return View(list.Data);

        }

        public ActionResult QuestionPdfContent(PdfContentFilterModel model)
        {
            var result = _ifile.GetQuestionPdfContents(model);
            ViewBag.pagination = result.Paginition;
            return View(result.Data);
        }

        public ActionResult AddQuestionPdf()
        {

            return View();
        }
        [HttpPost]
        public ActionResult AddQuestionPdf(QuestionPdf model, IFormFile file)
        {
            try
            {
                string uploads = "", filePath = "";
                Byte[] fileBytes = null;
                if (file != null && file.Length > 0)
                    try
                    {
                        var uniqueFileName = Guid.NewGuid() + ".pdf";
                        uploads = Path.Combine(_hostingEnvironment.WebRootPath, "Upload");
                        filePath = Path.Combine(uploads, uniqueFileName);

                        string type = file.ContentType;
                        if (type != "application/pdf")
                        {
                            TempData["success"] = false;
                            TempData["message"] = "Lütfen dosya tipinin pdf olduğunu kontrol ediniz";
                            return View(model);
                        }
                        string fileName = Guid.NewGuid() + ".pdf";
                        model.PdfUrl = fileName;
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                            // act on the Base64 data
                        }
                        file.CopyTo(new FileStream(filePath, FileMode.Create));
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    }
                else
                {
                    TempData["success"] = false;
                    TempData["message"] = "Lütfen döküman giriniz";
                    return View(model);
                }
                if (!ModelState.IsValid)
                {
                    TempData["success"] = false;
                    TempData["message"] = _iLocalizer["Error.ControlFields"].ToString();
                    return View(model);
                }
                PdfDocument doc = new PdfDocument();
                doc.LoadFromBytes(fileBytes);
                model.CreatedDate = DateTime.Now;
                model.TotalPage = doc.Pages.Count;
                string userId = User.GetUserId();
                var result = _ifile.AddQuestionPdf(model, User.GetUserId());
                if (result.Success)
                {
                    //var myTask = System.Threading.Tasks.Task.Factory.StartNew(() =>
                    //{
                    UploadPdf(doc, result.InstertedId, model.PdfUrl, model.Name, uploads);
                    //});
                }

                TempData["success"] = result.Success;
                TempData["message"] = result.Message;
                return RedirectToAction("QuestionPdf");
            }
            catch (Exception ex)
            {
                TempData["success"] = false;
                TempData["message"] = ex.InnerException != null ? ex.InnerException.ToString() : ex.Message != null ? ex.Message.ToString() : string.Empty;
                return View(model);
            }
        }
        public void UploadPdf(PdfDocument doc, int contentId, string pdfName, string name, string dir)
        {

            try
            {

                for (int i = 0; i < doc.Pages.Count; i++)
                {
                    Image bmp = doc.SaveAsImage(i);
                    Image zoomImg = new Bitmap((int)(bmp.Size.Width * 2), (int)(bmp.Size.Height * 2));
                    using (Graphics g = Graphics.FromImage(zoomImg))
                    {
                        g.ScaleTransform(2.0f, 2.0f);
                        g.DrawImage(bmp, new Rectangle(new Point(0, 0), bmp.Size), new Rectangle(new Point(0, 0), bmp.Size), GraphicsUnit.Pixel);
                    }
                    //bmp.Save("convertToBmp.bmp", ImageFormat.Bmp);
                    //System.Diagnostics.Process.Start("convertToBmp.bmp");
                    //emf.Save(@"C:\Users\erdemspc\source\repos\AqApplication\AqApplication.Management\Upload\convertToEmf"+i.ToString()+".png", ImageFormat.Png);

                    //zoomImg.Save(dir +"\\"+ pdfName + "_" + i.ToString() + ".png", ImageFormat.Png);

                    string outputFileName = dir + "\\" + pdfName + "_" + i.ToString() + ".png";
                    using (MemoryStream memory = new MemoryStream()) // anti virusu kapatmayı unutma!!!!
                    {
                        using (FileStream fs = new FileStream(outputFileName, FileMode.Create, FileAccess.Write))
                        {
                            zoomImg.Save(memory, ImageFormat.Png);
                            byte[] bytes = memory.ToArray();
                            fs.Write(bytes, 0, bytes.Length);
                        }
                    }

                    //bmp.Save("convertToBmp.bmp", ImageFormat.Bmp);

                    _ifile.AddQuestionContentPdf(new Entity.Question.QuestionPdfContent
                    {
                        FileName = pdfName + "_" + i.ToString() + ".png",
                        ImageUrl = "/Upload/" + pdfName + "_" + i.ToString() + ".png",
                        QuestionId = contentId,
                        Seo = i
                    });
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        #endregion


        #region ImageProccessing
        private static readonly HttpClient client = new HttpClient();
        [HttpPost]
        public async Task<JsonResult> GetTextFormImage(string imageString)
        {
            try
            {
                var values = new Dictionary<string, string>
                        {
                        { "imageString",imageString }
                        };

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("http://localhost:28724/Home/GetTextFormImage", content);

                var responseString = await response.Content.ReadAsStringAsync();

                return Json(new { success = true, data = responseString });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.InnerException != null ? ex.InnerException.ToString() : "Bir hata oluştu." });
            }
        }
        #endregion

    }
}