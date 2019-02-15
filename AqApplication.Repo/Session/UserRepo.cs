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
    public class UserRepo: IUser
    {
        private ApplicationDbContext context;
        private bool disposedValue = false;
        public UserRepo()
        {
            context = new ApplicationDbContext();
        }

        public Result<IEnumerable<ApplicationUser>> GetUsers(UserFilterModel filterModel)
        {
            try
            {
                var list = context.Users.AsQueryable();
                if (!string.IsNullOrEmpty(filterModel.Name))
                    list.Where(x => (x.FirstName + " " + x.LastName).ToLower().Contains(filterModel.Name.ToLower()));
                //if(filterModel.StartDate.HasValue)
                //    list.Where(x=>x.reg)




                    list.AsEnumerable();
                return new Result<IEnumerable<ApplicationUser>>
                {
                    Data = list,
                    Success = true,
                    Message = "Soru listesini görüntülemektesiniz"
                };

            }
            catch (Exception ex)
            {
                return new Result<IEnumerable<ApplicationUser>>(ex);
            }
        }

        public Result<UserInfoModel> GetUserInfo(string userId)
        {
            try
            {
                var model = context.Users.Where(x => x.Id == userId).FirstOrDefault();
                if (model == null)
                    return new Result<UserInfoModel>
                    {
                        Success = false,
                        Message = "Kullanıcı bulunamadı"
                    };


                return new Result<UserInfoModel>
                {
                    Data = new UserInfoModel { Email=model.Email,FirstName=model.FirstName,LastName=model.LastName},
                    Success = true,
                    Message = "Soru listesini görüntülemektesiniz"
                };

            }
            catch (Exception ex)
            {
                return new Result<UserInfoModel>(ex);
            }
        }
    }
}