﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AqApplication.Core.Type;
using AqApplication.Repository.Challenge;
using AqApplication.Repository.ViewModels;
using AqApplication.Service.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AqApplication.Service.Controllers
{
    [Route("api/Question")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IChallenge _iChallenge;
        private readonly ILogger<QuestionController> _logger;

        public QuestionController(IChallenge iChallenge, ILogger<QuestionController> logger)
        {
            _iChallenge = iChallenge;
            _logger = logger;
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
        [Route("GetRandomChallenge")]
        [HttpGet]
        public ActionResult<Result<List<ChallengeQuestionViewModel>>> GetRandomChallenge()
        {
            var result = _iChallenge.RandomChallenge(HttpContextUserInfo.GetUserId(HttpContext.User.Identity));
            if (!result.Success)
                return BadRequest();
            return Ok(result.Data);
        }
        [Route("SetChallengeAnswer")]
        [HttpPost]
        public Result SetChallengeAnswer(int ChallengeId, int AnswerIndex, int QuestionId, string userId
                )
        {
            return _iChallenge.SetChallengeAnswer(new ChallengeQuestionAnswerViewModel
            {
                AnswerIndex = AnswerIndex,
                ChallengeId = ChallengeId,
                QuestionId = QuestionId,
                UserId = HttpContextUserInfo.GetUserId(HttpContext.User.Identity)
            });
        }
        [Route("GetResultChallenge")]
        [HttpGet]
        public ActionResult<Result<List<ChallengeChallengeUserViewModel>>> GetResultChallenge(int ChallengeId)
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

    }
}