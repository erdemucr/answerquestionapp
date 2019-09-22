using AqApplication.Core.Type;
using AqApplication.Entity.Identity.Data;
using AqApplication.Repository.FilterModels;
using AqApplication.Repository.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AqApplication.Repository.Session
{
    public interface IUser
    {
        Result<IEnumerable<ApplicationUser>> GetUsers(UserFilterModel filterModel);
        Result<UserInfoModel> GetUserInfo(string userId);

        Result BlockUser(string userId, string editor);

        Result AddUser(ApplicationUser model, string userId);

        Result<ApplicationUser> GetUserByKey(string id);

        Result EditUser(ApplicationUser model, string userId);
    }
}