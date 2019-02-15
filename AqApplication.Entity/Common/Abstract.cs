using AqApplication.Entity.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AqApplication.Entity.Common
{
  public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }


        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? Seo { get; set; }

        public string Creator { get; set; }

        public string Editor { get; set; }

        [Display(Name = "Durumu:")]
        public bool IsActive { get; set; }

        [ForeignKey("Creator")]
        public ApplicationUser AppUserCreator { get; set; }

        [ForeignKey("Editor")]
        public ApplicationUser AppUserEditor { get; set; }

    }
}