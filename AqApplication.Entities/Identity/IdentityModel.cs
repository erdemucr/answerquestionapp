using AqApplication.Entities.Challenge;
using AqApplication.Entities.Constants;
using AqApplication.Entities.Question;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AqApplication.Entities.Identity.Data
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

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("IndentityContext", throwIfV1Schema: false)
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
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
