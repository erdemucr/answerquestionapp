using AnswerQuestionApp.Manage.HangFire;
using AnswerQuestionApp.Repository.Advisor;
using AnswerQuestionApp.Repository.Configuration;
using AnswerQuestionApp.Repository.Lang;
using AnswerQuestionApp.Repository.Mail;
using AnswerQuestionApp.Repository.Report;
using AnswerQuestionApp.Service.Infrastructure;
using AqApplication.Entity.Identity.Data;
using AqApplication.Logging.Providers;
using AqApplication.Repository.Challenge;
using AqApplication.Repository.File;
using AqApplication.Repository.Question;
using AqApplication.Repository.Session;
using AqApplication.Service.Hubs;
using AqApplication.Service.Models;
using AqApplication.Service.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Text;

namespace AqApplication.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private readonly AqApplication.Repository.Session.IUser _iUser;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkSqlServer()
            .AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("Indenti" +
            "tyContext")));


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
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<AnswerQuestionApp.Repository.Messages.IMessage, AnswerQuestionApp.Repository.Messages.MessageRepo>();
            services.AddMemoryCache();

            services.AddTransient<WebApiHandler>();


            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
                options.User.RequireUniqueEmail = false;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!#$%&'*+-/=?^_`{|}~.@";
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("AnswerQuestionApp", new Info { Title = "My API", Version = "v1" });
            });

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));// fetch the variables of appsettings

            var appSettings = appSettingsSection.Get<AppSettings>();
            var signingKey = Encoding.ASCII.GetBytes(appSettings.Secret);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(signingKey),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;


            })
              .AddJwtBearer(o =>
               {
                   o.RequireHttpsMetadata = false;
                   o.SaveToken = true;
                   o.TokenValidationParameters = tokenValidationParameters;
                   o.Events = new JwtBearerEvents()
                   {
                       OnAuthenticationFailed = c =>
                       {
                           c.NoResult();

                           c.Response.StatusCode = 401;
                           c.Response.ContentType = "text/plain";

                           return c.Response.WriteAsync(c.Exception.ToString());
                       }

                   };
               });


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

            services.AddSignalR(); 

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseSwagger();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AnswerQuestionApp");
            });

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
                else
                {
                    await nextMsg();
                }
            });

            loggerFactory.AddProvider(new LoggingDbProvider());

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            app.UseMvc();
        }
    }

}
