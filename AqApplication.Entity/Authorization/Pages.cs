using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnswerQuestionApp.Entity.Authorization
{
   public class Pages
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(200, ErrorMessage ="MaxLengthError")]
        public string Name { get; set; }

        public string Url { get; set; }
    }
}
