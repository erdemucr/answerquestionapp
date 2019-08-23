using AqApplication.Repository.Challenge;
using Hangfire;
using System;

namespace AnswerQuestionApp.Manage.HangFire
{
    public class ChallengeCron
    {
        private readonly IChallenge _iChallenge;
        public ChallengeCron(IChallenge iChallenge)
        {
            _iChallenge = iChallenge;
            RecurringJob.AddOrUpdate(() => ProcessRecurringJob(), "*/5 * * * *");
        }
        public void ProcessRecurringJob()
        {
            Console.WriteLine("Test deneme");
           // _iChallenge.RandomChallenge("11efabde-f29e-4240-aa5b-995d07169ced");
        }
    }
}
