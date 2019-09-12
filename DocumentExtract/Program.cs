using AnswerQuestionApp.Repository.Configuration;
using AqApplication.Entity.Identity.Data;
using AqApplication.Entity.Question;
using AqApplication.Repository.Challenge;
using AqApplication.Repository.File;
using AqApplication.Repository.Question;
using AqApplication.Repository.Session;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DocumentExtract
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Register IOC
            var collection = new ServiceCollection()
                      .AddEntityFrameworkSqlServer()
             .AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer("Server=192.168.1.10;Initial Catalog=Anqdb;Persist Security Info=False;User ID=aquser;Password=aq123456;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;", null));

            collection
            .AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            collection.AddScoped<DbContext, ApplicationDbContext>()
            .AddScoped<UserManager<ApplicationUser>, UserManager<ApplicationUser>>()
            .AddScoped<SignInManager<ApplicationUser>, SignInManager<ApplicationUser>>()
            .AddScoped<IQuestion, QuestionRepo>()
            .AddScoped<IQuestion, QuestionRepo>()
            .AddScoped<IChallenge, ChallengeRepo>()
            .AddScoped<IFile, FileRepo>()
            .AddScoped<IUser, UserRepo>()
            .AddScoped<IConfigurationValues, ConfigurationValuesRepo>();

            var serviceProvider = collection.BuildServiceProvider();

            #endregion
            int questionCnt = 0;

            try
            {

                var _questionRepo = serviceProvider.GetService<IQuestion>();


                StringBuilder text = new StringBuilder();
                using (PdfReader reader = new PdfReader("C:\\english-grammar-tests.pdf"))
                {
                    for (int i = 316; i <= reader.NumberOfPages; i++) //reader.NumberOfPages 316
                    {
                        text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                    }
                }
                int j = 0;
                string subject = "", diff = "";
                int q = 1;
                char[] trimChars = { '*', '@', ' ', '\n' };


                string[] pdfPagesQuestions = new string[0];

                string[] pdfPagesAnswered = text.ToString().Split("English Grammar /").ToArray();
                int ids = 67;
                foreach (var item in pdfPagesAnswered)
                {
                    j++;
                    Console.WriteLine(j);
                    if (j == 1)
                        continue;

                    int subjectLast = item.IndexOf("/") - 1;
                    int difflast = item.IndexOf("#");
                    subject = item.Substring(1, subjectLast - 1);
                    diff = item.Substring((subjectLast + 3), (difflast - 2) - (subjectLast + 2));


                    string question = "";
                    int start = 0, end = 0, next = 0;

                    string optionA = "", optionB = "", optionC = "", optionD = "";

                    bool option3 = false;
                    bool itemComplted = false;
                    string correctAnswer = "";
                    int correctIndex = 0;
                    while (!itemComplted)
                    {
                        start = item.IndexOf("A" + q);
                        if (start == -1)
                            break;
                        end = item.IndexOf("answer:", start);
                        if (end == -1)
                            break;
                        question = item.Substring(start + 3, (end - (start + 3))).TrimStart(trimChars).TrimEnd(trimChars);

                        next = item.IndexOf("A" + (q + 1));
                        if (next == -1)
                        {
                            itemComplted = true;
                            next = item.IndexOf('©', start);
                            q = 0;
                        }
                        q++;

                        //update
                        correctAnswer = item.Substring(end + 8, 3);

                        switch (correctAnswer)
                        {
                            case "(a)":
                                correctIndex = 0;
                                break;
                            case "(b)":
                                correctIndex = 1;
                                break;
                            case "(c)":
                                correctIndex = 2;
                                break;
                            case "(d)":
                                correctIndex = 3;
                                break;
                        }

                        Console.WriteLine(ids + " :: " + question + " :: " + correctIndex);
                            if(ids>2900)
                      _questionRepo.SetQuestionCorrectAnswer(ids, "", correctIndex, "11efabde-f29e-4240-aa5b-995d07169ced");

                        ids++;

                    }

                }


                Console.ReadKey();



                foreach (var item in pdfPagesQuestions)
                {
                    j++;
                    Console.WriteLine(j);
                    if (j == 1)
                        continue;
                    int subjectLast = item.IndexOf("/") - 1;
                    int difflast = item.IndexOf("#");
                    subject = item.Substring(1, subjectLast - 1);
                    diff = item.Substring((subjectLast + 3), (difflast - 2) - (subjectLast + 2));


                    var addSubject = _questionRepo.AddSubject(new AqApplication.Entity.Question.Subject
                    {
                        CreatedDate = DateTime.Now,
                        Name = subject.Trim(' '),
                        IsActive = true,
                        LectureId = 9,
                    }, "11efabde-f29e-4240-aa5b-995d07169ced");

                    var addDiff = _questionRepo.AddDifficulty(new AqApplication.Entity.Question.Difficulty
                    {
                        CreatedDate = DateTime.Now,
                        Name = diff.Trim(' '),
                        IsActive = true,
                    }, "11efabde-f29e-4240-aa5b-995d07169ced");

                    string question = "";
                    int start = 0, end = 0, next = 0;

                    string optionA = "", optionB = "", optionC = "", optionD = "";

                    bool option3 = false;
                    bool itemComplted = false;

                    while (!itemComplted)
                    {
                        start = item.IndexOf("Q" + q);
                        if (start == -1)
                            break;
                        end = item.IndexOf("(a)", start);
                        if (end == -1)
                            break;
                        question = item.Substring(start + 3, (end - (start + 3))).TrimStart(trimChars).TrimEnd(trimChars);

                        next = item.IndexOf("Q" + (q + 1));
                        if (next == -1)
                        {
                            itemComplted = true;
                            next = item.IndexOf('©', start);
                            q = 0;
                        }
                        q++;

                        start = item.IndexOf("(a)", start, (next - start));
                        end = item.IndexOf("(b)", start, (next - start));

                        optionA = item.Substring(start, (end - start)).Replace("(a)", "").TrimStart(trimChars).TrimEnd(trimChars).ToString();

                        start = item.IndexOf("(b)", start, (next - start));
                        end = item.IndexOf("(c)", start, (next - start));

                        optionB = item.Substring(start, (end - start)).Replace("(b)", "").TrimStart(trimChars).TrimEnd(trimChars).ToString();

                        start = item.IndexOf("(c)", start, (next - start));
                        end = item.IndexOf("(d)", start, (next - start));
                        if (end == -1)
                        {
                            option3 = true;
                            end = next;
                        }
                        else
                        {
                            option3 = false;
                        }

                        optionC = item.Substring(start, (end - start)).Replace("(c)", "").TrimStart(trimChars).TrimEnd(trimChars).ToString();

                        if (!option3)
                        {
                            start = item.IndexOf("(d)", start, (next - start));
                            end = next;
                            optionD = item.Substring(start, (end - start)).Replace("(d)", "").TrimStart(trimChars).TrimEnd(trimChars).ToString();
                        }
                        else
                        {
                            optionD = string.Empty;
                        }

                        Console.WriteLine(subject);
                        Console.WriteLine(diff);
                        Console.WriteLine(question);
                        Console.WriteLine(optionA);
                        Console.WriteLine(optionB);
                        Console.WriteLine(optionC);
                        Console.WriteLine(optionD);
                        #region AddQuestion
                        questionCnt++;

                        var addModel = new QuestionMain
                        {
                            MainTitle = question,
                            SubjectId = addSubject.InstertedId,
                            LectureId = 9,
                            IsActive = false,
                            ModifiedDate = DateTime.Now,
                            CreatedDate = DateTime.Now,
                            Licence = false,
                            Difficulty = addDiff.InstertedId,
                            Editor = "11efabde-f29e-4240-aa5b-995d07169ced",
                            Creator = "11efabde-f29e-4240-aa5b-995d07169ced",
                            Offer = false,
                            Seo = 0,
                            QuestionPdfId = 3043,
                            CorrectAnswer = -1,
                            AnswerCount = option3 ? 3 : 4,
                        };

                        var questionExams = new List<QuestionExam>();



                        var questionAnswers = new List<QuestionAnswer>();



                        questionAnswers.Add(new QuestionAnswer
                        {
                            Title = optionA,
                            IsTrue = false,
                        });
                        questionAnswers.Add(new QuestionAnswer
                        {
                            Title = optionB,
                            IsTrue = false,
                        });
                        questionAnswers.Add(new QuestionAnswer
                        {
                            Title = optionC,
                            IsTrue = false,
                        });
                        if (!option3)
                        {
                            questionAnswers.Add(new QuestionAnswer
                            {
                                Title = optionD,
                                IsTrue = false,
                            });
                        }
                        addModel.QuestionExams = questionExams.Any() ? questionExams : null;
                        addModel.QuestionAnswers = questionAnswers.Any() ? questionAnswers : null;

                        var result = _questionRepo.SaveQuestion(addModel);
                    }


                    #endregion



                }

                q = 1;
            }
            catch (Exception ex)
            {

            }
            Console.WriteLine("count " + questionCnt);
            Console.ReadKey();

        }
    }
}
