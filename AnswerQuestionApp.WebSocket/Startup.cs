using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnswerQuestionApp.Repository.Advisor;
using AnswerQuestionApp.Repository.Configuration;
using AnswerQuestionApp.Repository.Lang;
using AnswerQuestionApp.Repository.Mail;
using AnswerQuestionApp.Repository.Report;
using AnswerQuestionApp.WebSocket.Methods;
using AqApplication.Entity.Identity.Data;
using AqApplication.Repository.Challenge;
using AqApplication.Repository.File;
using AqApplication.Repository.Question;
using AqApplication.Repository.Session;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AnswerQuestionApp.WebSocket
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public object CompatibilityVersion { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkSqlServer()
            .AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(Configuration.GetConnectionString("IndentityContext")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.AddScoped<DbContext, ApplicationDbContext>();
            services.AddScoped<UserManager<ApplicationUser>, UserManager<ApplicationUser>>();
            services.AddScoped<IQuestion, QuestionRepo>();
            services.AddScoped<IChallenge, ChallengeRepo>();
            services.AddScoped<IFile, FileRepo>();
            services.AddScoped<IUser, UserRepo>();
            services.AddScoped<IReport, ReportRepo>();
            services.AddScoped<IAdvisor, AdvisorRepo>();
            services.AddScoped<ILang, LangRepo>();
            services.AddScoped<IConfigurationValues, ConfigurationValuesRepo>();
            services.AddScoped<AnswerQuestionApp.Repository.Messages.IMessage, AnswerQuestionApp.Repository.Messages.MessageRepo>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddMemoryCache();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    });
            });

            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseWebSockets(); //-> UseDotNetify socket development
            app.Use(async (ctx, nextMsg) =>
            {
                Console.WriteLine("Web Socket is listening");
                if (ctx.Request.Path == "/challenge")
                {
                    if (ctx.WebSockets.IsWebSocketRequest)
                    {
                        var wSocket = await ctx.WebSockets.AcceptWebSocketAsync();
                        await new ChallengeOperations().ChallengeStart(ctx, wSocket);
                    }
                    else
                    {
                        ctx.Response.StatusCode = 400;
                    }
                }
                else if (ctx.Request.Path == "/result")
                {
                    if (ctx.WebSockets.IsWebSocketRequest)
                    {
                        var wSocket = await ctx.WebSockets.AcceptWebSocketAsync();
                        await new ChallengeOperations().ChallengeEnd(ctx, wSocket);
                    }
                    else
                    {
                        ctx.Response.StatusCode = 400;
                    }
                }
                else if (ctx.Request.Path == "/messaging")
                {
                    if (ctx.WebSockets.IsWebSocketRequest)
                    {
                        var wSocket = await ctx.WebSockets.AcceptWebSocketAsync();
                        await new MessagingOperations().SendMessage(ctx, wSocket);
                    }
                    else
                    {
                        ctx.Response.StatusCode = 400;
                    }
                }
                else
                {
                    await nextMsg();
                }
            });

            app.UseCors("AllowAll");

            app.UseMvc();
        }
    }
}
