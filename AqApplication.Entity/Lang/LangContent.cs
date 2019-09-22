using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnswerQuestionApp.Entity.Lang
{
    public class LangContent
    {
        [Key]
        public int Id { get; set; }
        public L Key { get; set; }
        public string TrValue { get; set; }

        public string EngValue { get; set; }
    }
}
