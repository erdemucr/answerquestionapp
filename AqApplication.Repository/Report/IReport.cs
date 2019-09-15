using AnswerQuestionApp.Repository.ViewModels;
using AqApplication.Core.Type;
using AqApplication.Repository.FilterModels;
using System.Collections.Generic;

namespace AnswerQuestionApp.Repository.Report
{
    public interface IReport
    {
        Result<IEnumerable<HistoryChallengeViewModel>> GetHistoryChallengeByUserId(HistoryFilterModel model);

        Result<IEnumerable<StatisticChartViewModel>> GetStatisticChartData(HistoryFilterModel model);
    }
}
