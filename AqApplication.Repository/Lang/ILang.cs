using AnswerQuestionApp.Entity.Lang;
using System.Collections.Generic;

namespace AnswerQuestionApp.Repository.Lang
{
    public interface ILang 
    {
        List<LangContent> LangValues();

        string GetLangValue( L langKey, string lang);

    }
}
