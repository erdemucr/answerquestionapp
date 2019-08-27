using AnswerQuestionApp.Entity.Configuration;
using AqApplication.Core.Type;
using System.Collections.Generic;

namespace AnswerQuestionApp.Repository.Configuration
{
    public interface IConfigurationValues
    {
        Result<Dictionary<ConfigKey, string>> GetByKey(ConfigKey key);

        Result<IEnumerable<ConfigurationValues>> GetAll();

        Result Edit(List<ConfigurationValues> list);
    }
}
