using AqApplication.Entity.Common;
using System.ComponentModel.DataAnnotations;

namespace AnswerQuestionApp.Entity.Configuration
{
   public  class ConfigurationValues : BaseEntity
    {
        [Display(Name = "Anahtar")]
        public ConfigKey Key { get; set; }
        [Display(Name = "Değer")]
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
        ChallengeTimeSecond=1,
        ChallengeServiceIsOpen=2,
        ChallengeQuestionCount=3
    }

}
