using AqApplication.Entity.Common;
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
        [Display(Name = "Name")]
        [Required(ErrorMessage = "RequiredField")]
        public string Name { get; set; }
        [NotMapped]
        public string[] Exams { get; set; }
        public virtual ICollection<ExamLecture> ExamLectures { get; set; }
    }

    public class Class : BaseEntity
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "RequiredField")]
        public string Name { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
    }

    public class Exam : BaseEntity
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "RequiredField")]
        public string Name { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
 
    }
    public class Subject : BaseEntity
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "RequiredField")]
        public string Name { get; set; }
        [Required(ErrorMessage = "RequiredField")]
        [Display(Name = "Lecture")]
        public int LectureId { get; set; }
        [ForeignKey("LectureId")]
        public Lecture Lecture { get; set; }
    }
    public class SubSubject : BaseEntity
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Bu alan boş geçilmez")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Bu alan boş geçilmez")]
        [Display(Name = "Subject")]
        public int SubjectId { get; set; }

        [ForeignKey("SubjectId")]
        public Subject Subject { get; set; }
    }

    public class ExamLecture : BaseEntity
    {
        public int LectureId { get; set; }
        [ForeignKey("LectureId")]
        public Lecture Lecture { get; set; }
        public int ExamId { get; set; }
        [ForeignKey("ExamId")]
        public Exam Exam { get; set; }
    }
    public class Difficulty: BaseEntity
    {
        public short Value { get; set; }
        [MaxLength(255,ErrorMessage ="Bu alan en fazla 250 karakter olabilir")]
        public string Name { get; set; }
    }
}