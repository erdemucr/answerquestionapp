using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AqApplication.Manage.Models
{
    public class QuestionAddModel
    {


        public string MainTitle { get; set; }
        public string MainImage { get; set; }
        public int? SubjectId { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilmez")]
        public int LectureId { get; set; }
        public int? SubSubjectId { get; set; }
        [Required(ErrorMessage = "Bu alan boş geçilmez")]
        public int? Difficulty { get; set; }
        public bool? Offer { get; set; }
        [Required(ErrorMessage = "Bu alan boş geçilmez")]
        public bool Licence { get; set; }
        [Required(ErrorMessage = "Bu alan boş geçilmez")]
        public string[] Exams { get; set; }

        public int? QuestionPdfId { get; set; }

        public int? CurrentPage { get; set; }
        public List<AnswerAddModel> QuestionAnswer { get; set; }

        public int TrueOption { get; set; }
        public bool Option4  {get;set;}
    }

    public class AnswerAddModel
    {
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public bool IsTrue { get; set; }

    }
    public class AnswerListModel
    {
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public bool IsTrue { get; set; }
        public int Seo { get; set; }
 
    }

    public class QuestionListModel
    {

        public int Id { get; set; }
        public string MainTitle { get; set; }
        public string MainImage { get; set; }
        public int SubSubjectId { get; set; }
        public int? Difficulty { get; set; }
        public bool? Offer { get; set; }
        public bool Licence { get; set; }
        public string[] Exams { get; set; }
        public List<AnswerAddModel> QuestionAnswer { get; set; }
        public string Creator { get; set; }
        public string Editor { get; set; }
        public string CorrectAnswer { get; set; }
        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
    public class QuestionPdfViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int TotalPage { get; set; }

        public string PdfUrl { get; set; }

    }
    public class CustomSelectModel
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }
    
}