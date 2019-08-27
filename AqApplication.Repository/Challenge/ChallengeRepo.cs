﻿using AqApplication.Core.Type;
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

namespace AqApplication.Repository.Challenge
{
    public class ChallengeRepo : IChallenge
    {
        private ApplicationDbContext context;
        private bool disposedValue = false;
        private readonly int PageSize = 20;
        private const int challengeExpDay = 12, challengeQuestionLimit = 8;
        public ChallengeRepo(ApplicationDbContext _context)
        {
            context = _context;
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
                var addChallenge = this.AddChallenge(challengeExpDay, challengeQuestionLimit, userId);

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

        private Result<AqApplication.Entity.Challenge.Challenge> AddChallenge(int challengeExpDay, int challengeQuestionLimit, string userId)
        {
            try
            {
                var questions = context.QuestionMain.OrderByDescending(X => X.Id).Take(challengeQuestionLimit).AsEnumerable();
                var list = questions.Select(x => new ChallengeQuestions
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
                    EndDate = DateTime.Now.AddDays(challengeExpDay),
                    CreatedDate = DateTime.Now,
                    ChallengeTypeId = (int)ChallengeTypeEnum.RandomMode,
                    ChallengeQuestions = list,
                    ChallengeSessions = new List<ChallengeSession>
                    {
                        new ChallengeSession
                        {
                            CreatedDate=DateTime.Now,
                            IsActive=true,
                            Creator=userId,
                            UserId=userId,
                            EndDate=DateTime.Now.AddDays(challengeExpDay),
                            StartDate=DateTime.Now,
                            IsCompleted=false
                        }
                    }

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
        public Result<AqApplication.Entity.Challenge.Challenge> AddChallenge(string userId, int challengeType)
        {
            try
            {

                var model = new AqApplication.Entity.Challenge.Challenge
                {
                    IsActive = true,
                    Creator = userId,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(challengeExpDay),
                    CreatedDate = DateTime.Now,
                    ChallengeTypeId = challengeType,
                    ChallengeSessions = new List<ChallengeSession>
                    {
                        new ChallengeSession
                        {
                            CreatedDate=DateTime.Now,
                            IsActive=true,
                            Creator=userId,
                            UserId=userId,
                            EndDate=DateTime.Now.AddDays(challengeExpDay),
                            StartDate=DateTime.Now,
                            IsCompleted=false
                        }
                    }

                };

                context.Challenge.Add(model);
                context.SaveChanges();

                Task.Run(() =>
                {
                    AddChallengeQuestions(model.Id);
                });
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
                var list = context.Challenge
                   .AsQueryable();
                if (model.StartDate.ToDate().HasValue)
                    list = list.Where(x => x.CreatedDate >= model.StartDate.ToDate().Value);
                if ( model.EndDate.ToDate().HasValue)
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