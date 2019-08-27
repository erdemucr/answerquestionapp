using AqApplication.Entity.Common;
using AqApplication.Entity.Identity.Data;
using AqApplication.Entity.Question;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AqApplication.Entity.Challenge
{
    public class Challenge : BaseEntity
    {

        public int ChallengeTypeId { get; set; }

        [Display(Name = "Başlangıç Tarihi")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Bitiş Tarihi")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Süre Periodu")]
        public int? TimePeriod { get; set; }

        [ForeignKey("ChallengeTypeId")]
        public virtual ChallengeType ChallengeType { get; set; }

        public virtual ICollection<ChallengeSession> ChallengeSessions { get; set; }

        public virtual ICollection<ChallengeQuestions> ChallengeQuestions { get; set; }

    }
    public class ChallengeSession : BaseEntity
    {

        public int ChallengeId { get; set; }

        [Display(Name = "Başlangıç Tarihi")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Bitiş Tarihi")]
        public DateTime? EndDate { get; set; }

        public string UserId { get; set; }

        public bool IsCompleted { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("ChallengeId")]
        public virtual Challenge Challenge { get; set; }


    }

    public class ChallengeQuestions
    {
        [Key]
        public int Id { get; set; }

        public int Seo { get; set; }

        public int ChallengeId { get; set; }

        public int QuestionId { get; set; }

        [ForeignKey("QuestionId")]
        public QuestionMain QuestionMain { get; set; }

        [ForeignKey("ChallengeId")]
        public virtual Challenge Challenge { get; set; }


    }

    public class ChallengeQuestionAnswers
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int QuestionId { get; set; }

        public int AnswerIndex { get; set; }
        public int ChallengeId { get; set; }

        public string UserId { get; set; }

        public int TimeInterval { get; set; }

        [ForeignKey("ChallengeId")]
        public virtual Challenge Challenge { get; set; }

        //[ForeignKey("AnswerId")]
        //public virtual QuestionAnswer QuestionAnswer { get; set; }


        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

    }

    public class ChallengeTemplate : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        [Display(Name = "Başlangıç Tarihi")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Bitiş Tarihi")]
        public DateTime? EndDate { get; set; }

       public  ICollection<ChallengeTemplateItems> ChallengeTemplateItems { get; set; }
    }

    public class ChallengeTemplateItems : BaseEntity
    {
        public int ExamId { get; set; }

        [ForeignKey("ExamId")]
        public Exam Exam { get; set; }

        [Display(Name = "Zorluk")]
        public int? Difficulty { get; set; }

        [Display(Name = "Alt Konu")]
        public int? SubSubjectId { get; set; }

        [Display(Name = "Konu")]
        public int? SubjectId { get; set; }

        public int? QuestionPdfId { get; set; }

        public int ChallengeTemplateId { get; set; }
        [Display(Name = "Sayı")]
        public int Count { get; set; }

        [ForeignKey("ChallengeTemplateId")]
        public ChallengeTemplate ChallengeTemplate { get; set; }

        [ForeignKey("SubSubjectId")]
        public SubSubject SubSubject { get; set; }

        [ForeignKey("SubjectId")]
        public Subject Subject { get; set; }

        [ForeignKey("QuestionPdfId")]
        public QuestionPdf QuestionPdf { get; set; }
    }
}