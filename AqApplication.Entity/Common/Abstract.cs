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

        public bool? IsDeleted { get; set; }
        [Display(Name = "CreatedDate")]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "ModifiedDate")]
        public DateTime? ModifiedDate { get; set; }

        public int? Seo { get; set; }
        [Display(Name = "Creator")]
        public string Creator { get; set; }
        [Display(Name = "Editor")]
        public string Editor { get; set; }

        [Display(Name = "IsActive")]
        public bool IsActive { get; set; }

        [ForeignKey("Creator")]
        public ApplicationUser AppUserCreator { get; set; }

        [ForeignKey("Editor")]
        public ApplicationUser AppUserEditor { get; set; }

    }
}