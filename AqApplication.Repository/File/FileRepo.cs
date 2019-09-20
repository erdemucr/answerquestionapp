using AqApplication.Core.Type;
using AqApplication.Entity.Identity.Data;
using AqApplication.Entity.Question;
using AqApplication.Repository.FilterModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AqApplication.Repository.File
{
    public class FileRepo : IFile, IDisposable
    {
        private readonly int PageSize = 20;
        private ApplicationDbContext context;
        private bool disposedValue = false;
        public FileRepo(ApplicationDbContext _context)
        {
            context = _context;
        }

        public Result<IEnumerable<QuestionPdf>> GetQuestionPdf(DocumentFilterModel model)
        {
            try
            {
                var list = context.QuestionPdfs.AsQueryable();
                if (!string.IsNullOrEmpty(model.Name))
                {
                    list.Where(x => x.Name.ToLower().Contains(model.Name.ToLower()));
                }
                list
                    .Include(x => x.AppUserCreator)
                    .Include(x => x.AppUserEditor);

                return new Result<IEnumerable<QuestionPdf>>
                {
                    Data = list.AsEnumerable(),
                    Success = true,
                    Message = "Soru pdf döküman listesini görüntülemektesiniz",
                };

            }
            catch (Exception ex)
            {
                return new Result<IEnumerable<QuestionPdf>>(ex);
            }

        }
        public Result<QuestionPdf> GetQuestionPdf(int id)
        {
            try
            {
                var item = context.QuestionPdfs.FirstOrDefault(x => x.Id == id);
                if (item == null)
                {
                    return new Result<QuestionPdf> { Success = false, Message = "Bir hata oluştu" };
                }
                return new Result<QuestionPdf> { Success = true, Message = "Döküman", Data = item };

            }
            catch (Exception ex)
            {
                return new Result<QuestionPdf>(ex);
            }

        }

        public Result AddQuestionPdf(QuestionPdf model, string userId)
        {
            try
            {
                model.Creator = userId;
                context.QuestionPdfs.Add(model);
                var result = context.SaveChanges();
                return new Result { Success = true, Message = "Yeni pdf döküman başarı ile eklenişmiştir", InstertedId = model.Id };
            }
            catch (Exception ex)
            {
                new Result(ex);
            }
            return new Result { Success = false, Message = "Bir hata oluştu" };
        }

        public Result AddQuestionContentPdf(QuestionPdfContent model)
        {
            try
            {
                context.QuestionPdfContent.Add(model);
                context.SaveChanges();
                return new Result { Success = true, Message = "Yeni pdf döküman içeriği başarı ile eklenişmiştir" };
            }
            catch (Exception ex)
            {
                new Result(ex);
            }
            return new Result { Success = false, Message = "Bir hata oluştu" };
        }



        public Result<IEnumerable<QuestionPdfContent>> GetQuestionPdfContents(PdfContentFilterModel model)
        {
            try
            {
                var list = context.QuestionPdfContent.AsQueryable()
                    .Where(x => x.QuestionId == model.Id)
                    .AsEnumerable();
                return new Result<IEnumerable<QuestionPdfContent>>
                {
                    Data = list.OrderBy(x => x.Seo).Skip(model.CurrentPage.HasValue ? ((model.CurrentPage.Value - 1) * PageSize) : 0).Take(PageSize).AsEnumerable(),
                    Success = true,
                    Message = "Soru pdf içerik listesini görüntülemektesiniz",
                    Paginition = new Paginition(list.Count(), PageSize, model.CurrentPage.HasValue ? model.CurrentPage.Value : 1, null, null, null, model.Id)
                };

            }
            catch (Exception ex)
            {
                return new Result<IEnumerable<QuestionPdfContent>>(ex);
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