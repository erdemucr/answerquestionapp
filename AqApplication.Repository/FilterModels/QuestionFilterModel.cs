using AqApplication.Core.Type;

namespace AqApplication.Repository.FilterModels
{
    public class QuestionFilterModel: BaseFilterModel
    {
    }

    public class DocumentFilterModel: BaseFilterModel
    {
    }
    public class PdfFilterModel : BaseFilterModel
    {
    }
    public class HistoryFilterModel : BaseFilterModel
    {
        public string userId { get; set; }
    
        public int Day { get; set; }
    }
}