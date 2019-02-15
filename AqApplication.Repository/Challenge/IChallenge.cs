using AqApplication.Core.Type;
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

        Result<List<ChallengeChallengeUserViewModel>> GetResultChallenge(int challengeId, string userId);

        Result<AqApplication.Entity.Challenge.Challenge> AddChallenge(string userId, int challengeType);

        Result<List<ChallengeQuestionViewModel>> ChallengeQuestions(int challengeId);
    }
}