using AqApplication.Entity.Common;
using AqApplication.Entity.Identity.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnswerQuestionApp.Entity.Advisor
{
    public class Advisor : BaseEntity
    {
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        [Range(0, 100)]
        public decimal Rating { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        [NotMapped]
        [MinLength(6, ErrorMessage = "AtLeastChar")]
        [Required(ErrorMessage ="RequiredField")]
        [DataType(dataType: DataType.Password, ErrorMessage = "ValidPassword")]
        public string Password { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "RequiredField")]
        [DataType(dataType: DataType.EmailAddress, ErrorMessage = "ValidEmail")]
        public string Email { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "RequiredField")]
        public string PhoneNumber { get; set; }
    }
}
