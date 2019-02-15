using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AqApplication.Entity.Identity.Data;
using AqApplication.Manage.Models;
using System;

namespace AqApplication.Manage.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> SignInManager;
        private readonly UserManager<ApplicationUser> UserManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login()
        {

            ViewBag.ReturnUrl = "/Home/Index";
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        public JsonResult Login(LoginViewModel model, string returnUrl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Success = false, Message = Models.Utilities.Messages.FormNotValidError });
                }
                var result = SignInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                if (result.Result == Microsoft.AspNetCore.Identity.SignInResult.Success)
                    return Json(new { Success = true, ReturnUrl = returnUrl });
                else if (result.Result == Microsoft.AspNetCore.Identity.SignInResult.LockedOut)
                    return Json(new { Success = false, Message = Models.Utilities.Messages.UserLockError });
                else
                    return Json(new { Success = false, Message = Models.Utilities.Messages.NotValidUserError });
            }
            catch(Exception ex)
            {
                return Json(new { Success = false, Message = ex.InnerException!=null?ex.InnerException.ToString():"" });
            }
         

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
           
        }


        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        public  JsonResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };
                var result =  UserManager.CreateAsync(user, model.Password);
                if (result.Result.Succeeded)
                {
                    //await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                    
                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                     string code =  UserManager.GenerateEmailConfirmationTokenAsync(user).Result;
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Scheme);
                     //UserManager.email(user.Id, "A&Q Portal Hesab Doğrulama", "Hesabınızı dorğulamak için lütfen <a href=\"" + callbackUrl + "\">tklayınız</a>");

                    return Json(new { Success = true, Message = Models.Utilities.Messages.EPostaConfrim });
                }


                return Json(new { Success = false, Message = AddErrors(result.Result) });
            }

            // If we got this far, something failed, redisplay form
            return Json(new { Success=false,Message= Models.Utilities.Messages.FormNotValidError});
        }
        /// <summary>
        /// Identity Singin manager logout 
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOff()
        {
            SignInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        private string AddErrors(IdentityResult result)
        {
            var str = "";
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
                str += error;
            }
            return str;
        }
    }
}