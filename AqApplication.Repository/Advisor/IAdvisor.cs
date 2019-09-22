using AqApplication.Core.Type;
using AqApplication.Repository.FilterModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnswerQuestionApp.Repository.Advisor
{
    public interface IAdvisor
    {
        Result<IEnumerable<Entity.Advisor.Advisor>> GetAdvisors(UserFilterModel model);

        Result AddAdvisor(Entity.Advisor.Advisor model, string userId);

        Result<Entity.Advisor.Advisor> GetAdvisorByKey(int id);

        Result EditAdvisor(Entity.Advisor.Advisor model, string userId);

        Result DeleteAdvisor(int id, string userId);
    }
}
