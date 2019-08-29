using AqApplication.Core.Type;
using AqApplication.Entity.Identity.Data;
using AqApplication.Repository.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AqApplication.Entity.Challenge;
using AqApplication.Entity.Constants;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AnswerQuestionApp.Repository.Configuration;
using AnswerQuestionApp.Repository.FilterModels;
using AnswerQuestionApp.Entity.Configuration;
using AnswerQuestionApp.Repository;
using Microsoft.Extensions.Caching.Memory;
using AnswerQuestionApp.Repository.Mail;

namespace AqApplication.Repository.Challenge
{
    public class ChallengeRepo : BaseRepo, IChallenge
    {
        private ApplicationDbContext context;
        private bool disposedValue = false;
        private readonly int PageSize = 20;
        private readonly IEmailSender iEmailSender;

        public ChallengeRepo(ApplicationDbContext _context, IMemoryCache memoryCache, IEmailSender _iEmailSender) : base(_context, memoryCache)
        {
            context = _context;
            iEmailSender = _iEmailSender;
        }

        public Result<List<ChallengeQuestionViewModel>> RandomQuestion(string userId)
        {
            try
            {
                var question = context.QuestionMain.FirstOrDefault(x => x.Id == 78);

                if (question == null)
                    return new Result<List<ChallengeQuestionViewModel>>
                    {
                        Success = false,
                        Message = "Bir hata oluştu"
                    };

                var model = new ChallengeQuestionViewModel
                {
                    ImageExits = true,
                    MainText = question.MainTitle,
                    Image = question.MainImage,
                    QuestionId = question.Id,
                    AnswerCount = question.AnswerCount

                };

                return new Result<List<ChallengeQuestionViewModel>>
                {
                    Data = new List<ChallengeQuestionViewModel> { model },
                    Success = true,
                    Message = "İşlem başarı ile tamamlandı"
                };
            }

            catch (Exception ex)
            {
                return new Result<List<ChallengeQuestionViewModel>>(ex);
            }

        }
        public Result<List<ChallengeQuestionViewModel>> RandomChallenge(string userId)
        {
            try
            {
                var cValue = ConfigurationValues.FirstOrDefault(x => x.Key == ConfigKey.ChallengeServiceIsOpen);
                if (cValue == null)
                {
                    return new Result<List<ChallengeQuestionViewModel>>
                    {
                        Success = false,
                        Message = "Bir hata oluştu"
                    };
                }
                if (cValue.Values != "1")
                {
                    return new Result<List<ChallengeQuestionViewModel>>
                    {
                        Success = false,
                        Message = "Sunucu şuan bakımdadır. Lütfen daha sonra tekrar deneyiniz"
                    };
                }

                var addChallenge = this.AddChallenge( userId);

                if (!addChallenge.Success)
                    return new Result<List<ChallengeQuestionViewModel>>
                    {
                        Success = false,
                        Message = addChallenge.Message,
                    };
                if (!addChallenge.Data.ChallengeQuestions.Any())
                    return new Result<List<ChallengeQuestionViewModel>>
                    {
                        Success = false,
                        Message = "Bir hata oluştu"
                    };

                var quizDuration = new ConfigurationValuesRepo(context).GetByKey(AnswerQuestionApp.Entity.Configuration.ConfigKey.ChallengeTimeSecond);

                var questionList = addChallenge.Data.ChallengeQuestions.Select(x => new ChallengeQuestionViewModel
                {
                    ImageExits = true,
                    MainText = x.QuestionMain.MainTitle,
                    Image = x.QuestionMain.MainImage,
                    QuestionId = x.QuestionMain.Id,
                    AnswerCount = x.QuestionMain.AnswerCount,
                    QuizDuration = Convert.ToInt32(quizDuration.Data.Values)

                }).ToList();

                return new Result<List<ChallengeQuestionViewModel>>
                {
                    Data = questionList,
                    Success = true,
                    Message = "İşlem başarı ile tamamlandı"
                };
            }

            catch (Exception ex)
            {
                iEmailSender.SendEmail("Error " + DateTime.Now.ToLongDateString(), "erdemucar87@gmail.com", ex.Message);
                return new Result<List<ChallengeQuestionViewModel>>(ex);
            }

        }

        public Result<List<ChallengeQuestionViewModel>> ChallengeQuestions(int challengeId)
        {
            try
            {
                var challengeQuestions =
                    context.ChallengeQuizs.Include(x => x.QuestionMain)
                    .Where(x => x.ChallengeId == challengeId).AsEnumerable();

                var questionList = challengeQuestions.Select(x => new ChallengeQuestionViewModel
                {
                    ImageExits = true,
                    MainText = x.QuestionMain.MainTitle,
                    Image = x.QuestionMain.MainImage,
                    QuestionId = x.QuestionMain.Id,
                    AnswerCount = x.QuestionMain.AnswerCount,
                    ChallengeId = x.ChallengeId,
                    CorrectAnswer = x.QuestionMain.CorrectAnswer

                }).ToList();

                return new Result<List<ChallengeQuestionViewModel>>
                {
                    Data = questionList,
                    Success = questionList.Any(),
                    Message = questionList.Any() ? "İşlem başarı ile tamamlandı" : "Soru datası bulunamadı"
                };
            }

            catch (Exception ex)
            {
                return new Result<List<ChallengeQuestionViewModel>>(ex);
            }

        }

        public Result<AqApplication.Entity.Challenge.Challenge> AddChallenge(string userId)
        {
            try
            {
                var list = GetQuestionsByRandomTemplate().Data.Select(x => new ChallengeQuestions
                {
                    QuestionId = x.Id,
                    Seo = x.Id,
                    QuestionMain = x,
                }).ToList();



                var model = new AqApplication.Entity.Challenge.Challenge
                {
                    IsActive = true,
                    Creator = userId,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddSeconds(challengeExpDay),
                    CreatedDate = DateTime.Now,
                    ChallengeTypeId = (int)ChallengeTypeEnum.RandomMode,
                    ChallengeQuestions = list

                };

                context.Challenge.Add(model);
                context.SaveChanges();
                return new Result<AqApplication.Entity.Challenge.Challenge>
                {
                    Data = model,
                    Success = true
                };
            }

            catch (Exception ex)
            {
                return new Result<AqApplication.Entity.Challenge.Challenge>(ex);
            }



        }

        public Result AddChallengeSession(string userId, int challengeId)
        {
            try
            {
                var model = new ChallengeSession
                {
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    Creator = userId,
                    UserId = userId,
                    StartDate = DateTime.Now,
                    IsCompleted = false
                };

                context.ChallengeSessions.Add(model);
                context.SaveChanges();


                return new Result
                {
                    Success = true,
                    Message = "İşme başarılı"
                };
            }

            catch (Exception ex)
            {
                return new Result(ex);
            }

        }
        public Result UpdateChallengeSessionCompleted(string userId, int challengeId, string totalMark, int correctCount)
        {
            try
            {

                var model = context.ChallengeSessions.FirstOrDefault(x => x.ChallengeId == challengeId && x.UserId == userId);
                if (model != null)
                {
                    model.ModifiedDate = DateTime.Now;
                    model.EndDate = DateTime.Now;
                    model.IsCompleted = true;
                    model.Editor = userId;
                    model.CorrectCount = correctCount;
                    model.TotalScore = totalMark;
                }

                context.Entry(model).State = EntityState.Modified;
                context.SaveChanges();

                return new Result
                {
                    Success = true,
                    Message = "İşme başarılı"
                };
            }

            catch (Exception ex)
            {
                return new Result(ex);
            }

        }

        private Result AddChallengeQuestions(int challengeId)
        {
            try
            {
                var questions = context.QuestionMain.OrderByDescending(X => X.Id).Take(challengeQuestionLimit).AsEnumerable();
                var list = questions.Select(x => new ChallengeQuestions
                {
                    QuestionId = x.Id,
                    Seo = x.Id,
                    ChallengeId = challengeId

                }).AsEnumerable();

                context.ChallengeQuizs.AddRange(list);
                context.SaveChanges();

            }
            catch (Exception ex)
            {
                return new Result(ex);

            }
            return new Result
            {
                Success = true,
                Message = "İşlem başarı ile tamamlandı"
            };
        }
        public Result SetChallengeAnswer(ChallengeQuestionAnswerViewModel model)
        {
            try
            {
                var currentAnswer = context.ChallengeQuestionAnswers.FirstOrDefault(x => x.QuestionId == model.QuestionId && x.ChallengeId == model.ChallengeId && x.UserId == model.UserId);

                if (currentAnswer != null)
                {
                    currentAnswer.AnswerIndex = model.AnswerIndex;
                    context.Entry(currentAnswer).State = EntityState.Modified;
                }
                else
                {
                    context.ChallengeQuestionAnswers.Add(new ChallengeQuestionAnswers
                    {
                        CreatedDate = DateTime.Now,
                        ChallengeId = model.ChallengeId,
                        QuestionId = model.QuestionId,
                        UserId = model.UserId,
                        AnswerIndex = model.AnswerIndex,

                    });
                }

                context.SaveChanges();
                return new Result
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new Result(ex);
            }
        }

        public Result<ChallengeChallengeUserViewModel> GetResultChallenge(int challengeId, string userId)
        {
            try
            {
                List<ChallengeQuestions> challangeQuestion = context.ChallengeQuizs.Include(x => x.QuestionMain).Where(x => x.ChallengeId == challengeId).ToList();

                List<ChallengeQuestionAnswers> challengeAnswer = context.ChallengeQuestionAnswers
                    .Include(x => x.ApplicationUser)
                    .Where(x => x.ChallengeId
                == challengeId).ToList();

                //List<string> challengeUser = challengeAnswer.Select(x => x.UserId).Distinct().ToList();

                var resultList = new List<ChallengeChallengeUserViewModel>();

                int totalQuestion = challangeQuestion.Count();

                //foreach (var user in challengeUser)
                //{
                var answers = challengeAnswer.Where(x => x.UserId == userId && x.ChallengeId
              == challengeId).ToList();
                int correct = 0, wrong = 0;
                foreach (var item in answers)
                {
                    var question = challangeQuestion.FirstOrDefault(x => x.QuestionId == item.QuestionId);
                    if (question == null)
                        throw new Exception();
                    if (item.AnswerIndex == question.QuestionMain.CorrectAnswer)
                        correct++;
                    else
                        wrong++;
                }
                resultList.Add(new ChallengeChallengeUserViewModel
                {
                    correct = correct,
                    UserName = challengeAnswer.FirstOrDefault(x => x.UserId == userId).ApplicationUser.UserName
                });

                //}
                var resultordered = new List<ChallengeChallengeUserViewModel>();
                foreach (var item in resultList)
                {

                    resultordered.Add(new ChallengeChallengeUserViewModel
                    {

                        Mark = decimal.Round(((item.correct * 100) / totalQuestion), 2, MidpointRounding.AwayFromZero).ToString(),
                        UserName = item.UserName
                    });
                }

                var resultseqList = new ChallengeChallengeUserViewModel();

                resultordered = resultordered.OrderByDescending(x => x.Mark).ToList();

                int i = 1;

                foreach (var item in resultordered)
                {

                    resultseqList = new ChallengeChallengeUserViewModel
                    {

                        Mark = item.Mark,
                        Seq = i,
                        UserName = item.UserName
                    };
                    i++;
                }

                return new Result<ChallengeChallengeUserViewModel>
                {
                    Data = resultseqList,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new Result<ChallengeChallengeUserViewModel>(ex);
            }
        }

        public Result<Entity.Challenge.Challenge> GetLastChallengeByType(ChallengeTypeEnum type)
        {
            try
            {
                var challenge = context.Challenge.Where(x => x.ChallengeTypeId == (int)type).OrderByDescending(x => x.Id).FirstOrDefault();
                if (challenge != null)
                {
                    return new Result<Entity.Challenge.Challenge>
                    {
                        Data = challenge,
                        Success = true,
                    };
                }
                return new Result<Entity.Challenge.Challenge>
                {
                    Message = "Kayıt bulunamadı",
                    Success = false,
                };
            }
            catch (Exception ex)
            {
                return new Result<Entity.Challenge.Challenge>(ex);

            }
        }

        public void SetCompletedChallenge()
        {
            try
            {
                //await
            }

            catch (Exception ex)
            {
                throw new Exception();
            }
        }


        public Result<IEnumerable<Entity.Challenge.Challenge>> GetChallengesPaginated(ChallengeFilterModel model)
        {
            try
            {
                var list = context.Challenge.Include(x => x.ChallengeSessions).Include(x => x.ChallengeQuestions)
                   .AsQueryable();
                if (model.StartDate.ToDate().HasValue)
                    list = list.Where(x => x.CreatedDate >= model.StartDate.ToDate().Value);
                if (model.EndDate.ToDate().HasValue)
                    list = list.Where(x => x.CreatedDate < model.EndDate.ToDate().Value);

                return new Result<IEnumerable<Entity.Challenge.Challenge>>
                {
                    Data = list.OrderByDescending(x => x.Id).Skip(model.CurrentPage.HasValue ? ((model.CurrentPage.Value - 1) * PageSize) : 0).Take(PageSize).AsEnumerable(),
                    Success = true,
                    Message = "Challenge listesini görüntülemektesiniz",
                    Paginition = new Paginition(list.Count(), PageSize, model.CurrentPage.HasValue ? model.CurrentPage.Value : 1, model.Name,
                    model.StartDate,
                model.EndDate)
                };
            }
            catch (Exception ex)
            {
                new Result<IEnumerable<Entity.Challenge.Challenge>>(ex);
            }
            return new Result<IEnumerable<Entity.Challenge.Challenge>>
            {
                Success = false,
                Message = "Bir hata oluştu"
            };
        }


        #region ChallengeQuizGeneration
        private Result<List<Entity.Question.QuestionMain>> GetQuestionsByRandomTemplate()
        {
            try
            {
                Random rnd = new Random();

                int rndTemplateIndex = rnd.Next(0, context.ChallengeTemplates.Where(x => x.IsActive).Count());

                var templates = context.ChallengeTemplates.Include(x => x.ChallengeTemplateItems)
                    .Where(x => x.IsActive && x.ChallengeTemplateItems.Where(y=>y.IsActive).Any()).Skip(rndTemplateIndex).Take(1).FirstOrDefault();

                #region NullControl
                if (templates == null)
                    return new Result<List<Entity.Question.QuestionMain>>
                    {
                        Success = false,
                        Message = "Kayıt bulunamadı"
                    };

                if (templates.ChallengeTemplateItems == null)
                    return new Result<List<Entity.Question.QuestionMain>>
                    {
                        Success = false,
                        Message = "Kayıt bulunamadı"
                    };
                #endregion

                var result = new List<Entity.Question.QuestionMain>();
                bool completed = false;
                int questionCount = base.challengeQuestionCount;

                foreach (var item in templates.ChallengeTemplateItems)
                {

                    var question = context.QuestionMain.Include(x => x.QuestionExams).AsQueryable();

                    if (item.Difficulty.HasValue)
                        question.Where(x => x.Difficulty == item.Difficulty.Value);

                    if (item.SubjectId.HasValue)
                        question.Where(x => x.SubjectId == item.SubjectId.Value);

                    if (item.SubSubjectId.HasValue)
                        question.Where(x => x.SubSubjectId == item.SubSubjectId.Value);

                    if (item.LectureId.HasValue)
                        question.Where(x => x.LectureId == item.LectureId.Value);

                    if (item.ExamId.HasValue)
                        question.Where(x => x.QuestionExams.Where(y => y.Id == item.ExamId).Any());

                    int questionTemplateListCount = question.Count();
                    var questionTemplateList = question.ToList();

                    if (questionTemplateListCount >= item.Count)
                    {
                        for (int i = 0; i < item.Count; i++) // yterli limitte soru varmı
                        {
                            int rndQ = 0;
                            while (true)
                            {
                                rndQ = rnd.Next(0, questionTemplateListCount);
                                if (result.FirstOrDefault(x => x.Id == questionTemplateList[rndQ].Id) == null)
                                    result.Add(questionTemplateList[rndQ]);
                                else
                                    break;
                            }

                            if (result.Count == questionCount)
                            {
                                completed = true;
                                break;
                            }
                        }

                    }
                    else
                    {
                        for (int i = 0; i < questionTemplateListCount; i++) 
                        {
                            result.Add(questionTemplateList[i]);
                            if (result.Count == questionCount)
                            {
                                completed = true;
                                break;
                            }
                        }
                    }

                    if (completed)
                        break;
                }

                return new Result<List<Entity.Question.QuestionMain>>
                {
                    Data= result,
                    Success = true,
                    Message = "İşlem başarı ile tamamlandı"
                };

            }
            catch (Exception ex)
            {
                return new Result<List<Entity.Question.QuestionMain>>(ex);
            }

        }

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