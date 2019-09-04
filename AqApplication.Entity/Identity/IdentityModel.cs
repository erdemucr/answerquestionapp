using System;
using AqApplication.Entity.Challenge;
using AqApplication.Entity.Constants;
using AqApplication.Entity.Question;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using System.IO;
using AqApplication.Entity.Logging;
using AnswerQuestionApp.Entity.Configuration;

namespace AqApplication.Entity.Identity.Data
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Adı")]
        public string FirstName { get; set; }
        [Display(Name = "Soyadı")]
        public string LastName { get; set; }
        [Display(Name = "Üye Tipi")]
        public MemberType MemberType { get; set; }
        [Display(Name = "Cep Telefonu")]
        public string TelNo { get; set; }
        [Display(Name = "Kayıt Tarihi")]
        public DateTime RegisterDate { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<QuestionMain> QuestionMain { get; set; }
        public DbSet<SubSubject> SubSubjects { get; set; }

        public DbSet<Class> Classes { get; set; }
        public DbSet<QuestionAnswer> QuestionAnswers { get; set; }
        public DbSet<QuestionClass> QuestionClass { get; set; }
        public DbSet<QuestionPdf> QuestionPdfs { get; set; }
        public DbSet<QuestionPdfContent> QuestionPdfContent { get; set; }

        public DbSet<ChallengeType> ChallengeTypes { get; set; }

        public DbSet<Challenge.Challenge> Challenge { get; set; }

        public DbSet<ChallengeSession> ChallengeSessions { get; set; }

        public DbSet<ChallengeQuestions> ChallengeQuizs { get; set; }

        public DbSet<QuestionExam> QuestionExams { get; set; }

        public DbSet<ChallengeQuestionAnswers> ChallengeQuestionAnswers { get; set; }

        public DbSet<HostCallLoggingModel> HostCallLogging { get; set; }

        public DbSet<ConfigurationValues> ConfigurationValues { get; set; }
        public DbSet<RandomUser> RandomUsers { get; set; }

        public DbSet<ChallengeTemplate> ChallengeTemplates { get; set; }

        public DbSet<ChallengeTemplateItems> ChallengeTemplateItems { get; set; }

        public DbSet<ExamLecture> ExamLectures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }


    }
}
