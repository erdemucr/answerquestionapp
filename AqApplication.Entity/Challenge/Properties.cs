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
        [Display(Name = "Kod")]
        public string Code { get; set; }
        [MaxLength(250)]
        [Display(Name ="Açıklma")]
        public string Description { get; set; }
    }
}