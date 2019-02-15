using AqApplication.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AqApplication.Entities.Challenge
{
    public class ChallengeType: BaseEntities
    {
        [MaxLength(30)]
        [Display(Name = "Kod")]
        public string Code { get; set; }
        [MaxLength(250)]
        [Display(Name ="Açıklma")]
        public string Description { get; set; }
    }
}