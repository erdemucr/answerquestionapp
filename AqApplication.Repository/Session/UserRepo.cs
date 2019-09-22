using AqApplication.Core.Type;
using AqApplication.Entity.Constants;
using AqApplication.Entity.Identity.Data;
using AqApplication.Repository.FilterModels;
using AqApplication.Repository.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AqApplication.Repository.Session
{
    public class UserRepo : IUser
    {
        private readonly int PageSize = 20;
        private ApplicationDbContext context;
        private bool disposedValue = false;
        private readonly UserManager<ApplicationUser> UserManager;
        public UserRepo(ApplicationDbContext _context, UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
            context = _context;
        }
        public Result<IEnumerable<ApplicationUser>> GetUsers(UserFilterModel model)
        {
            try
            {
                var list = context.Users.AsQueryable();
                if (!string.IsNullOrEmpty(model.Name))
                    list.Where(x => (x.FirstName + " " + x.LastName).ToLower().Contains(model.Name.ToLower()));
                if (model.StartDate.ToDate().HasValue)
                    list = list.Where(x => x.RegisterDate >= model.StartDate.ToDate().Value);
                if (model.EndDate.ToDate().HasValue)
                    list = list.Where(x => x.RegisterDate < model.EndDate.ToDate().Value);
                if (model.Type.HasValue)
                    list = list.Where(x => x.MemberType == (MemberType)model.Type.Value);

                list.AsEnumerable();
                return new Result<IEnumerable<ApplicationUser>>
                {
                    Data = list.OrderByDescending(x => x.Id).Skip(model.CurrentPage.HasValue ? ((model.CurrentPage.Value - 1) * PageSize) : 0).Take(PageSize).AsEnumerable(),
                    Success = true,
                    Message = "Kullanıcı listesini görüntülemektesiniz",
                    Paginition = new Paginition(list.Count(), PageSize, model.CurrentPage.HasValue ? model.CurrentPage.Value : 1, model.Name,
                    model.StartDate,
                   model.EndDate,type: model.Type)
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
                        Message = "Kullanıcı bulunamadı",
                        MessageType = MessageType.RecordNotFound
                    };


                return new Result<UserInfoModel>
                {
                    Data = new UserInfoModel { Email = model.Email, FirstName = model.FirstName, LastName = model.LastName },
                    Success = true,
                    Message = "User listesini görüntülemektesiniz",
                    MessageType= MessageType.OperationCompleted
                };

            }
            catch (Exception ex)
            {
                return new Result<UserInfoModel>(ex);
            }
        }

        public Result BlockUser(string userId, string editor)
        {
            try
            {
                ApplicationUser user = UserManager.FindByIdAsync(userId).Result;
                context.Entry(user).State = EntityState.Modified;
                user.IsBlocked = true;
                user.SecurityStamp = Guid.NewGuid().ToString();

                var result = UserManager.UpdateAsync(user);

                return new Result { Success = true, Message = "Yeni advisor başarı ile güncellenmiştir", MessageType = MessageType.UpdateSuccess };
            }
            catch (Exception ex)
            {
                return new Result(ex);
            }
        }

        public Result AddUser(ApplicationUser model, string userId)
        {
            try
            {
                var userCnt = UserManager.FindByEmailAsync(model.Email);
                if (userCnt.Result != null)
                {
                    if (userCnt.Result.IsBlocked.HasValue && userCnt.Result.IsBlocked.Value)
                        return new Result { Success = false, Message = "Bu hesap blokelidir. Lütfen sistem yöneticisinden destek alınız.", MessageType = MessageType.AccountBlocked };
                    else
                        return new Result { Success = false, Message = "Bu e-posta adresi kullanımdadır!", MessageType = MessageType.EmailAlreadyUsed };
                }


                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    MemberType = AqApplication.Entity.Constants.MemberType.Advisor,
                    Creator= userId
                };
                var result = UserManager.CreateAsync(user, model.Password);
                if (result.Result.Succeeded)
                    return new Result { Success = true, Message = "Yeni user başarı ile eklenişmiştir", MessageType = MessageType.InsertSuccess };
                return new Result { Success = false, Message = "Bir hata oluştu", MessageType = MessageType.InsertFailed };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Result<ApplicationUser> GetUserByKey(string id)
        {
            var model = context.Users.Include(x => x.AppUserCreator).FirstOrDefault(x => x.Id == id);
            if (model == null)
                return new Result<ApplicationUser> { Success = false, Message = "Kayıt bulunamadı", MessageType = MessageType.RecordNotFound };
            return new Result<ApplicationUser> { Success = true, Message = "İşlem başarılı", Data = model, MessageType = MessageType.OperationCompleted };
        }

        public Result EditUser(ApplicationUser model, string userId)
        {
            try
            {
                var editModel = context.Users.FirstOrDefault(x => x.Id == model.Id);
                if (editModel == null)
                {
                    return new Result { Success = false, Message = "Kayıt bulunamadı", MessageType = MessageType.RecordNotFound };
                }
                ApplicationUser user = UserManager.FindByIdAsync(model.Id).Result;

                context.Entry(user).State = EntityState.Modified;


                user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.PhoneNumber = model.PhoneNumber;
                user.SecurityStamp = Guid.NewGuid().ToString();

                var result = UserManager.UpdateAsync(user);
                if (result.Result.Succeeded)
                    return new Result { Success = true, Message = "Yeni advisor başarı ile güncellenmiştir", MessageType = MessageType.UpdateSuccess };

                return new Result { Success = false, Message = "Bir hata oluştu", MessageType = MessageType.InsertFailed };

            }
            catch (Exception ex)
            {
                return new Result(ex);
            }
        }
       
    }
}