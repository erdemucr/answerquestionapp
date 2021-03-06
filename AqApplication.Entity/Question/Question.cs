﻿using AqApplication.Entity.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AqApplication.Entity.Question
{
    public class QuestionMain: BaseEntity
    {
        [Display(Name="Başlık")]
       public string MainTitle { get; set; }
        [Display(Name = "Image")]
        public string MainImage { get; set; }
        [Display(Name = "SubSubject")]
        public int? SubSubjectId { get; set; }

        [Display(Name = "Subject")]
        public int? SubjectId { get; set; }


        [Display(Name = "Lecture")]
        public int? LectureId { get; set; }

        [Display(Name = "Difficulty")]
        public int? Difficulty { get; set; }

        [Display(Name = "Offer")]
        public bool? Offer { get; set; }
        [Required(ErrorMessage = "RequiredField")]
        public bool Licence { get; set; }

        public int? QuestionPdfId { get; set; }

        [ForeignKey("QuestionPdfId")]
        public QuestionPdf QuestionPdf { get; set; }

        public int CorrectAnswer { get; set; }
        public int AnswerCount { get; set; }
        [ForeignKey("LectureId")]
        public Lecture Lecture { get; set; }
        [ForeignKey("SubSubjectId")]
        public SubSubject SubSubject { get; set; }
        [ForeignKey("SubjectId")]
        public Subject Subject { get; set; }
        public ICollection<QuestionAnswer> QuestionAnswers { get; set; }
        public ICollection<QuestionClass> QuestionClasses { get; set; }
        public ICollection<QuestionExam> QuestionExams { get; set; }

        public int? WidthImage { get; set; }
        public int? HeightImage { get; set; }
    }

    public class QuestionAnswer 
    {
        [Key]
        public int Id { get; set; }
        [Display(Name="Soru")]
        [Required(ErrorMessage ="Bu alan boş geçilmez")]
        public int QuestionId {get;set;}


        public string Title { get; set; }
        public string ImageUrl { get; set; }

        public bool IsTrue { get; set; }

        public int? Seo { get; set; }

        public string Description { get; set; }
        [ForeignKey("QuestionId")]
        public QuestionMain Question { get; set; }

    }

    public class QuestionExam:BaseEntity
    {
        [Display(Name="Soru")]
        [Required(ErrorMessage = "Bu alan boş geçilmez")]
        public int QuestionId { get; set; }

        [ForeignKey("QuestionId")]
        public QuestionMain Question { get; set; }

        [Display(Name = "Sınav")]
        [Required(ErrorMessage = "Bu alan boş geçilmez")]
        public int ExamId { get; set; }

        [ForeignKey("ExamId")]
        public Exam Exam { get; set; }

    }

    public class QuestionClass : BaseEntity
    {
        [Display(Name = "Soru")]
        [Required(ErrorMessage = "Bu alan boş geçilmez")]
        public int QuestionId { get; set; }

        [ForeignKey("QuestionId")]
        public QuestionMain Question { get; set; }

        [Display(Name = "Sınıf")]
        [Required(ErrorMessage = "Bu alan boş geçilmez")]
        public int ClassId { get; set; }

        [ForeignKey("ClassId")]
        public Class Class { get; set; }
    }

    public class QuestionPdf : BaseEntity
    {
        [Display(Name = "Adı:")]
        [Required(ErrorMessage = "Bu alan boş geçilmez")]
        public string Name { get; set; }
        public int TotalPage { get; set; }
        public string PdfUrl { get; set; }
        public string Description { get; set; }

        public virtual ICollection<QuestionPdfContent> QuestionPdfContent { get; set; }
    }
    public class QuestionPdfContent
    {
        [Key]
        public int Id { get; set; }
        public string FileName { get; set; }

        public string ImageUrl { get; set; }

        public int QuestionId { get; set; }

        public int Seo { get; set; }

        [ForeignKey("QuestionId")]
        public  QuestionPdf QuestionPdf { get;set;}


    }
   
}