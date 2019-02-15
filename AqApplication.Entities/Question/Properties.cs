using AqApplication.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AqApplication.Entities.Question
{
    public class Lecture : BaseEntities
    {
        [Display(Name = "Adı:")]
        [Required(ErrorMessage = "Bu alan boş geçilmez")]
        public string Name { get; set; }
    }

    public class Class : BaseEntities
    {
        [Display(Name = "Adı:")]
        [Required(ErrorMessage = "Bu alan boş geçilmez")]
        public string Name { get; set; }
        [Display(Name = "Açıklama")]
        public string Description { get; set; }
    }

    public class Exam : BaseEntities
    {
        [Display(Name = "Adı:")]
        [Required(ErrorMessage = "Bu alan boş geçilmez")]
        public string Name { get; set; }
        [Display(Name = "Açıklama")]
        public string Description { get; set; }
    }
    public class Subject : BaseEntities
    {
        [Display(Name = "Adı:")]
        [Required(ErrorMessage = "Bu alan boş geçilmez")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Bu alan boş geçilmez")]
        [Display(Name = "Branş")]
        public int LectureId { get; set; }
        [ForeignKey("LectureId")]
        public Lecture Lecture { get; set; }
    }
    public class SubSubject : BaseEntities
    {
        [Display(Name = "Adı:")]
        [Required(ErrorMessage = "Bu alan boş geçilmez")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Bu alan boş geçilmez")]
        [Display(Name = "Konu")]
        public int SubjectId { get; set; }

        [ForeignKey("SubjectId")]
        public Subject Subject { get; set; }
    }
}