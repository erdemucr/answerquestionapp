﻿// <auto-generated />
using System;
using AqApplication.Entity.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AqApplication.Entity.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20190823085112_SubSubjectAdded")]
    partial class SubSubjectAdded
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AqApplication.Entity.Challenge.Challenge", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ChallengeTypeId");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Creator");

                    b.Property<string>("Editor");

                    b.Property<DateTime?>("EndDate");

                    b.Property<bool>("IsActive");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<int?>("Seo");

                    b.Property<DateTime?>("StartDate");

                    b.Property<int?>("TimePeriod");

                    b.HasKey("Id");

                    b.HasIndex("ChallengeTypeId");

                    b.HasIndex("Creator");

                    b.HasIndex("Editor");

                    b.ToTable("Challenge");
                });

            modelBuilder.Entity("AqApplication.Entity.Challenge.ChallengeQuestionAnswers", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AnswerIndex");

                    b.Property<int>("ChallengeId");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int>("QuestionId");

                    b.Property<int>("TimeInterval");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ChallengeId");

                    b.HasIndex("UserId");

                    b.ToTable("ChallengeQuestionAnswers");
                });

            modelBuilder.Entity("AqApplication.Entity.Challenge.ChallengeQuestions", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ChallengeId");

                    b.Property<int>("QuestionId");

                    b.Property<int>("Seo");

                    b.HasKey("Id");

                    b.HasIndex("ChallengeId");

                    b.HasIndex("QuestionId");

                    b.ToTable("ChallengeQuizs");
                });

            modelBuilder.Entity("AqApplication.Entity.Challenge.ChallengeSession", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ChallengeId");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Creator");

                    b.Property<string>("Editor");

                    b.Property<DateTime?>("EndDate");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsCompleted");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<int?>("Seo");

                    b.Property<DateTime?>("StartDate");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ChallengeId");

                    b.HasIndex("Creator");

                    b.HasIndex("Editor");

                    b.HasIndex("UserId");

                    b.ToTable("ChallengeSessions");
                });

            modelBuilder.Entity("AqApplication.Entity.Challenge.ChallengeType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasMaxLength(30);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Creator");

                    b.Property<string>("Description")
                        .HasMaxLength(250);

                    b.Property<string>("Editor");

                    b.Property<bool>("IsActive");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<int?>("Seo");

                    b.HasKey("Id");

                    b.HasIndex("Creator");

                    b.HasIndex("Editor");

                    b.ToTable("ChallengeTypes");
                });

            modelBuilder.Entity("AqApplication.Entity.Identity.Data.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<int>("MemberType");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<DateTime>("RegisterDate");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("TelNo");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("AqApplication.Entity.Logging.HostCallLoggingModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Creator");

                    b.Property<string>("Editor");

                    b.Property<bool>("IsActive");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("RequestContentType");

                    b.Property<string>("RequestMethod");

                    b.Property<DateTime?>("RequestTimestamp");

                    b.Property<string>("RequestUri");

                    b.Property<string>("ResponseContentType");

                    b.Property<int>("ResponseStatusCode");

                    b.Property<DateTime?>("ResponseTimestamp");

                    b.Property<int?>("Seo");

                    b.Property<long>("TimeInterval");

                    b.HasKey("Id");

                    b.HasIndex("Creator");

                    b.HasIndex("Editor");

                    b.ToTable("HostCallLogging");
                });

            modelBuilder.Entity("AqApplication.Entity.Question.Class", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Creator");

                    b.Property<string>("Description");

                    b.Property<string>("Editor");

                    b.Property<bool>("IsActive");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int?>("Seo");

                    b.HasKey("Id");

                    b.HasIndex("Creator");

                    b.HasIndex("Editor");

                    b.ToTable("Classes");
                });

            modelBuilder.Entity("AqApplication.Entity.Question.Exam", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Creator");

                    b.Property<string>("Description");

                    b.Property<string>("Editor");

                    b.Property<bool>("IsActive");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int?>("Seo");

                    b.HasKey("Id");

                    b.HasIndex("Creator");

                    b.HasIndex("Editor");

                    b.ToTable("Exams");
                });

            modelBuilder.Entity("AqApplication.Entity.Question.Lecture", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Creator");

                    b.Property<string>("Editor");

                    b.Property<bool>("IsActive");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int?>("Seo");

                    b.HasKey("Id");

                    b.HasIndex("Creator");

                    b.HasIndex("Editor");

                    b.ToTable("Lectures");
                });

            modelBuilder.Entity("AqApplication.Entity.Question.QuestionAnswer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("ImageUrl");

                    b.Property<bool>("IsTrue");

                    b.Property<int>("QuestionId");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("QuestionAnswers");
                });

            modelBuilder.Entity("AqApplication.Entity.Question.QuestionClass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClassId");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Creator");

                    b.Property<string>("Editor");

                    b.Property<bool>("IsActive");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<int>("QuestionId");

                    b.Property<int?>("Seo");

                    b.HasKey("Id");

                    b.HasIndex("ClassId");

                    b.HasIndex("Creator");

                    b.HasIndex("Editor");

                    b.HasIndex("QuestionId");

                    b.ToTable("QuestionClass");
                });

            modelBuilder.Entity("AqApplication.Entity.Question.QuestionExam", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Creator");

                    b.Property<string>("Editor");

                    b.Property<int>("ExamId");

                    b.Property<bool>("IsActive");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<int>("QuestionId");

                    b.Property<int?>("Seo");

                    b.HasKey("Id");

                    b.HasIndex("Creator");

                    b.HasIndex("Editor");

                    b.HasIndex("ExamId");

                    b.HasIndex("QuestionId");

                    b.ToTable("QuestionExams");
                });

            modelBuilder.Entity("AqApplication.Entity.Question.QuestionMain", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AnswerCount");

                    b.Property<int>("CorrectAnswer");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Creator");

                    b.Property<int?>("Difficulty");

                    b.Property<string>("Editor");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("Licence");

                    b.Property<string>("MainImage");

                    b.Property<string>("MainTitle");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<bool?>("Offer");

                    b.Property<int?>("QuestionPdfId");

                    b.Property<int?>("Seo");

                    b.Property<int?>("SubSubjectId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Creator");

                    b.HasIndex("Editor");

                    b.HasIndex("QuestionPdfId");

                    b.HasIndex("SubSubjectId");

                    b.ToTable("QuestionMain");
                });

            modelBuilder.Entity("AqApplication.Entity.Question.QuestionPdf", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Creator");

                    b.Property<string>("Description");

                    b.Property<string>("Editor");

                    b.Property<bool>("IsActive");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("PdfUrl");

                    b.Property<int?>("Seo");

                    b.Property<int>("TotalPage");

                    b.HasKey("Id");

                    b.HasIndex("Creator");

                    b.HasIndex("Editor");

                    b.ToTable("QuestionPdfs");
                });

            modelBuilder.Entity("AqApplication.Entity.Question.QuestionPdfContent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FileName");

                    b.Property<string>("ImageUrl");

                    b.Property<int>("QuestionId");

                    b.Property<int>("Seo");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("QuestionPdfContent");
                });

            modelBuilder.Entity("AqApplication.Entity.Question.SubSubject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Creator");

                    b.Property<string>("Editor");

                    b.Property<bool>("IsActive");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int?>("Seo");

                    b.Property<int>("SubjectId");

                    b.HasKey("Id");

                    b.HasIndex("Creator");

                    b.HasIndex("Editor");

                    b.HasIndex("SubjectId");

                    b.ToTable("SubSubjects");
                });

            modelBuilder.Entity("AqApplication.Entity.Question.Subject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Creator");

                    b.Property<string>("Editor");

                    b.Property<bool>("IsActive");

                    b.Property<int>("LectureId");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int?>("Seo");

                    b.HasKey("Id");

                    b.HasIndex("Creator");

                    b.HasIndex("Editor");

                    b.HasIndex("LectureId");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("AqApplication.Entity.Challenge.Challenge", b =>
                {
                    b.HasOne("AqApplication.Entity.Challenge.ChallengeType", "ChallengeType")
                        .WithMany()
                        .HasForeignKey("ChallengeTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AqApplication.Entity.Identity.Data.ApplicationUser", "AppUserCreator")
                        .WithMany()
                        .HasForeignKey("Creator");

                    b.HasOne("AqApplication.Entity.Identity.Data.ApplicationUser", "AppUserEditor")
                        .WithMany()
                        .HasForeignKey("Editor");
                });

            modelBuilder.Entity("AqApplication.Entity.Challenge.ChallengeQuestionAnswers", b =>
                {
                    b.HasOne("AqApplication.Entity.Challenge.Challenge", "Challenge")
                        .WithMany()
                        .HasForeignKey("ChallengeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AqApplication.Entity.Identity.Data.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("AqApplication.Entity.Challenge.ChallengeQuestions", b =>
                {
                    b.HasOne("AqApplication.Entity.Challenge.Challenge", "Challenge")
                        .WithMany("ChallengeQuestions")
                        .HasForeignKey("ChallengeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AqApplication.Entity.Question.QuestionMain", "QuestionMain")
                        .WithMany()
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AqApplication.Entity.Challenge.ChallengeSession", b =>
                {
                    b.HasOne("AqApplication.Entity.Challenge.Challenge", "Challenge")
                        .WithMany("ChallengeSessions")
                        .HasForeignKey("ChallengeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AqApplication.Entity.Identity.Data.ApplicationUser", "AppUserCreator")
                        .WithMany()
                        .HasForeignKey("Creator");

                    b.HasOne("AqApplication.Entity.Identity.Data.ApplicationUser", "AppUserEditor")
                        .WithMany()
                        .HasForeignKey("Editor");

                    b.HasOne("AqApplication.Entity.Identity.Data.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("AqApplication.Entity.Challenge.ChallengeType", b =>
                {
                    b.HasOne("AqApplication.Entity.Identity.Data.ApplicationUser", "AppUserCreator")
                        .WithMany()
                        .HasForeignKey("Creator");

                    b.HasOne("AqApplication.Entity.Identity.Data.ApplicationUser", "AppUserEditor")
                        .WithMany()
                        .HasForeignKey("Editor");
                });

            modelBuilder.Entity("AqApplication.Entity.Logging.HostCallLoggingModel", b =>
                {
                    b.HasOne("AqApplication.Entity.Identity.Data.ApplicationUser", "AppUserCreator")
                        .WithMany()
                        .HasForeignKey("Creator");

                    b.HasOne("AqApplication.Entity.Identity.Data.ApplicationUser", "AppUserEditor")
                        .WithMany()
                        .HasForeignKey("Editor");
                });

            modelBuilder.Entity("AqApplication.Entity.Question.Class", b =>
                {
                    b.HasOne("AqApplication.Entity.Identity.Data.ApplicationUser", "AppUserCreator")
                        .WithMany()
                        .HasForeignKey("Creator");

                    b.HasOne("AqApplication.Entity.Identity.Data.ApplicationUser", "AppUserEditor")
                        .WithMany()
                        .HasForeignKey("Editor");
                });

            modelBuilder.Entity("AqApplication.Entity.Question.Exam", b =>
                {
                    b.HasOne("AqApplication.Entity.Identity.Data.ApplicationUser", "AppUserCreator")
                        .WithMany()
                        .HasForeignKey("Creator");

                    b.HasOne("AqApplication.Entity.Identity.Data.ApplicationUser", "AppUserEditor")
                        .WithMany()
                        .HasForeignKey("Editor");
                });

            modelBuilder.Entity("AqApplication.Entity.Question.Lecture", b =>
                {
                    b.HasOne("AqApplication.Entity.Identity.Data.ApplicationUser", "AppUserCreator")
                        .WithMany()
                        .HasForeignKey("Creator");

                    b.HasOne("AqApplication.Entity.Identity.Data.ApplicationUser", "AppUserEditor")
                        .WithMany()
                        .HasForeignKey("Editor");
                });

            modelBuilder.Entity("AqApplication.Entity.Question.QuestionAnswer", b =>
                {
                    b.HasOne("AqApplication.Entity.Question.QuestionMain", "Question")
                        .WithMany("QuestionAnswers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AqApplication.Entity.Question.QuestionClass", b =>
                {
                    b.HasOne("AqApplication.Entity.Question.Class", "Class")
                        .WithMany()
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AqApplication.Entity.Identity.Data.ApplicationUser", "AppUserCreator")
                        .WithMany()
                        .HasForeignKey("Creator");

                    b.HasOne("AqApplication.Entity.Identity.Data.ApplicationUser", "AppUserEditor")
                        .WithMany()
                        .HasForeignKey("Editor");

                    b.HasOne("AqApplication.Entity.Question.QuestionMain", "Question")
                        .WithMany("QuestionClasses")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AqApplication.Entity.Question.QuestionExam", b =>
                {
                    b.HasOne("AqApplication.Entity.Identity.Data.ApplicationUser", "AppUserCreator")
                        .WithMany()
                        .HasForeignKey("Creator");

                    b.HasOne("AqApplication.Entity.Identity.Data.ApplicationUser", "AppUserEditor")
                        .WithMany()
                        .HasForeignKey("Editor");

                    b.HasOne("AqApplication.Entity.Question.Exam", "Exam")
                        .WithMany()
                        .HasForeignKey("ExamId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AqApplication.Entity.Question.QuestionMain", "Question")
                        .WithMany("QuestionExams")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AqApplication.Entity.Question.QuestionMain", b =>
                {
                    b.HasOne("AqApplication.Entity.Identity.Data.ApplicationUser", "AppUserCreator")
                        .WithMany()
                        .HasForeignKey("Creator");

                    b.HasOne("AqApplication.Entity.Identity.Data.ApplicationUser", "AppUserEditor")
                        .WithMany()
                        .HasForeignKey("Editor");

                    b.HasOne("AqApplication.Entity.Question.QuestionPdf", "QuestionPdf")
                        .WithMany()
                        .HasForeignKey("QuestionPdfId");

                    b.HasOne("AqApplication.Entity.Question.SubSubject", "SubSubject")
                        .WithMany()
                        .HasForeignKey("SubSubjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AqApplication.Entity.Question.QuestionPdf", b =>
                {
                    b.HasOne("AqApplication.Entity.Identity.Data.ApplicationUser", "AppUserCreator")
                        .WithMany()
                        .HasForeignKey("Creator");

                    b.HasOne("AqApplication.Entity.Identity.Data.ApplicationUser", "AppUserEditor")
                        .WithMany()
                        .HasForeignKey("Editor");
                });

            modelBuilder.Entity("AqApplication.Entity.Question.QuestionPdfContent", b =>
                {
                    b.HasOne("AqApplication.Entity.Question.QuestionPdf", "QuestionPdf")
                        .WithMany("QuestionPdfContent")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AqApplication.Entity.Question.SubSubject", b =>
                {
                    b.HasOne("AqApplication.Entity.Identity.Data.ApplicationUser", "AppUserCreator")
                        .WithMany()
                        .HasForeignKey("Creator");

                    b.HasOne("AqApplication.Entity.Identity.Data.ApplicationUser", "AppUserEditor")
                        .WithMany()
                        .HasForeignKey("Editor");

                    b.HasOne("AqApplication.Entity.Question.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AqApplication.Entity.Question.Subject", b =>
                {
                    b.HasOne("AqApplication.Entity.Identity.Data.ApplicationUser", "AppUserCreator")
                        .WithMany()
                        .HasForeignKey("Creator");

                    b.HasOne("AqApplication.Entity.Identity.Data.ApplicationUser", "AppUserEditor")
                        .WithMany()
                        .HasForeignKey("Editor");

                    b.HasOne("AqApplication.Entity.Question.Lecture", "Lecture")
                        .WithMany()
                        .HasForeignKey("LectureId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("AqApplication.Entity.Identity.Data.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("AqApplication.Entity.Identity.Data.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AqApplication.Entity.Identity.Data.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("AqApplication.Entity.Identity.Data.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
