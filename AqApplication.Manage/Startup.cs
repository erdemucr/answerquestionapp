using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AqApplication.Entity.Identity.Data;
using AqApplication.Repository.Question;
using AqApplication.Repository.Challenge;
using AqApplication.Repository.File;
using AqApplication.Repository.Session;
using Hangfire;
using Hangfire.SqlServer;
using AnswerQuestionApp.Manage.HangFire;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Server.IISIntegration;
using AnswerQuestionApp.Repository.Configuration;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using AnswerQuestionApp.Repository.Advisor;
using AnswerQuestionApp.Repository.Lang;
using Microsoft.AspNetCore.Mvc.Razor;
using AnswerQuestionApp.Manage.Utilities;
using Microsoft.Extensions.Localization;

namespace AqApplication.Manage
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddEntityFrameworkSqlServer()
             .AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("IndentityContext")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            services.AddScoped<DbContext, ApplicationDbContext>();
            services.AddScoped<UserManager<ApplicationUser>, UserManager<ApplicationUser>>();
            services.AddScoped<SignInManager<ApplicationUser>, SignInManager<ApplicationUser>>();
            services.AddScoped<IQuestion, QuestionRepo>();
            services.AddScoped<IChallenge, ChallengeRepo>();
            services.AddScoped<IFile, FileRepo>();
            services.AddScoped<IUser, UserRepo>();
            services.AddScoped<IAdvisor, AdvisorRepo>();
            services.AddScoped<ILang, LangRepo>();
            services.AddScoped<ChallengeCron>();
            services.AddScoped<IConfigurationValues, ConfigurationValuesRepo>();
            services.AddScoped<AnswerQuestionApp.Repository.Mail.IEmailSender, AnswerQuestionApp.Repository.Mail.EmailSender>();

            services.AddTransient<SharedViewLocalizer>();
            //services.AddSingleton<IHostingEnvironment>(new HostingEnvironment());// File I/0 enviroment etc.
            services.AddLocalization(o =>
            {
                // Çevirilerin yer alacağı klasörü belirtiyoruz.
                o.ResourcesPath = "Resources";
            });

            services.AddHangfire(configuration => configuration
         .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
         .UseSimpleAssemblyNameTypeSerializer()
         .UseRecommendedSerializerSettings()
         .UseSqlServerStorage(Configuration.GetConnectionString("HangfireDbConn"), new SqlServerStorageOptions
         {
             CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
             SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
             QueuePollInterval = TimeSpan.Zero,
             UseRecommendedIsolationLevel = true,
             UsePageLocksOnDequeue = true,
             DisableGlobalLocks = true
         }));

            services.AddMemoryCache();

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

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(180);
                options.LoginPath = "/Account/Login";
                options.SlidingExpiration = true;
            });
            services.AddSession(opt =>
            {
                opt.Cookie.IsEssential = true;
            });



            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddSessionStateTempDataProvider()
               .AddDataAnnotationsLocalization(options =>
            options.DataAnnotationLocalizerProvider = (t, f) => f.Create(typeof(AnswerQuestionApp.Manage.SharedResource)));


            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = true;
            });

            services.AddAuthentication(IISDefaults.AuthenticationScheme);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
    IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseSession();

            var supportedCultures = new[] { new CultureInfo("tr-TR"), new CultureInfo("en-US") };

            app.UseRequestLocalization(options =>
            {

                options.DefaultRequestCulture = new RequestCulture(culture: "tr-TR", uiCulture: "tr-TR");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                //var defaultCookieRequestProvider =
                //    options.RequestCultureProviders.FirstOrDefault(rcp =>
                //        rcp.GetType() == typeof(CookieRequestCultureProvider));
                //if (defaultCookieRequestProvider != null)
                //    options.RequestCultureProviders.Remove(defaultCookieRequestProvider);

                //options.RequestCultureProviders.Insert(0,
                //    new CookieRequestCultureProvider()
                //    {
                //        CookieName = ".AspNetCore.Culture",
                //        Options = options
                //    });
            });

            GlobalConfiguration.Configuration
        .UseActivator(new HangfireActivator(serviceProvider));

            app.UseHangfireDashboard();
            app.UseHangfireServer();

            RecurringJob.AddOrUpdate(() => serviceProvider.GetRequiredService<IChallenge>().CreateChallenge("11efabde-f29e-4240-aa5b-995d07169ced", (int?)null, Entity.Constants.ChallengeTypeEnum.RandomMode), "*/1 * * * *");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
