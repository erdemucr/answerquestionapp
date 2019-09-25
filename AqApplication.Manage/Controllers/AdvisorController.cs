using AnswerQuestionApp.Entity.Advisor;
using AnswerQuestionApp.Manage.Utilities;
using AnswerQuestionApp.Repository.Advisor;
using AqApplication.Manage.Controllers;
using AqApplication.Manage.Models;
using AqApplication.Manage.Utilities;
using AqApplication.Repository.FilterModels;
using Cloudinary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AnswerQuestionApp.Manage.Controllers
{
    [Authorize]
    public class AdvisorController : BaseController
    {
        private readonly IAdvisor _iAdvisor;
        private readonly SharedViewLocalizer _iLocalizer;
        public AdvisorController(SharedViewLocalizer iLocalizer, IAdvisor iadvisor) : base()
        {
            _iLocalizer = iLocalizer;
            _iAdvisor = iadvisor;
        }
        public IActionResult Index(UserFilterModel model)
        {
            #region SearchModel
            var searhmodel = new SearchModel();
            searhmodel.Controller = "Advisor";
            searhmodel.Action = "Index";
            searhmodel.Position = SearchModelPosition.Vertical;
            searhmodel.SearchInput = new List<SearchInput>();
            searhmodel.SearchInput.Add(new SearchInput(AqApplication.Repository.Enums.InputType.Text, "Name", "nameSearchTxt", _iLocalizer["Name"]));
            searhmodel.SearchInput.Add(new SearchInput(AqApplication.Repository.Enums.InputType.Date, "StartDate", "startdateSearchTxt", _iLocalizer["StartDate"]));
            searhmodel.SearchInput.Add(new SearchInput(AqApplication.Repository.Enums.InputType.Date, "EndDate", "enddateSearchTxt", _iLocalizer["EndDate"]));
            ViewBag.searchModel = searhmodel;
            #endregion

            var result = _iAdvisor.GetAdvisors(model);
            ViewBag.pagination = result.Paginition;

            return View(result.Data);
        }
        public IActionResult AddAdvisor()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddAdvisor(Advisor model, IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                TempData["success"] = false;
                TempData["message"] = _iLocalizer["Error.ControlFields"].ToString();
                return View(model);
            }
            if (file != null && file.Length > 0)
            {
                try
                {
                    string type = file.ContentType;
                    var list = new List<string>() { "image/jpg", "image/png", "image/jpeg" };
                    if (!list.Contains(type))
                    {
                        TempData["success"] = false;
                        TempData["message"] = "Sadece jpg, jpeg, png uzantılı dosyalar yukleyebilirsiniz!";
                        return View(model);
                    }
                    var uploadResult = CdnHelper.GetUploader()
                       .Upload(new UploadInformation(Guid.NewGuid().ToString(), file.OpenReadStream()));


                    if (!string.IsNullOrEmpty(uploadResult.Error) || string.IsNullOrEmpty(uploadResult.Url))
                    {
                        TempData["success"] = false;
                        TempData["message"] = "Cdn'e resim yuklenemedi!";
                        return View(model);
                    }
                    model.PhotoUrl = uploadResult.Url;


                }
                catch (Exception ex)
                {
                    TempData["success"] = false;
                    TempData["message"] = "ERROR:" + ex.Message.ToString();
                    if (!string.IsNullOrEmpty(model.PhotoUrl)) // must be change
                    {
                        var currentImageFileName = Path.GetFileNameWithoutExtension(model.PhotoUrl);
                        var x = CdnHelper.GetUploader().Destroy(currentImageFileName);
                    }
                    return View(model);
                }
            }
            model.ApplicationUser.Email = model.Email;
            model.ApplicationUser.PhoneNumber = model.PhoneNumber.TrimTelnoMask();

            var result = _iAdvisor.AddAdvisor(model, User.GetUserId());
            TempData["success"] = result.Success;
            TempData["message"] = result.Message;

            return RedirectToAction("Index");
        }
        public IActionResult EditAdvisor(int id)
        {
            var result = _iAdvisor.GetAdvisorByKey(id);
            if (!result.Success)
            {
                TempData["success"] = false;
                TempData["message"] = result.Message;
            }
            result.Data.Email = result.Data.ApplicationUser.Email;
            result.Data.PhoneNumber = result.Data.ApplicationUser.PhoneNumber;
            return View(result.Data);
        }
        [HttpPost]
        public IActionResult EditAdvisor(Advisor model, IFormFile file)
        {
            ModelState.Remove("Password");
            if (!ModelState.IsValid)
            {
                TempData["success"] = false;
                TempData["message"] = _iLocalizer["Error.ControlFields"].ToString();
                return View(model);
            }
            //once resmi upload etmeye calisalim
            if (file != null && file.Length > 0)
            {
                try
                {
                    string type = file.ContentType;
                    var list = new List<string>() { "image/jpg", "image/png", "image/jpeg" };
                    if (!list.Contains(type))
                    {
                        TempData["success"] = false;
                        TempData["message"] = "Sadece jpg, jpeg, png uzantılı dosyalar yukleyebilirsiniz!";
                        return View(model);
                    }
                    var uploadResult = CdnHelper.GetUploader()
                        .Upload(new UploadInformation(Guid.NewGuid().ToString(), file.OpenReadStream()));

                    if (!string.IsNullOrEmpty(uploadResult.Error) || string.IsNullOrEmpty(uploadResult.Url))
                    {
                        TempData["success"] = false;
                        TempData["message"] = "Cdn'e resim yuklenemedi!";
                        return View(model);
                    }
                    if (!string.IsNullOrEmpty(model.PhotoUrl)) // must be change
                    {
                        var currentImageFileName = Path.GetFileNameWithoutExtension(model.PhotoUrl);
                        var x = CdnHelper.GetUploader().Destroy(currentImageFileName);
                    }

                    model.PhotoUrl = uploadResult.Url;
                }
                catch (Exception ex)
                {
                    TempData["success"] = false;
                    TempData["message"] = "ERROR:" + ex.Message.ToString();

                    return View(model);
                }
            }
            model.ApplicationUser.PhoneNumber = model.PhoneNumber.TrimTelnoMask();
            model.ApplicationUser.Email = model.Email;
            var result = _iAdvisor.EditAdvisor(model, User.GetUserId());
            TempData["success"] = result.Success;
            TempData["message"] = result.Message;
            return RedirectToAction("Index");
        }

        public IActionResult DeleteAdvisor(int id)
        {
            var result = _iAdvisor.DeleteAdvisor(id, User.GetUserId());
            TempData["success"] = result.Success;
            TempData["message"] = result.Message;
            return RedirectToAction("Index");
        }
    }
}