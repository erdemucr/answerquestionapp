using AqApplication.Core.Type;
using AqApplication.Entity.Identity.Data;
using AqApplication.Repository.FilterModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnswerQuestionApp.Entity.Advisor;
using Microsoft.AspNetCore.Identity;
using AqApplication.Repository.Session;

namespace AnswerQuestionApp.Repository.Advisor
{
    public class AdvisorRepo : IAdvisor, IDisposable
    {
        private readonly int PageSize = 20;
        private ApplicationDbContext context;
        private bool disposedValue = false;
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly IUser _iUser;
        public AdvisorRepo(ApplicationDbContext _context, UserManager<ApplicationUser> userManager, IUser iUser)
        {
            UserManager = userManager;
            context = _context;
            _iUser = iUser;
        }

        public Result<IEnumerable<Entity.Advisor.Advisor>> GetAdvisors(UserFilterModel model)
        {
            try
            {
                var list = context.Advisor.Include(x => x.ApplicationUser).Include(x => x.AppUserCreator).Include(x => x.AppUserEditor).AsQueryable();
                list = list.Where(x => (!x.IsDeleted.HasValue || (x.IsDeleted.HasValue && !x.IsDeleted.Value)));
                if (!string.IsNullOrEmpty(model.Name))
                    list.Where(x => (x.ApplicationUser.FirstName + " " + x.ApplicationUser.LastName).ToLower().Contains(model.Name.ToLower()));

                return new Result<IEnumerable<Entity.Advisor.Advisor>>
                {
                    Data = list.AsEnumerable().OrderByDescending(x => x.Id).Skip(model.CurrentPage.HasValue ? ((model.CurrentPage.Value - 1) * PageSize) : 0).Take(PageSize).AsEnumerable(),
                    Success = true,
                    Paginition = new Paginition(list.Count(), PageSize, model.CurrentPage.HasValue ? model.CurrentPage.Value : 1, model.Name,
                    model.StartDate,
                   model.EndDate)
                };
            }
            catch (Exception ex)
            {
                return new Result<IEnumerable<Entity.Advisor.Advisor>>(ex);
            }
        }


        public Result AddAdvisor(Entity.Advisor.Advisor model, string userId)
        {
            try
            {
                var userCnt = UserManager.FindByEmailAsync(model.ApplicationUser.Email);
                if (userCnt.Result != null)
                {
                    if (userCnt.Result.IsBlocked.HasValue && userCnt.Result.IsBlocked.Value)
                        return new Result { Success = false, Message = "Bu hesap blokelidir. Lütfen sistem yöneticisinden destek alınız.", MessageType = MessageType.AccountBlocked };
                    else
                        return new Result { Success = false, Message = "Bu e-posta adresi kullanımdadır!", MessageType = MessageType.EmailAlreadyUsed };
                }


                var user = new ApplicationUser
                {
                    UserName = model.ApplicationUser.Email,
                    Email = model.ApplicationUser.Email,
                    FirstName = model.ApplicationUser.FirstName,
                    LastName = model.ApplicationUser.LastName,
                    PhoneNumber = model.ApplicationUser.PhoneNumber,
                    MemberType = AqApplication.Entity.Constants.MemberType.Advisor,
                    RegisterDate= DateTime.Now,
                    ProfilPicture= model.PhotoUrl
                };
                var result = UserManager.CreateAsync(user, model.Password);
                if (result.Result.Succeeded)
                {
                    model.UserId = user.Id;
                    model.ApplicationUser = null;
                    model.Creator = userId;
                    model.CreatedDate = DateTime.Now;
                    context.Advisor.Add(model);
                    context.SaveChanges();
                }

                return new Result { Success = true, Message = "Yeni advisor başarı ile eklenişmiştir" };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Result<Entity.Advisor.Advisor> GetAdvisorByKey(int id)
        {
            var model = context.Advisor.Include(x => x.ApplicationUser).FirstOrDefault(x => x.Id == id);
            if (model == null)
                return new Result<Entity.Advisor.Advisor> { Success = false, Message = "Advisor bulunamadı" };
            return new Result<Entity.Advisor.Advisor> { Success = true, Message = "İşlem başarılı", Data = model };
        }

        public Result EditAdvisor(Entity.Advisor.Advisor model, string userId)
        {
            try
            {
                var editModel = context.Advisor.FirstOrDefault(x => x.Id == model.Id);
                if (editModel == null)
                {
                    return new Result { Success = false, Message = "Kayıt bulunamadı" };
                }
                ApplicationUser user = UserManager.FindByIdAsync(editModel.UserId).Result;

                context.Entry(user).State = EntityState.Modified;

                user.UserName = model.ApplicationUser.Email;
                user.Email = model.ApplicationUser.Email;
                user.FirstName = model.ApplicationUser.FirstName;
                user.LastName = model.ApplicationUser.LastName;
                user.PhoneNumber = model.ApplicationUser.PhoneNumber;
                user.SecurityStamp = Guid.NewGuid().ToString();
                user.ProfilPicture = model.PhotoUrl;

                var result = UserManager.UpdateAsync(user);
                if (result.Result.Succeeded)
                {
                    editModel.PhotoUrl = model.PhotoUrl;
                    editModel.IsActive = model.IsActive;
                    editModel.Editor = userId;
                    editModel.ModifiedDate = DateTime.Now;
                    context.Entry(editModel).State = EntityState.Modified;
                    context.SaveChanges();
                }

                return new Result { Success = true, Message = "Yeni advisor başarı ile güncellenmiştir", MessageType = MessageType.UpdateSuccess };
            }
            catch (Exception ex)
            {
                return new Result(ex);
            }
        }
        public Result DeleteAdvisor(int id, string userId)
        {
            try
            {
                var editModel = context.Advisor.FirstOrDefault(x => x.Id == id);
                if (editModel == null)
                {
                    return new Result { Success = false, Message = "Kayıt bulunamadı" };
                }
                _iUser.BlockUser(editModel.UserId, userId);

                editModel.IsActive = false;
                editModel.IsDeleted = true;
                editModel.Editor = userId;
                editModel.ModifiedDate = DateTime.Now;
                context.Entry(editModel).State = EntityState.Modified;
                context.SaveChanges();

                return new Result { Success = true, Message = "Yeni advisor başarı ile silinmiştir", MessageType = MessageType.DeleteSuccess };
            }
            catch (Exception ex)
            {
                return new Result(ex);
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    context = null;
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);

        }
    }
}
