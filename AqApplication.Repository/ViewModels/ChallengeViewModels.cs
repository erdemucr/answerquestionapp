using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AqApplication.Repository.ViewModels
{
     public class ChallengeQuestionViewModel
    {
        public string MainText { get; set; }

        public bool ImageExits { get; set; }

        public string Image { get; set; }

        public int QuestionId { get; set; }

        public int AnswerCount { get; set; }

        public int ChallengeId { get; set; }

        public int CorrectAnswer { get; set; }

        public int QuizDuration { get; set; }

        public int? ImageWidth { get; set; }

        public int? ImageHeight { get; set; }

        public int Seo { get; set; }
    }
    public class ChallengeQuestionAnswerViewModel
    {
        public int ChallengeId { get; set; }

        public int AnswerIndex { get; set; }

        public string UserId { get; set; }

        public int QuestionId { get; set; }

    }

    public class ChallengeResultViewModel
    {
        public int CorrentCount { get; set; }

        public int WrongCount { get; set; }

        public string TotalTime { get; set; }
    }


    public class ChallengeUserViewModel
    {
        public string UserName { get; set; }

        public string Mark { get; set; }

        public int Seq { get; set; }

        public int correct { get; set; }

    }
    public class QuestionAnswerViewModel
    {
        public int Seo { get; set; }
        public int AnswerCount { get; set; }

        public int CorrectAnswer { get; set; }

        public int UserAnswer { get; set; }
    }
    public class QuizResultViewModel
    {
        public ChallengeUserViewModel ChallengeUserViewModel { get; set; }

        public List<QuestionAnswerViewModel> QuestionAnswerViewModel { get; set; }
    }
}