using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnswerQuestionApp.Service.Models;
using AqApplication.Core.Type;
using AqApplication.Repository.Challenge;
using AqApplication.Repository.Question;
using AqApplication.Repository.ViewModels;
using AqApplication.Service.Utilities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AqApplication.Service.Controllers
{
    [Route("api/Question")]
    [EnableCors("AllowAll")]
    [Produces("application/json")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IChallenge _iChallenge;
        private readonly IQuestion _iQuestion;
        private readonly ILogger<QuestionController> _logger;

        public QuestionController(IChallenge iChallenge, ILogger<QuestionController> logger, IQuestion iQuestion)
        {
            _iChallenge = iChallenge;
            _logger = logger;
            _iQuestion = iQuestion;
        }
        [Route("GetRandomQuestion")]
        [HttpGet]
        public ActionResult<List<ChallengeQuestionViewModel>> GetRandomQuestion()
        {
            _logger.LogInformation("Test request logging!");
            var result = _iChallenge.RandomQuestion("");
            if (!result.Success)
                return BadRequest();
            return Ok(result.Data);
        }
        [Route("SetChallengeAnswer")]
        [HttpPost]
        public ActionResult<Result> SetChallengeAnswer([FromBody]QuestionAnswerDto model)
        {
            var result = _iChallenge.SetChallengeAnswer(new ChallengeQuestionAnswerViewModel
            {
                AnswerIndex = model.AnswerIndex,
                ChallengeId = model.ChallengeId,
                QuestionId = model.QuestionId,
                UserId = model.userId
            });
            if (!result.Success)
                return BadRequest();
            return Ok(result);
        }
        [Route("GetResultChallenge")]
        [HttpGet]
        public ActionResult<Result<List<ChallengeUserViewModel>>> GetResultChallenge(int ChallengeId)
        {
            var result = _iChallenge.GetResultChallenge(ChallengeId, HttpContextUserInfo.GetUserId(HttpContext.User.Identity));
            if (!result.Success)
                return BadRequest();
            return Ok(result.Data);
        }

        [Route("GetChallengeQuestions")]
        [HttpGet]
        public ActionResult<Result<List<ChallengeQuestionViewModel>>> GetChallengeQuestions(int ChallengeId)
        {
            var result = _iChallenge.ChallengeQuestions(ChallengeId);
            if (!result.Success)
                return BadRequest();
            return Ok(result.Data);
        }
        [Route("GetPractiveModeLectures")]
        [HttpGet]
        public ActionResult<Result<List<ChallengeQuestionViewModel>>> GetPractiveModeLectures(int ExamId)
        {
            var result = _iQuestion.GetLecturesByExamId(ExamId);
            if (!result.Success)
                return BadRequest();

            return Ok(result.Data.ToDictionary(t => t.Id, t => t.Name));
        }
        [Route("CreatePractiveModeChallenge")]
        [HttpGet]
        public ActionResult<Result<List<ChallengeQuestionViewModel>>> CreatePractiveModeChallenge(int lectureId, string userId)
        {
            var result = _iChallenge.CreateChallenge(userId, lectureId, Entity.Constants.ChallengeTypeEnum.PracticeMode);
            if (!result.Success)
                return Ok(result);

            _iChallenge.AddChallengeSession(userId, result.InstertedId);

            return Ok(new { Success = true, Data = result.Data, Duration = _iChallenge.practiceModeExamDuration(), ChallangeId = result.InstertedId });
        }
        [Route("GetResultPractiveModeChallenge")]
        [HttpGet]
        public ActionResult<Result<QuizResultViewModel>> GetResultPractiveModeChallenge(int ChallengeId, string userId)
        {
            var result = _iChallenge.GetResultChallenge(ChallengeId, userId);
            if (!result.Success)
                return BadRequest();

            return Ok(result.Data);
        }

    }
}