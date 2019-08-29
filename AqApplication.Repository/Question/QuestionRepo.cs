using AnswerQuestionApp.Repository.FilterModels;
using AqApplication.Core.Type;
using AqApplication.Entity.Challenge;
using AqApplication.Entity.Identity.Data;
using AqApplication.Entity.Question;
using AqApplication.Repository.FilterModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace AqApplication.Repository.Question
{
    public class QuestionRepo : IQuestion, IDisposable
    {
        private readonly int PageSize = 20;
        private ApplicationDbContext context;
        private bool disposedValue = false;
        public QuestionRepo(ApplicationDbContext _context)
        {
            context = _context;
        }

        public Result SaveQuestion(QuestionMain model)
        {
            var result = 0;
            try
            {
                context.QuestionMain.Add(model);
                result = context.SaveChanges();
            }

            catch (Exception ex)
            {
                return new Result(ex);
            }
            return new Result
            {
                InstertedId = model.Id,
                Success = (result > 0),
                Message = (result > 0) ? "Yeni soru başarı ile eklenilmiştir" : "Bir hata oluştu"
            };
        }
        public Result<IEnumerable<QuestionMain>> GetQuestion(QuestionFilterModel model)
        {
            try
            {
                var list = context.QuestionMain
                    .Include(x => x.AppUserCreator)
                    .Include(x => x.AppUserEditor).AsQueryable();
                if (!string.IsNullOrEmpty(model.Name))
                    list = list.Where(x => x.MainTitle.ToLower().Contains(model.Name));
                if (model.StartDate.ToDate().HasValue)
                    list = list.Where(x => x.CreatedDate >= model.StartDate.ToDate().Value);
                if (model.EndDate.ToDate().HasValue)
                    list = list.Where(x => x.CreatedDate < model.EndDate.ToDate().Value);

                return new Result<IEnumerable<QuestionMain>>
                {
                    Data = list.OrderByDescending(x => x.Id).Skip(model.CurrentPage.HasValue ? ((model.CurrentPage.Value - 1) * PageSize) : 0).Take(PageSize).AsEnumerable(),
                    Success = true,
                    Message = "Soru listesini görüntülemektesiniz",
                    Paginition = new Paginition(list.Count(), PageSize, model.CurrentPage.HasValue ? model.CurrentPage.Value : 1, model.Name,
                    model.StartDate,
                   model.EndDate)
                };
            }
            catch (Exception ex)
            {
                new Result<IEnumerable<QuestionMain>>(ex);
            }
            return new Result<IEnumerable<QuestionMain>>
            {
                Success = false,
                Message = "Bir hata oluştu"
            };

        }

        public Result<IEnumerable<QuestionAnswer>> GetAnswers(int questionId)
        {
            try
            {
                var list = context.QuestionAnswers.Where(x => x.QuestionId == questionId)
                    .AsEnumerable();
                return new Result<IEnumerable<QuestionAnswer>>
                {
                    Data = list,
                    Success = true,
                    Message = "Soru listesini görüntülemektesiniz"
                };

            }
            catch (Exception ex)
            {
                return new Result<IEnumerable<QuestionAnswer>>(ex);
            }


        }


        public Result DeleteQuestion(int questionId)
        {
            var result = 0;
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    var model = context.QuestionMain.FirstOrDefault(x => x.Id == questionId);
                    var questionAnswers = context.QuestionAnswers.Where(x => x.QuestionId == questionId);
                    foreach (var item in questionAnswers)
                    {
                        context.Entry(item).State = EntityState.Deleted;
                    }
                    context.SaveChanges();
                    var questionClass = context.QuestionClass.Where(x => x.QuestionId == questionId);
                    foreach (var item in questionClass)
                    {
                        context.Entry(item).State = EntityState.Deleted;
                    }
                    context.SaveChanges();
                    var questionExams = context.QuestionExams.Where(x => x.QuestionId == questionId);
                    foreach (var item in questionExams)
                    {
                        context.Entry(item).State = EntityState.Deleted;
                    }
                    context.SaveChanges();
                    var questionPdfContent = context.QuestionPdfContent.Where(x => x.QuestionId == questionId);
                    foreach (var item in questionPdfContent)
                    {
                        context.Entry(item).State = EntityState.Deleted;
                    }
                    context.SaveChanges();
                    context.Entry(model).State = EntityState.Deleted;
                    context.SaveChanges();
                    transaction.Commit();
                }

                return new Result
                {
                    Success = true,
                    Message = "Kayıt başarı ile silinmiştir"
                };
            }

            catch (Exception ex)
            {
                return new Result(ex);
            }

        }


        #region Lecture

        public Result<IEnumerable<Lecture>> GetLectures()
        {
            try
            {
                var list = context.Lectures.Include(x => x.AppUserCreator)
                        .Include(x => x.AppUserEditor).AsEnumerable();


                return new Result<IEnumerable<Lecture>>
                {
                    Data = list,
                    Success = true,
                    Message = "Branş listesini görüntülemektesiniz"
                };
            }
            catch (Exception ex)
            {
                new Result<IEnumerable<QuestionMain>>(ex);
            }

            return new Result<IEnumerable<Lecture>>
            {
                Success = false,
                Message = "Bir hata oluştu"
            };
        }


        public Result AddLecture(Lecture model, string userId)
        {
            try
            {
                model.Creator = userId;
                context.Lectures.Add(model);
                context.SaveChanges();
                return new Result { Success = true, Message = "Yeni ders başarı ile eklenişmiştir" };
            }
            catch (Exception ex)
            {
                new Result(ex);
            }
            return new Result { Success = false, Message = "Bir hata oluştu" };
        }

        public Result<Lecture> GetLectureByKey(int id)
        {

            var model = context.Lectures.FirstOrDefault(x => x.Id == id);
            if (model == null)
                return new Result<Lecture> { Success = false, Message = "Branş bulunamadı" };
            return new Result<Lecture> { Success = true, Message = "İşlem başarılı", Data = model };
        }


        public Result SetLectureStatus(int id, string userId)
        {
            try
            {
                var model = context.Lectures.FirstOrDefault(x => x.Id == id);
                if (model == null)
                    return new Result { Success = false, Message = "Branş bulunamadı" };
                bool currentStatus = model.IsActive;
                model.IsActive = !model.IsActive;
                model.Editor = userId;
                context.Entry(model).State = EntityState.Modified;
                context.SaveChanges();
                return new Result { Success = true, Message = string.Format("Yeni {0} ile {1} edilmiştir", "branş", currentStatus ? "pasif" : "aktif") };
            }
            catch (Exception ex)
            {
                new Result(ex);
            }
            return new Result { Success = false, Message = "Bir hata oluştu" };
        }

        public Result EditLecture(Lecture model, string userId)
        {
            try
            {
                var editmodel = context.Lectures.FirstOrDefault(x => x.Id == model.Id);
                if (model == null)
                    return new Result { Success = false, Message = "Branş bulunamadı" };
                bool currentStatus = model.IsActive;
                editmodel.Name = model.Name;
                editmodel.IsActive = model.IsActive;
                editmodel.Editor = userId;
                context.Entry(editmodel).State = EntityState.Modified;
                context.SaveChanges();
                return new Result { Success = true, Message = string.Format("Yeni {0} ile {1}", "branş", "güncellenmiştir") };
            }
            catch (Exception ex)
            {
                new Result(ex);
            }
            return new Result { Success = false, Message = "Bir hata oluştu" };
        }

        public Result DeleteLecture(int id)
        {
            try
            {
                var deletemodel = context.Lectures.FirstOrDefault(x => x.Id == id);
                if (deletemodel == null)
                    return new Result { Success = false, Message = "Branş bulunamadı" };


                context.Entry(deletemodel).State = EntityState.Deleted;
                context.SaveChanges();
                return new Result { Success = true, Message = string.Format("Branş başarı ile silinmiştir") };
            }
            catch (Exception ex)
            {
                new Result(ex);
            }
            return new Result { Success = false, Message = "Bir hata oluştu" };
        }

        #endregion

        #region Subject

        public Result AddSubject(Subject model, string userId)
        {
            try
            {
                model.Creator = userId;
                context.Subjects.Add(model);
                context.SaveChanges();
                return new Result { Success = true, Message = "Yeni konu başarı ile eklenişmiştir" };
            }
            catch (Exception ex)
            {
                new Result(ex);
            }
            return new Result { Success = false, Message = "Bir hata oluştu" };
        }
        public Result<IEnumerable<Subject>> GetSubjects()
        {
            try
            {
                var list = context.Subjects
                    .Include(x => x.AppUserCreator)
                    .Include(x => x.AppUserEditor)
                    .Include(x => x.Lecture)
                    .AsEnumerable();

                return new Result<IEnumerable<Subject>>
                {
                    Data = list,
                    Success = true,
                    Message = "Konu listesini görüntülemektesiniz"
                };
            }
            catch (Exception ex)
            {
                new Result<IEnumerable<QuestionMain>>(ex);
            }

            return new Result<IEnumerable<Subject>>
            {
                Success = false,
                Message = "Bir hata oluştu"
            };
        }
        public Result<IEnumerable<Subject>> GetSubjectsByLectureId(int lectureId)
        {
            try
            {
                var list = context.Subjects
                    .Where(x => x.LectureId == lectureId)
                    .AsEnumerable();
                return new Result<IEnumerable<Subject>>
                {
                    Data = list,
                    Success = true,
                    Message = "Konu listesini görüntülemektesiniz"
                };
            }
            catch (Exception ex)
            {
                new Result<IEnumerable<QuestionMain>>(ex);
            }

            return new Result<IEnumerable<Subject>>
            {
                Success = false,
                Message = "Bir hata oluştu"
            };
        }

        public Result<Subject> GetSubjectByKey(int id)
        {

            var model = context.Subjects.FirstOrDefault(x => x.Id == id);
            if (model == null)
                return new Result<Subject> { Success = false, Message = "Konu bulunamadı" };
            return new Result<Subject> { Success = true, Message = "İşlem başarılı", Data = model };
        }


        public Result SetSubjectStatus(int id, string userId)
        {
            try
            {
                var model = context.Lectures.FirstOrDefault(x => x.Id == id);
                if (model == null)
                    return new Result { Success = false, Message = "Konu bulunamadı" };
                bool currentStatus = model.IsActive;
                model.IsActive = !model.IsActive;
                model.Editor = userId;
                context.Entry(model).State = EntityState.Modified;
                context.SaveChanges();
                return new Result { Success = true, Message = string.Format("Yeni {0} ile {1} edilmiştir", "konu", currentStatus ? "pasif" : "aktif") };
            }
            catch (Exception ex)
            {
                new Result(ex);
            }
            return new Result { Success = false, Message = "Bir hata oluştu" };
        }

        public Result EditSubject(Subject model, string userId)
        {
            try
            {
                var editmodel = context.Subjects.FirstOrDefault(x => x.Id == model.Id);
                if (model == null)
                    return new Result { Success = false, Message = "Konu bulunamadı" };
                bool currentStatus = model.IsActive;
                editmodel.Name = model.Name;
                editmodel.IsActive = model.IsActive;
                editmodel.Editor = userId;
                editmodel.LectureId = model.LectureId;
                context.Entry(editmodel).State = EntityState.Modified;
                context.SaveChanges();
                return new Result { Success = true, Message = string.Format("Yeni {0} ile {1}", "konu", "güncellenmiştir") };
            }
            catch (Exception ex)
            {
                new Result(ex);
            }
            return new Result { Success = false, Message = "Bir hata oluştu" };
        }

        public Result DeleteSubject(int id)
        {
            try
            {
                var deletemodel = context.Subjects.FirstOrDefault(x => x.Id == id);
                if (deletemodel == null)
                    return new Result { Success = false, Message = "Konu bulunamadı" };


                context.Entry(deletemodel).State = EntityState.Deleted;
                context.SaveChanges();
                return new Result { Success = true, Message = string.Format("Konu başarı ile silinmiştir") };
            }
            catch (Exception ex)
            {
                new Result(ex);
            }
            return new Result { Success = false, Message = "Bir hata oluştu" };
        }

        #endregion

        #region Exam

        public Result AddExam(Exam model, string userId)
        {
            try
            {
                model.Creator = userId;
                context.Exams.Add(model);
                context.SaveChanges();
                return new Result { Success = true, Message = "Yeni sınav başarı ile eklenişmiştir" };
            }
            catch (Exception ex)
            {
                new Result(ex);
            }
            return new Result { Success = false, Message = "Bir hata oluştu" };
        }

        public Result<IEnumerable<Exam>> GetExams()
        {
            try
            {
                var list = context.Exams
                      .Include(x => x.AppUserCreator)
                    .Include(x => x.AppUserEditor)
                    .AsEnumerable();


                return new Result<IEnumerable<Exam>>
                {
                    Data = list,
                    Success = true,
                    Message = "Sınav listesini görüntülemektesiniz"
                };
            }
            catch (Exception ex)
            {
                new Result<IEnumerable<QuestionMain>>(ex);
            }

            return new Result<IEnumerable<Exam>>
            {
                Success = false,
                Message = "Bir hata oluştu"
            };
        }

        public Result<Exam> GetExamByKey(int id)
        {

            var model = context.Exams.FirstOrDefault(x => x.Id == id);
            if (model == null)
                return new Result<Exam> { Success = false, Message = "Sınav bulunamadı" };
            return new Result<Exam> { Success = true, Message = "İşlem başarılı", Data = model };
        }


        public Result SetExamStatus(int id, string userId)
        {
            try
            {
                var model = context.Exams.FirstOrDefault(x => x.Id == id);
                if (model == null)
                    return new Result { Success = false, Message = "Konu bulunamadı" };
                bool currentStatus = model.IsActive;
                model.IsActive = !model.IsActive;
                model.Editor = userId;
                context.Entry(model).State = EntityState.Modified;
                context.SaveChanges();
                return new Result { Success = true, Message = string.Format("Yeni {0} ile {1} edilmiştir", "sınav", currentStatus ? "pasif" : "aktif") };
            }
            catch (Exception ex)
            {
                new Result(ex);
            }
            return new Result { Success = false, Message = "Bir hata oluştu" };
        }

        public Result EditExam(Exam model, string userId)
        {
            try
            {
                var editmodel = context.Exams.FirstOrDefault(x => x.Id == model.Id);
                if (model == null)
                    return new Result { Success = false, Message = "Sınav bulunamadı" };
                bool currentStatus = model.IsActive;
                editmodel.Name = model.Name;
                editmodel.IsActive = model.IsActive;
                editmodel.Editor = userId;
                context.Entry(editmodel).State = EntityState.Modified;
                context.SaveChanges();
                return new Result { Success = true, Message = string.Format("Yeni {0} ile {1}", "sınav", "güncellenmiştir") };
            }
            catch (Exception ex)
            {
                new Result(ex);
            }
            return new Result { Success = false, Message = "Bir hata oluştu" };
        }

        public Result DeleteExam(int id)
        {
            try
            {
                var deletemodel = context.Exams.FirstOrDefault(x => x.Id == id);
                if (deletemodel == null)
                    return new Result { Success = false, Message = "Sınav bulunamadı" };


                context.Entry(deletemodel).State = EntityState.Deleted;
                context.SaveChanges();
                return new Result { Success = true, Message = string.Format("Sınav başarı ile silinmiştir") };
            }
            catch (Exception ex)
            {
                new Result(ex);
            }
            return new Result { Success = false, Message = "Bir hata oluştu" };
        }

        #endregion

        #region Class

        public Result AddClass(Class model, string userId)
        {
            try
            {
                model.Creator = userId;
                context.Classes.Add(model);
                context.SaveChanges();
                return new Result { Success = true, Message = "Yeni sınıf başarı ile eklenişmiştir" };
            }
            catch (Exception ex)
            {
                new Result(ex);
            }
            return new Result { Success = false, Message = "Bir hata oluştu" };
        }

        public Result<IEnumerable<Class>> GetClass()
        {
            try
            {
                var list = context.Classes.Include(x => x.AppUserCreator)
                    .Include(x => x.AppUserEditor).AsEnumerable();


                return new Result<IEnumerable<Class>>
                {
                    Data = list,
                    Success = true,
                    Message = "Sınıf listesini görüntülemektesiniz"
                };
            }
            catch (Exception ex)
            {
                new Result<IEnumerable<QuestionMain>>(ex);
            }

            return new Result<IEnumerable<Class>>
            {
                Success = false,
                Message = "Bir hata oluştu"
            };
        }

        public Result<Class> GetClassByKey(int id)
        {

            var model = context.Classes.FirstOrDefault(x => x.Id == id);
            if (model == null)
                return new Result<Class> { Success = false, Message = "Sınıf bulunamadı" };
            return new Result<Class> { Success = true, Message = "İşlem başarılı", Data = model };
        }


        public Result SetClassStatus(int id, string userId)
        {
            try
            {
                var model = context.Classes.FirstOrDefault(x => x.Id == id);
                if (model == null)
                    return new Result { Success = false, Message = "Sınıf bulunamadı" };
                bool currentStatus = model.IsActive;
                model.IsActive = !model.IsActive;
                model.Editor = userId;
                context.Entry(model).State = EntityState.Modified;
                context.SaveChanges();
                return new Result { Success = true, Message = string.Format("Yeni {0} ile {1} edilmiştir", "sınıf", currentStatus ? "pasif" : "aktif") };
            }
            catch (Exception ex)
            {
                new Result(ex);
            }
            return new Result { Success = false, Message = "Bir hata oluştu" };
        }

        public Result EditClass(Class model, string userId)
        {
            try
            {
                var editmodel = context.Classes.FirstOrDefault(x => x.Id == model.Id);
                if (model == null)
                    return new Result { Success = false, Message = "Sınıf bulunamadı" };
                bool currentStatus = model.IsActive;
                editmodel.Name = model.Name;
                editmodel.IsActive = model.IsActive;
                editmodel.Editor = userId;
                context.Entry(editmodel).State = EntityState.Modified;
                context.SaveChanges();
                return new Result { Success = true, Message = string.Format("Yeni {0} ile {1}", "sınıf", "güncellenmiştir") };
            }
            catch (Exception ex)
            {
                new Result(ex);
            }
            return new Result { Success = false, Message = "Bir hata oluştu" };
        }

        public Result DeleteClass(int id)
        {
            try
            {
                var deletemodel = context.Classes.FirstOrDefault(x => x.Id == id);
                if (deletemodel == null)
                    return new Result { Success = false, Message = "Sınıf bulunamadı" };


                context.Entry(deletemodel).State = EntityState.Deleted;
                context.SaveChanges();
                return new Result { Success = true, Message = string.Format("Sınıf başarı ile silinmiştir") };
            }
            catch (Exception ex)
            {
                new Result(ex);
            }
            return new Result { Success = false, Message = "Bir hata oluştu" };
        }

        #endregion


        #region SubSubject

        public Result AddSubSubject(SubSubject model, string userId)
        {
            try
            {
                model.Creator = userId;
                context.SubSubjects.Add(model);
                context.SaveChanges();
                return new Result { Success = true, Message = "Yeni alt konu başarı ile eklenişmiştir" };
            }
            catch (Exception ex)
            {
                new Result(ex);
            }
            return new Result { Success = false, Message = "Bir hata oluştu" };
        }
        public Result<IEnumerable<SubSubject>> GetSubSubjects(int subjectId)
        {
            try
            {
                var list = context.SubSubjects.Where(x => x.SubjectId == subjectId).AsEnumerable();

                return new Result<IEnumerable<SubSubject>>
                {
                    Data = list,
                    Success = true,
                    Message = "Alt konu listesini görüntülemektesiniz"
                };
            }
            catch (Exception ex)
            {
                new Result<IEnumerable<QuestionMain>>(ex);
            }

            return new Result<IEnumerable<SubSubject>>
            {
                Success = false,
                Message = "Bir hata oluştu"
            };
        }
        public Result<IEnumerable<SubSubject>> GetSubSubjectsAll()
        {
            try
            {
                var list = context.SubSubjects.AsEnumerable();

                return new Result<IEnumerable<SubSubject>>
                {
                    Data = list,
                    Success = true,
                    Message = "Alt konu listesini görüntülemektesiniz"
                };
            }
            catch (Exception ex)
            {
                new Result<IEnumerable<QuestionMain>>(ex);
            }

            return new Result<IEnumerable<SubSubject>>
            {
                Success = false,
                Message = "Bir hata oluştu"
            };
        }
        public Result<SubSubject> GetSubSubjectByKey(int id)
        {

            var model = context.SubSubjects.FirstOrDefault(x => x.Id == id);
            if (model == null)
                return new Result<SubSubject> { Success = false, Message = "Alt Konu bulunamadı" };
            return new Result<SubSubject> { Success = true, Message = "İşlem başarılı", Data = model };
        }


        public Result SetSubSubjectStatus(int id, string userId)
        {
            try
            {
                var model = context.SubSubjects.FirstOrDefault(x => x.Id == id);
                if (model == null)
                    return new Result { Success = false, Message = "Alt konu bulunamadı" };
                bool currentStatus = model.IsActive;
                model.IsActive = !model.IsActive;
                model.Editor = userId;
                context.Entry(model).State = EntityState.Modified;
                context.SaveChanges();
                return new Result { Success = true, Message = string.Format("Yeni {0} ile {1} edilmiştir", "alt konu", currentStatus ? "pasif" : "aktif") };
            }
            catch (Exception ex)
            {
                new Result(ex);
            }
            return new Result { Success = false, Message = "Bir hata oluştu" };
        }

        public Result EditSubSubject(SubSubject model, string userId)
        {
            try
            {
                var editmodel = context.SubSubjects.FirstOrDefault(x => x.Id == model.Id);
                if (model == null)
                    return new Result { Success = false, Message = "Alt konu bulunamadı" };
                bool currentStatus = model.IsActive;
                editmodel.Name = model.Name;
                editmodel.IsActive = model.IsActive;
                editmodel.Editor = userId;
                editmodel.SubjectId = model.SubjectId;
                context.Entry(editmodel).State = EntityState.Modified;
                context.SaveChanges();
                return new Result { Success = true, Message = string.Format("Yeni {0} ile {1}", "konu", "güncellenmiştir") };
            }
            catch (Exception ex)
            {
                new Result(ex);
            }
            return new Result { Success = false, Message = "Bir hata oluştu" };
        }

        public Result DeleteSubSubject(int id)
        {
            try
            {
                var deletemodel = context.SubSubjects.FirstOrDefault(x => x.Id == id);
                if (deletemodel == null)
                    return new Result { Success = false, Message = "Konu bulunamadı" };


                context.Entry(deletemodel).State = EntityState.Deleted;
                context.SaveChanges();
                return new Result { Success = true, Message = string.Format("Alt konu başarı ile silinmiştir") };
            }
            catch (Exception ex)
            {
                new Result(ex);
            }
            return new Result { Success = false, Message = "Bir hata oluştu" };
        }

        #endregion


        #region QuizTemplate

        public Result<IEnumerable<ChallengeTemplate>> GetChallengeTemplates(ChallengeFilterModel model)
        {
            try
            {
                var list = context.ChallengeTemplates
                     .Include(x => x.AppUserCreator)
                     .Include(x => x.AppUserEditor).AsQueryable();
                if (!string.IsNullOrEmpty(model.Name))
                    list = list.Where(x => x.Name.ToLower().Contains(model.Name));
                if (model.StartDate.ToDate().HasValue)
                    list = list.Where(x => x.CreatedDate >= model.StartDate.ToDate().Value);
                if (model.EndDate.ToDate().HasValue)
                    list = list.Where(x => x.CreatedDate < model.EndDate.ToDate().Value);

                return new Result<IEnumerable<ChallengeTemplate>>
                {
                    Data = list.OrderByDescending(x => x.Id).Skip(model.CurrentPage.HasValue ? ((model.CurrentPage.Value - 1) * PageSize) : 0).Take(PageSize).AsEnumerable(),
                    Success = true,
                    Message = "Soru listesini görüntülemektesiniz",
                    Paginition = new Paginition(list.Count(), PageSize, model.CurrentPage.HasValue ? model.CurrentPage.Value : 1, model.Name,
                    model.StartDate,
                   model.EndDate)
                };
            }
            catch (Exception ex)
            {
                return new Result<IEnumerable<ChallengeTemplate>>(ex);
            }
        }

        public Result AddChallengeTemplate(ChallengeTemplate model, string userId)
        {
            try
            {
                model.Creator = userId;
                model.CreatedDate = DateTime.Now;
                context.ChallengeTemplates.Add(model);
                context.SaveChanges();
                return new Result { Success = true, Message = "Yeni template başarı ile eklenişmiştir" };
            }
            catch (Exception ex)
            {
                return new Result(ex);
            }
        }

        public Result EditChallengeTemplate(ChallengeTemplate model, string userId)
        {
            try
            {
                var editmodel = context.ChallengeTemplates.FirstOrDefault(x => x.Id == model.Id);
                if (model == null)
                    return new Result { Success = false, Message = "Branş bulunamadı" };
                bool currentStatus = model.IsActive;
                editmodel.Name = model.Name;
                editmodel.Description = model.Description;
                editmodel.IsActive = model.IsActive;
                editmodel.Editor = userId;
                editmodel.StartDate = model.StartDate;
                editmodel.EndDate = model.EndDate;
                context.Entry(editmodel).State = EntityState.Modified;
                context.SaveChanges();
                return new Result { Success = true, Message = string.Format(" {0} başarı ile {1}", "Template", "güncellenmiştir") };
            }
            catch (Exception ex)
            {
                return new Result(ex);
            }
        }

        public Result<Entity.Challenge.ChallengeTemplate> GetChallengeTemplateByKey(int id)
        {

            var model = context.ChallengeTemplates.FirstOrDefault(x => x.Id == id);
            if (model == null)
                return new Result<ChallengeTemplate> { Success = false, Message = "Template bulunamadı" };
            return new Result<ChallengeTemplate> { Success = true, Message = "İşlem başarılı", Data = model };
        }


        public Result<IEnumerable<ChallengeTemplateItems>> GetChallengeTemplateItems(int id)
        {
            try
            {
                var list = context.ChallengeTemplateItems
                      .Include(x => x.AppUserCreator)
                      .Include(x => x.AppUserEditor)
                      .Include(x => x.Subject)
                      .Include(x => x.SubSubject)
                      .Include(x => x.Exam)
                      .Include(x => x.Lecture)
                    .Where(x => x.ChallengeTemplateId == id)
                    .ToList();

                return new Result<IEnumerable<ChallengeTemplateItems>>
                {
                    Data = list,
                    Success = true,
                    Message = "Challenge template listesini görüntülemektesiniz"
                };
            }
            catch (Exception ex)
            {
               return new Result<IEnumerable<ChallengeTemplateItems>>(ex);
            }
        }
        public Result<ChallengeTemplateItems> GetChallengeTemplateItemByKey(int id)
        {

            try
            {
                var list = context.ChallengeTemplateItems
                      .Include(x => x.AppUserCreator)
                    .Include(x => x.AppUserEditor)
                    .FirstOrDefault(x => x.Id == id);

                return new Result<ChallengeTemplateItems>
                {
                    Data = list,
                    Success = true,
                    Message = "Challenge listesini görüntülemektesiniz"
                };
            }
            catch (Exception ex)
            {
                return new Result<ChallengeTemplateItems>(ex);
            }

        }
        public Result AddChallengeTemplateItem(ChallengeTemplateItems model, string userId)
        {
            try
            {
                model.Creator = userId;
                model.CreatedDate = DateTime.Now;
                context.ChallengeTemplateItems.Add(model);
                context.SaveChanges();
                return new Result { Success = true, Message = "Yeni kural başarı ile eklenişmiştir" };
            }
            catch (Exception ex)
            {
                return new Result(ex);
            }
        }

        public Result EditChallengeTemplateItem(ChallengeTemplateItems model, string userId)
        {
            try
            {
                var editmodel = context.ChallengeTemplateItems.FirstOrDefault(x => x.Id == model.Id);
                if (model == null)
                    return new Result { Success = false, Message = "Branş bulunamadı" };
                bool currentStatus = model.IsActive;
                editmodel.IsActive = model.IsActive;
                editmodel.Editor = userId;
                editmodel.LectureId = model.LectureId;
                editmodel.QuestionPdfId = model.QuestionPdfId;
                editmodel.SubjectId = model.SubjectId;
                editmodel.SubSubjectId = model.SubSubjectId;
                editmodel.Count = model.Count;
                editmodel.Difficulty = model.Difficulty;
                editmodel.ExamId = model.ExamId;
                editmodel.ModifiedDate = DateTime.Now;
                context.Entry(editmodel).State = EntityState.Modified;
                context.SaveChanges();
                return new Result { Success = true, Message = string.Format("{0} başarı ile {1}", "Template kuralı", "güncellenmiştir") };
            }
            catch (Exception ex)
            {
                return new Result(ex);
            }
        }
        //public Result SetQuizTemplateItem(int id, string userId)
        //{
        //    try
        //    {
        //        var model = context.Lectures.FirstOrDefault(x => x.Id == id);
        //        if (model == null)
        //            return new Result { Success = false, Message = "Branş bulunamadı" };
        //        bool currentStatus = model.IsActive;
        //        model.IsActive = !model.IsActive;
        //        model.Editor = userId;
        //        context.Entry(model).State = EntityState.Modified;
        //        context.SaveChanges();
        //        return new Result { Success = true, Message = string.Format("Yeni {0} ile {1} edilmiştir", "branş", currentStatus ? "pasif" : "aktif") };
        //    }
        //    catch (Exception ex)
        //    {
        //        new Result(ex);
        //    }
        //    return new Result { Success = false, Message = "Bir hata oluştu" };
        //}


        //public Result DeleteQuizTemplateItem(int id)
        //{
        //    try
        //    {
        //        var deletemodel = context.Lectures.FirstOrDefault(x => x.Id == id);
        //        if (deletemodel == null)
        //            return new Result { Success = false, Message = "Branş bulunamadı" };


        //        context.Entry(deletemodel).State = EntityState.Deleted;
        //        context.SaveChanges();
        //        return new Result { Success = true, Message = string.Format("Branş başarı ile silinmiştir") };
        //    }
        //    catch (Exception ex)
        //    {
        //        new Result(ex);
        //    }
        //    return new Result { Success = false, Message = "Bir hata oluştu" };
        //}

        #endregion

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