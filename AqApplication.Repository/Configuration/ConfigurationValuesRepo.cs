using AnswerQuestionApp.Entity.Configuration;
using AqApplication.Core.Type;
using AqApplication.Entity.Identity.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnswerQuestionApp.Repository.Configuration
{
    public class ConfigurationValuesRepo : IConfigurationValues
    {
        private ApplicationDbContext context;
        private bool disposedValue = false;
        private const int challengeExpDay = 12, challengeQuestionLimit = 8;
        public ConfigurationValuesRepo(ApplicationDbContext _context)
        {
            context = _context;
        }

        public Result<Dictionary<ConfigKey, string>> GetByKey(ConfigKey key)
        {
            try
            {
                var question = context.ConfigurationValues.FirstOrDefault(x => x.Key == key);

                if (question == null)
                    return new Result<Dictionary<ConfigKey, string>>
                    {
                        Success = false,
                        Message = "Bir hata oluştu"
                    };


                return new Result<Dictionary<ConfigKey, string>>
                {
                    Data = new Dictionary<ConfigKey, string> { { question.Key, question.Values } },
                    Success = true,
                    Message = "İşlem başarı ile tamamlandı"
                };
            }

            catch (Exception ex)
            {
                return new Result<Dictionary<ConfigKey, string>>(ex);
            }

        }

        public Result<IEnumerable<ConfigurationValues>> GetAll()
        {
            try
            {
                var question = context.ConfigurationValues.AsEnumerable();

                if (question == null)
                    return new Result<IEnumerable<ConfigurationValues>>
                    {
                        Success = false,
                        Message = "Bir hata oluştu"
                    };


                return new Result<IEnumerable<ConfigurationValues>>
                {
                    Data = question,
                    Success = true,
                    Message = "İşlem başarı ile tamamlandı"
                };
            }

            catch (Exception ex)
            {
                return new Result<IEnumerable<ConfigurationValues>>(ex);
            }

        }

        public Result Edit(List<Entity.Configuration.ConfigurationValues> list)
        {
            try
            {
                 foreach(var item in list)
                {
                    var model = context.ConfigurationValues.FirstOrDefault( x=> x.Id== item.Id);
                    if(model!=null)
                    {
                        model.Values = item.Values;
                        context.Entry(model).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }

                return new Result { Success = true, Message = "Değişiklikler başarı ile kayıt edilmiştir" };
            }
            catch (Exception ex)
            {
                new Result(ex);
            }
            return new Result { Success = false, Message = "Bir hata oluştu" };
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
