using AqApplication.Entity.Common;
using System.ComponentModel.DataAnnotations;

namespace AnswerQuestionApp.Entity.Configuration
{
   public  class ConfigurationValues : BaseEntity
    {
        [Display(Name = "Key")]
        public ConfigKey Key { get; set; }
        [Display(Name = "Values")]
        public string Values { get; set; }
        [MaxLength(200,ErrorMessage ="En fazla 200 karakter olabilir")]
        public string Description { get; set; }

    }

    public class RandomUser
    {
        [Key]
        public int Id { get; set; }

        public int Count { get; set; }
    }

    public enum ConfigKey
    {
        ChallengeNextSecond=0,
        ChallengeTimeDuration=1,
        ChallengeServiceIsOpen=2,
        ChallengeQuestionCount=3,
        MinimumSecondEntryChallenge=4,
        ChallengeAttemptSecond=5,
        PracticeModeQuestionCount=6,
        PracticeModeExamDuration=7
    }

}
