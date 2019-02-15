using AqApplication.Core.Type;
using AqApplication.Entity.Identity.Data;
using AqApplication.Repo.FilterModels;
using AqApplication.Repo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AqApplication.Repo.Session
{
    public interface IUser
    {
        Result<IEnumerable<ApplicationUser>> GetUsers(UserFilterModel filterModel);
        Result<UserInfoModel> GetUserInfo(string userId);
    }
}