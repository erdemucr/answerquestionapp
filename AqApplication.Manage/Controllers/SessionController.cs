using AqApplication.Repository.FilterModels;
using AqApplication.Repository.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AqApplication.Manage.Controllers
{
    [Authorize]
    public class SessionController : Controller
    {
        private readonly IUser _iUser;
        public SessionController(IUser iUser)
        {
            _iUser = iUser;
        }
        // GET: Session
        public ActionResult Users(UserFilterModel model)
        {
            var list = _iUser.GetUsers(model);
            return View(list.Data);
        }
    }
}