using AnswerQuestionApp.Repository.FilterModels;
using AqApplication.Core.Type;
using AqApplication.Entity.Constants;
using AqApplication.Repository.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AqApplication.Repository.Challenge
{
    public interface IChallenge
    {
        Result<List<ChallengeQuestionViewModel>> RandomQuestion(string userId);

         Result<List<ChallengeQuestionViewModel>> RandomChallenge(string userId);

        Result SetChallengeAnswer(ChallengeQuestionAnswerViewModel model);

        Result<ChallengeChallengeUserViewModel> GetResultChallenge(int challengeId, string userId);

        Result<AqApplication.Entity.Challenge.Challenge> AddChallenge(string userId);

        Result<List<ChallengeQuestionViewModel>> ChallengeQuestions(int challengeId);

        Result<Entity.Challenge.Challenge> GetLastChallengeByType(ChallengeTypeEnum type);

        Result<IEnumerable<Entity.Challenge.Challenge>> GetChallengesPaginated(ChallengeFilterModel model);

        Result AddChallengeSession(string userId, int challengeId);

        Result UpdateChallengeSessionCompleted(string userId, int challengeId, string totalMark, int correctCount);
    }
}