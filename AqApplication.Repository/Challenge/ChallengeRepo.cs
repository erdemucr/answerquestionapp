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

namespace AqApplication.Repository.Challenge
{
    public class ChallengeRepo : IChallenge
    {
        private ApplicationDbContext context;
        private bool disposedValue = false;
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
                var questionList = addChallenge.Data.ChallengeQuestions.Select(x => new ChallengeQuestionViewModel
                {
                    ImageExits = true,
                    MainText = x.QuestionMain.MainTitle,
                    Image = x.QuestionMain.MainImage,
                    QuestionId = x.QuestionMain.Id,
                    AnswerCount = x.QuestionMain.AnswerCount,
                    //ChallengeId = x.ChallengeSessionId

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
                    ChallengeTypeId = (int)ChallengeTypeEnum.QuizMode,
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
                context.ChallengeQuestionAnswers.Add(new ChallengeQuestionAnswers
                {
                    CreatedDate = DateTime.Now,
                    ChallengeId = model.ChallengeId,
                    QuestionId = model.QuestionId,
                    UserId = model.UserId,
                    AnswerIndex = model.AnswerIndex,

                });
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


        public Result<List<ChallengeChallengeUserViewModel>> GetResultChallenge(int challengeId, string userId)
        {
            try
            {
                List<ChallengeQuestions> challangeQuestion = context.ChallengeQuizs.Include(x => x.QuestionMain).Where(x => x.ChallengeId == challengeId).ToList();

                List<ChallengeQuestionAnswers> challengeAnswer = context.ChallengeQuestionAnswers
                    .Include(x => x.ApplicationUser)
                    .Where(x => x.ChallengeId
                == challengeId).ToList();

                List<string> challengeUser = challengeAnswer.Select(x => x.UserId).Distinct().ToList();

                var resultList = new List<ChallengeChallengeUserViewModel>();

                int totalQuestion = challangeQuestion.Count();

                foreach (var user in challengeUser)
                {
                    var answers = challengeAnswer.Where(x => x.UserId == user && x.ChallengeId
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
                        UserName = challengeAnswer.FirstOrDefault(x => x.UserId == user).ApplicationUser.UserName
                    });

                }
                var resultordered = new List<ChallengeChallengeUserViewModel>();
                foreach (var item in resultList)
                {

                    resultordered.Add(new ChallengeChallengeUserViewModel
                    {

                        Mark = decimal.Round(((item.correct * 100) / totalQuestion), 2, MidpointRounding.AwayFromZero).ToString(),
                        UserName = item.UserName
                    });
                }

                var resultseqList = new List<ChallengeChallengeUserViewModel>();

                resultordered = resultordered.OrderByDescending(x => x.Mark).ToList();

                int i = 1;

                foreach (var item in resultordered)
                {

                    resultseqList.Add(new ChallengeChallengeUserViewModel
                    {

                        Mark = item.Mark,
                        Seq = i,
                        UserName = item.UserName
                    });
                    i++;
                }

                return new Result<List<ChallengeChallengeUserViewModel>>
                {
                    Data = resultseqList,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new Result<List<ChallengeChallengeUserViewModel>>(ex);
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