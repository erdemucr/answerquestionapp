using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AnswerQuestionApp.Repository.Mail
{
    public class EmailSender : IEmailSender
    {


        public string SendEmail(string Subject, string Email, string Message)
        {

            try
            {
                // Credentials
                var credentials = new NetworkCredential("answerandquestionapp@gmail.com", "Aq123456");
                // Mail message
                var mail = new MailMessage()
                {
                    From = new MailAddress("answerandquestionapp@gmail.com"),
                    Subject = Subject,
                    Body = Message
                };
                mail.IsBodyHtml = true;
                mail.To.Add(new MailAddress(Email));
                // Smtp client
                var client = new SmtpClient()
                {
                    Port = 587,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = "smtp.gmail.com",
                    EnableSsl = true,
                    Credentials = credentials
                };
                client.Send(mail);
                return "Email Sent Successfully!";
            }
            catch (System.Exception e)
            {
                return e.Message;
            }

        }

    }
}
