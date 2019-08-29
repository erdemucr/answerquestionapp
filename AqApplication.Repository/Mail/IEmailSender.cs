using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnswerQuestionApp.Repository.Mail
{
    public interface IEmailSender
    {
        string SendEmail(string Subject, string Email, string Message);
    }
}
