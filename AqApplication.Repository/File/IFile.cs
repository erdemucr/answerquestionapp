using AqApplication.Core.Type;
using AqApplication.Entity.Question;
using AqApplication.Repository.FilterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AqApplication.Repository.File
{
    public interface IFile
    {
        Result<IEnumerable<QuestionPdf>> GetQuestionPdf(DocumentFilterModel model);

        Result<IEnumerable<QuestionPdfContent>> GetQuestionPdfContents(int id);

        Result AddQuestionPdf(QuestionPdf model, string userId);

        Result AddQuestionContentPdf(QuestionPdfContent model);

        Result<QuestionPdf> GetQuestionPdf(int id);
    }
}