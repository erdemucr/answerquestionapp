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

        Result<List<ChallengeQuestionViewModel>> CreateChallenge(string userId, int? lectureId, ChallengeTypeEnum challengeType = ChallengeTypeEnum.RandomMode);

        Result SetChallengeAnswer(ChallengeQuestionAnswerViewModel model);

        Result<QuizResultViewModel> GetResultChallenge(int challengeId, string userId, ChallengeTypeEnum challengeType = ChallengeTypeEnum.RandomMode);

        Result<AqApplication.Entity.Challenge.Challenge> AddChallenge(string userId, ChallengeTypeEnum challengeType, int questionCount, int? lectureId);

        Result<List<ChallengeQuestionViewModel>> ChallengeQuestions(int challengeId);

        Result<Entity.Challenge.Challenge> GetLastChallengeByType(ChallengeTypeEnum type);

        Result<IEnumerable<Entity.Challenge.Challenge>> GetChallengesPaginated(ChallengeFilterModel model);

        Result AddChallengeSession(string userId, int challengeId, DateTime startDate);

        Result UpdateChallengeSessionCompleted(string userId, int challengeId, string totalMark, int correctCount, DateTime date);

        bool challengeServiceIsOpen();

        int minimumSecondEntryChallenge();

        int challengeNextSecond();

        int challengeAttemptSecond();

        int practiceModeQuestionCount();

        int practiceModeExamDuration();
    }
}