using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnswerQuestionApp.Service.Models
{
    public class QuestionAnswerDto
    {
      public  int ChallengeId { get; set; }

        public int AnswerIndex { get; set; }

        public int QuestionId { get; set; }

        public string userId { get; set; }
    }
}
