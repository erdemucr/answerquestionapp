using AqApplication.Entity.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AnswerQuestionApp.Entity.Message
{
   public class ChatHistory
    {
        public int Id { get; set; }
        [DisplayName("Message")]
        public string Message { get; set; }

        [DisplayName("Sender")]
        public string Sender { get; set; }

        [DisplayName("Receiver")]
        public string Receiver { get; set; }
        [ForeignKey("Sender")]
        public ApplicationUser SenderUser { get; set; }
        [ForeignKey("Receiver")]
        public ApplicationUser ReceiverUser { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
