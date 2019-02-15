﻿using AqApplication.Entity.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AqApplication.Entity.Question
{
    public class Lecture : BaseEntity
    {
        [Display(Name = "Adı:")]
        [Required(ErrorMessage = "Bu alan boş geçilmez")]
        public string Name { get; set; }
    }

    public class Class : BaseEntity
    {
        [Display(Name = "Adı:")]
        [Required(ErrorMessage = "Bu alan boş geçilmez")]
        public string Name { get; set; }
        [Display(Name = "Açıklama")]
        public string Description { get; set; }
    }

    public class Exam : BaseEntity
    {
        [Display(Name = "Adı:")]
        [Required(ErrorMessage = "Bu alan boş geçilmez")]
        public string Name { get; set; }
        [Display(Name = "Açıklama")]
        public string Description { get; set; }
    }
    public class Subject : BaseEntity
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
    public class SubSubject : BaseEntity
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