using AqApplication.Entity.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AqApplication.Entity.Challenge
{
    public class ChallengeType: BaseEntity
    {
        [MaxLength(30)]
        [Display(Name = "Code")]
        public string Code { get; set; }
        [MaxLength(250)]
        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}