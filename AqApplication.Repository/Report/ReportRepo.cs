using AnswerQuestionApp.Repository.Mail;
using AnswerQuestionApp.Repository.ViewModels;
using AqApplication.Core.Type;
using AqApplication.Entity.Challenge;
using AqApplication.Entity.Constants;
using AqApplication.Entity.Identity.Data;
using AqApplication.Repository.FilterModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnswerQuestionApp.Repository.Report
{
    public class ReportRepo : IReport
    {
        private ApplicationDbContext context;
        private bool disposedValue = false;
        private readonly int PageSize = 20;
        private readonly IEmailSender iEmailSender;

        public ReportRepo(ApplicationDbContext _context, IMemoryCache memoryCache, IEmailSender _iEmailSender)
        {
            context = _context;
            iEmailSender = _iEmailSender;
        }

        public Result<IEnumerable<HistoryChallengeViewModel>> GetHistoryChallengeByUserId(HistoryFilterModel model)
        {
            try
            {
                var list = context.ChallengeSessions.Include(x => x.Challenge.ChallengeType).Where(x => x.IsCompleted && x.UserId == model.userId)
                  .OrderByDescending(x => x.Id)
                  .Skip(model.CurrentPage.HasValue ? ((model.CurrentPage.Value - 1) * PageSize) : 0).Take(PageSize)
                  .AsEnumerable()
                  .Select(x => new HistoryChallengeViewModel
                  {
                      CorrectCount = x.CorrectCount ?? -1,
                      Mark = x.TotalScore,
                      Date = x.Challenge.StartDate.HasValue ? x.Challenge.StartDate.Value.ToString("dd/MM/yyyy") : string.Empty,
                      Hour = x.Challenge.StartDate.HasValue ? x.Challenge.StartDate.Value.ToString("HH:mm") : string.Empty,
                      ChallengeType = x.Challenge.ChallengeType.Description,
                      ChallengeId = x.Challenge.Id
                  })
                  .ToList();


                return new Result<IEnumerable<HistoryChallengeViewModel>>
                {
                    Data = list,
                    Success = true,
                    Message = "Soru listesini görüntülemektesiniz"
                };
            }
            catch (Exception ex)
            {
                return new Result<IEnumerable<HistoryChallengeViewModel>>(ex);
            }


        }

        public Result<IEnumerable<StatisticChartViewModel>> GetStatisticChartData(HistoryFilterModel model)
        {
            try
            {
                var list = context.ChallengeSessions.Include(x => x.Challenge.ChallengeType).Where(x => x.IsCompleted && x.UserId == model.userId)
                  .OrderByDescending(x => x.Id)
                  .Where(x => x.StartDate.HasValue && x.StartDate.Value >= DateTime.Now.AddDays(model.Day * -1))
                  .AsEnumerable()
                  .Select(x => new StatisticChartViewModel
                  {
                      Mark = x.TotalScore,
                      Date = x.Challenge.StartDate.HasValue ? x.Challenge.StartDate.Value.ToString("dd/MM/yyyy") : string.Empty,
                      ChallengeType = x.Challenge.ChallengeType.Description,
                      ChallengeTypeId = x.Challenge.ChallengeTypeId,
                      ChallengeId = x.Challenge.Id
                  })
                  .ToList();


                return new Result<IEnumerable<StatisticChartViewModel>>
                {
                    Data = list,
                    Success = true,
                    Message = "Soru listesini görüntülemektesiniz"
                };
            }
            catch (Exception ex)
            {
                return new Result<IEnumerable<StatisticChartViewModel>>(ex);
            }


        }
    }
}
