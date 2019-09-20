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
        public Result<List<ChallengeQuestionViewModel>> CreateChallenge(string userId, int? lectureId, ChallengeTypeEnum challengeType)
        {
            try
            {
                var addChallenge = this.AddChallenge(userId, challengeType, challengeType == ChallengeTypeEnum.RandomMode ? challengeQuestionCount : practiceModeQuestionCount(), lectureId);

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
                        Message = "Üzgünüz! Bu kategori şuan yapım aşamasında"
                    };


                var questionList = addChallenge.Data.ChallengeQuestions.Select(x => new ChallengeQuestionViewModel
                {
                    ImageExits = true,
                    MainText = x.QuestionMain.MainTitle,
                    Image = x.QuestionMain.MainImage,
                    QuestionId = x.QuestionMain.Id,
                    AnswerCount = x.QuestionMain.AnswerCount,
                    QuizDuration = challengeExpDay,
                    CorrectAnswer = x.QuestionMain.CorrectAnswer,
                    ChallengeAnswerViewModel = x.QuestionMain.QuestionAnswers.Select(y => new ChallengeAnswerViewModel
                    {
                        Index = (y.Seo ?? 0) - 1,
                        Title = y.Title
                    }).ToList()
                }).ToList();

                return new Result<List<ChallengeQuestionViewModel>>
                {
                    Data = questionList,
                    InstertedId = addChallenge.Data.Id,
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
                    context.ChallengeQuizs.Include(x => x.QuestionMain.QuestionAnswers)
                    .Where(x => x.ChallengeId == challengeId).AsEnumerable();

                var questionList = challengeQuestions.Select(x => new ChallengeQuestionViewModel
                {
                    ImageExits = true,
                    MainText = x.QuestionMain.MainTitle,
                    Image = x.QuestionMain.MainImage,
                    QuestionId = x.QuestionMain.Id,
                    AnswerCount = x.QuestionMain.AnswerCount,
                    ChallengeId = x.ChallengeId,
                    CorrectAnswer = x.QuestionMain.CorrectAnswer,
                    Seo = x.Seo,
                    ChallengeAnswerViewModel = x.QuestionMain.QuestionAnswers.Select(y => new ChallengeAnswerViewModel
                    {
                        Index = (y.Seo ?? 0) - 1,
                        Title = y.Title
                    }).ToList()

                }).OrderBy(x => x.Seo).ToList();

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

        public Result<AqApplication.Entity.Challenge.Challenge> AddChallenge(string userId, ChallengeTypeEnum challengeType, int questionCount, int? lectureId)
        {
            try
            {
                var list = GetQuestionsByRandomTemplate(challengeType, questionCount, lectureId).Data.Select(x => new ChallengeQuestions
                {
                    QuestionId = x.Id,
                    Seo = x.Seo ?? 0,
                    QuestionMain = x,
                }).ToList();



                var model = new AqApplication.Entity.Challenge.Challenge
                {
                    IsActive = true,
                    Creator = userId,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddSeconds(challengeExpDay),
                    CreatedDate = DateTime.Now,
                    ChallengeTypeId = (int)challengeType,
                    ChallengeQuestions = list,
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

        public Result AddChallengeSession(string userId, int challengeId, DateTime startDate)
        {
            try
            {
                var model = new ChallengeSession
                {
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    Creator = userId,
                    UserId = userId,
                    StartDate = startDate,
                    IsCompleted = false,
                    ChallengeId = challengeId
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
                throw ex;
            }

        }
        public Result UpdateChallengeSessionCompleted(string userId, int challengeId, string totalMark, int correctCount, DateTime date)
        {
            try
            {

                var model = context.ChallengeSessions.FirstOrDefault(x => x.ChallengeId == challengeId && x.UserId == userId);
                if (model != null)
                {
                    model.ModifiedDate = date;
                    model.EndDate = date;
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

        public Result<QuizResultViewModel> GetResultChallenge(int challengeId, string userId, ChallengeTypeEnum challengeType = ChallengeTypeEnum.RandomMode)
        {
            try
            {
                DateTime now = DateTime.Now;

                List<ChallengeQuestions> challangeQuestion = context.ChallengeQuizs.Include(x => x.QuestionMain)
                 .Where(x => x.ChallengeId == challengeId).ToList();

                List<QuestionAnswerViewModel> questionAnswerViewModelList = new List<QuestionAnswerViewModel>();

                List<ChallengeQuestionAnswers> challengeAnswer = context.ChallengeQuestionAnswers
                    .Include(x => x.ApplicationUser)
                    .Where(x => x.ChallengeId
                == challengeId).ToList();

                var challengeSession = context.ChallengeSessions.FirstOrDefault(x => x.UserId == userId && x.ChallengeId == challengeId);

                //List<string> challengeUser = challengeAnswer.Select(x => x.UserId).Distinct().ToList();

                int duration = 0;

                if (challengeType == ChallengeTypeEnum.RandomMode)
                    duration = challengeDuration;
                else if (challengeType == ChallengeTypeEnum.PracticeMode)
                    duration = practiceModeExamDuration();

                var result = new ChallengeUserViewModel();

                int totalQuestion = challangeQuestion.Count();

                int correct = 0, wrong = 0;

                #region DurationCalculation

                decimal calculatedDuration = 0;

                TimeSpan endDuration = ((TimeSpan)(now - challengeSession.StartDate));

                int totalSecondEndDuration = (int)endDuration.TotalSeconds;

                calculatedDuration = decimal.Round(((totalSecondEndDuration * 100) / duration), 1, MidpointRounding.AwayFromZero);

                string durationString = endDuration.ToString(@"hh\:mm\:ss");

                int orginalDuration = 0;

                if (challengeType == ChallengeTypeEnum.RandomMode)
                    orginalDuration = base.challengeDuration;
                else if (challengeType == ChallengeTypeEnum.PracticeMode)
                    orginalDuration = practiceModeExamDuration();

                #endregion


                foreach (var challengeQuestionItem in challangeQuestion)
                {
                    var answer = challengeAnswer.Where(x => x.UserId == userId && x.ChallengeId
         == challengeId && x.QuestionId == challengeQuestionItem.QuestionId).FirstOrDefault();

                    var question = challangeQuestion.FirstOrDefault(x => x.QuestionId == challengeQuestionItem.QuestionId);
                    if (question == null)
                        throw new Exception();

                    if (answer != null)
                    {
                        if (answer.AnswerIndex == question.QuestionMain.CorrectAnswer)
                            correct++;
                        else
                            wrong++;

                        questionAnswerViewModelList.Add(new QuestionAnswerViewModel
                        {
                            AnswerCount = question.QuestionMain.AnswerCount,
                            CorrectAnswer = question.QuestionMain.CorrectAnswer,
                            UserAnswer = answer.AnswerIndex,
                            Seo = challengeQuestionItem.Seo,
                            QuestionId = question.QuestionMain.Id
                        });
                    }
                    else
                    {
                        questionAnswerViewModelList.Add(new QuestionAnswerViewModel
                        {
                            AnswerCount = question.QuestionMain.AnswerCount,
                            CorrectAnswer = question.QuestionMain.CorrectAnswer,
                            UserAnswer = -1,
                            Seo = challengeQuestionItem.Seo,
                            QuestionId = question.QuestionMain.Id
                        });
                    }

                }

                result = new ChallengeUserViewModel
                {
                    correct = correct,
                    UserName = challengeAnswer.FirstOrDefault(x => x.UserId == userId).ApplicationUser.UserName,
                    Mark = decimal.Round(((correct * 100) / totalQuestion), 2, MidpointRounding.AwayFromZero).ToString(),
                    DurationPercentage = calculatedDuration.ToString(),
                    Duration = durationString,
                };


                UpdateChallengeSessionCompleted(userId, challengeId, result.Mark, result.correct, now);

                return new Result<QuizResultViewModel>
                {
                    Data = new QuizResultViewModel { ChallengeUserViewModel = result, QuestionAnswerViewModel = questionAnswerViewModelList },
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new Result<QuizResultViewModel>(ex);
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
                    model.EndDate,null)
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
        private Result<List<Entity.Question.QuestionMain>> GetQuestionsByRandomTemplate(ChallengeTypeEnum type, int questionCount, int? lectureId)
        {
            try
            {
                Random rnd = new Random();

                int rndTemplateIndex = rnd.Next(0, context.ChallengeTemplates.Where(x => x.IsActive && x.Type == type && (lectureId.HasValue && x.LectureId == lectureId || !lectureId.HasValue)).Count());

                var templates = context.ChallengeTemplates.Include(x => x.ChallengeTemplateItems)
                    .Where(x => x.IsActive && (lectureId.HasValue && x.LectureId == lectureId || !lectureId.HasValue) && x.ChallengeTemplateItems.Where(y => y.IsActive).Any())
                    .Skip(rndTemplateIndex).Take(1).FirstOrDefault();

                #region NullControl
                if (templates == null)
                    return new Result<List<Entity.Question.QuestionMain>>
                    {
                        Success = false,
                        Message = "Kayıt bulunamadı",
                        Data = new List<Entity.Question.QuestionMain>()
                    };

                if (templates.ChallengeTemplateItems == null)
                    return new Result<List<Entity.Question.QuestionMain>>
                    {
                        Success = false,
                        Message = "Kayıt bulunamadı",
                        Data = new List<Entity.Question.QuestionMain>()
                    };
                #endregion

                var result = new List<Entity.Question.QuestionMain>();
                bool completed = false;
                int seo = 1;
                foreach (var item in templates.ChallengeTemplateItems.OrderBy(x=>x.Seo).AsEnumerable())
                {

                    var question = context.QuestionMain.Include(x => x.QuestionExams).Include(x => x.QuestionAnswers).AsQueryable();

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
                            while (!completed)
                            {
                                rndQ = rnd.Next(0, questionTemplateListCount);
                                if (!result.Any(x => x.Id == questionTemplateList[rndQ].Id))
                                {
                                    questionTemplateList[rndQ].Seo = seo;
                                    seo++;
                                    result.Add(questionTemplateList[rndQ]);
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

                    }
                    else
                    {
                        for (int i = 0; i < questionTemplateListCount; i++)
                        {
                            if (!result.Any(x => x.Id == questionTemplateList[i].Id))
                            {
                                questionTemplateList[i].Seo = seo;
                                seo++;
                                result.Add(questionTemplateList[i]);
                                if (result.Count == questionCount)
                                {
                                    completed = true;
                                    break;
                                }
                            }
                        }
                    }

                    if (completed)
                        break;
                }

                return new Result<List<Entity.Question.QuestionMain>>
                {
                    Data = result,
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


        public bool challengeServiceIsOpen()
        {
            var cValue = ConfigurationValues.FirstOrDefault(x => x.Key == ConfigKey.ChallengeServiceIsOpen);
            if (cValue != null)
            {
                if (cValue.Values == null)
                    return false;
                return cValue.Values.ToString() == "1";
            }
            return false;
        }
        public int minimumSecondEntryChallenge()
        {

            var cValue = ConfigurationValues.FirstOrDefault(x => x.Key == ConfigKey.MinimumSecondEntryChallenge);
            if (cValue != null)
            {
                return Convert.ToInt32(cValue.Values);
            }
            return -1;
        }
        public int challengeNextSecond()
        {

            var cValue = ConfigurationValues.FirstOrDefault(x => x.Key == ConfigKey.ChallengeNextSecond);
            if (cValue != null)
            {
                return Convert.ToInt32(cValue.Values);
            }
            return -1;
        }

        public int challengeAttemptSecond()
        {

            var cValue = ConfigurationValues.FirstOrDefault(x => x.Key == ConfigKey.ChallengeAttemptSecond);
            if (cValue != null)
            {
                return Convert.ToInt32(cValue.Values);
            }
            return -1;
        }

        public int practiceModeQuestionCount()
        {

            var cValue = ConfigurationValues.FirstOrDefault(x => x.Key == ConfigKey.PracticeModeQuestionCount);
            if (cValue != null)
            {
                return Convert.ToInt32(cValue.Values);
            }
            return -1;
        }
        public int practiceModeExamDuration()
        {
            var cValue = ConfigurationValues.FirstOrDefault(x => x.Key == ConfigKey.PracticeModeExamDuration);
            if (cValue != null)
            {
                return Convert.ToInt32(cValue.Values);
            }
            return -1;
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