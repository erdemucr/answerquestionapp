using AqApplication.Core.Type;
using System;
using System.Collections.Generic;

namespace AnswerQuestionApp.Manage.Models
{
    public class ChallengeListModel
    {

        public int Id { get; set; }
        public int AttemptCount { get; set; }
        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }

    public class ChallengeTemplateListModel
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public int AttemptCount { get; set; }
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string Creator { get; set; }
        public string Editor { get; set; }

        public bool IsActive { get; set; }
    }

    public class ChallengeTemplateModel
    {
        public AqApplication.Entity.Challenge.ChallengeTemplateItems ChallengeItem { get; set; }
        public IEnumerable<AqApplication.Entity.Challenge.ChallengeTemplateItems> ChallengeList { get; set; }
    }
}
