using AqApplication.Entity.Common;
using AqApplication.Entity.Identity.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnswerQuestionApp.Entity.Advisor
{
   public class Advisor: BaseEntity
    {
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public decimal Rating { get; set; }

        public string Description { get; set; }
    }
}
