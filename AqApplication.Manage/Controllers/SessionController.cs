using AnswerQuestionApp.Manage.Utilities;
using AqApplication.Entity.Identity.Data;
using AqApplication.Manage.Models;
using AqApplication.Manage.Utilities;
using AqApplication.Repository.FilterModels;
using AqApplication.Repository.Session;
using Cloudinary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;

namespace AqApplication.Manage.Controllers
{
    [Authorize]
    public class SessionController : Controller
    {
        private readonly IUser _iUser;
        private readonly SharedViewLocalizer _iLocalizer;
        public SessionController(SharedViewLocalizer iLocalizer, IUser iUser)
        {
            _iUser = iUser;
            _iLocalizer = iLocalizer;
        }
        // GET: Session
        public ActionResult Index(UserFilterModel model)
        {
            #region SearchModel
            var typeList = Models.Utilities.MemberTypeSelectList();
            var searhmodel = new SearchModel();
            searhmodel.Controller = "Session";
            searhmodel.Action = "Index";
            searhmodel.Position = SearchModelPosition.Vertical;
            searhmodel.SearchInput = new List<SearchInput>();
            searhmodel.SearchInput.Add(new SearchInput(AqApplication.Repository.Enums.InputType.Text, "Name", "nameSearchTxt", _iLocalizer["Name"]));
            searhmodel.SearchInput.Add(new SearchInput(AqApplication.Repository.Enums.InputType.Date, "StartDate", "startdateSearchTxt", _iLocalizer["StartDate"]));
            searhmodel.SearchInput.Add(new SearchInput(AqApplication.Repository.Enums.InputType.Date, "EndDate", "enddateSearchTxt", _iLocalizer["EndDate"]));
            searhmodel.SearchInput.Add(new SearchInput ( AqApplication.Repository.Enums.InputType.SelectList, "Type", "memberTypeCmb", _iLocalizer["MemberType"],selectList: typeList));
            ViewBag.searchModel = searhmodel;
            #endregion

            var result = _iUser.GetUsers(model);
            ViewBag.pagination = result.Paginition;
            return View(result.Data);
        }

        public IActionResult AddUser()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddUser(ApplicationUser model, IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                TempData["success"] = false;
                TempData["message"] = _iLocalizer["Error.ControlFields"];
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
                    model.ProfilPicture = uploadResult.Url;


                }
                catch (Exception ex)
                {
                    TempData["success"] = false;
                    TempData["message"] = "ERROR:" + ex.Message.ToString();
                    if (!string.IsNullOrEmpty(model.ProfilPicture)) // must be change
                    {
                        var currentImageFileName = Path.GetFileNameWithoutExtension(model.ProfilPicture);
                        var x = CdnHelper.GetUploader().Destroy(currentImageFileName);
                    }
                    return View(model);
                }
            }
            model.PhoneNumber = model.PhoneNumber.TrimTelnoMask();

            var result = _iUser.AddUser(model, User.GetUserId());
            TempData["success"] = result.Success;
            TempData["message"] = result.Message;

            return RedirectToAction("Index");
        }
        public IActionResult EditUser(string id)
        {
            var result = _iUser.GetUserByKey(id);
            if (!result.Success)
            {
                TempData["success"] = false;
                TempData["message"] = result.Message;
            }
            return View(result.Data);
        }
        [HttpPost]
        public IActionResult EditUser(ApplicationUser model, IFormFile file)
        {
            ModelState.Remove("PhoneNumber");
            if (!ModelState.IsValid)
            {
                TempData["success"] = false;
                TempData["message"] = _iLocalizer["Error.ControlFields"];
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
                    if (!string.IsNullOrEmpty(model.ProfilPicture)) // must be change
                    {
                        var currentImageFileName = Path.GetFileNameWithoutExtension(model.ProfilPicture);
                        var x = CdnHelper.GetUploader().Destroy(currentImageFileName);
                    }

                    model.ProfilPicture = uploadResult.Url;
                }
                catch (Exception ex)
                {
                    TempData["success"] = false;
                    TempData["message"] = "ERROR:" + ex.Message.ToString();

                    return View(model);
                }
            }
            model.PhoneNumber = model.PhoneNumber.TrimTelnoMask();
            var result = _iUser.EditUser(model, User.GetUserId());
            TempData["success"] = result.Success;
            TempData["message"] = result.Message;
            return RedirectToAction("Index");
        }

        public IActionResult BlockUser(string id)
        {
            var result = _iUser.BlockUser(id, User.GetUserId());
            TempData["success"] = result.Success;
            TempData["message"] = result.Message;
            return RedirectToAction("Index");
        }
    }
}