using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using COI.BL.Questionnaire;
using COI.DAL.EF;
using COI.DAL.Questionnaire;
using COI.DAL.Questionnaire.EF;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

namespace UI_MVC
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
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            
            // dependency injection
            services.AddScoped<IQuestionnaireManager, QuestionnaireManager>();
            services.AddScoped<IQuestionnaireRepository, QuestionnaireRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IAnswerRepository, AnswerRepository>();
//            services.AddScoped(c =>
//            {
//                var options = new DbContextOptionsBuilder<CityOfIdeasDbContext>()
//					.UseSqlite("Data Source=../db/CityOfIdeas.db")
//					.UseLazyLoadingProxies()
//					.UseLoggerFactory(new LoggerFactory(
//						new[]
//						{
//							new DebugLoggerProvider(
//								(category, level) => category == DbLoggerCategory.Database.Command.Name
//													 && level == LogLevel.Information
//							)
//						}
//					))
//                    .Options;
//                return new CityOfIdeasDbContext(options);
//            });
            services.AddDbContext<CityOfIdeasDbContext>(options =>
	            options
		            .UseSqlite("Data Source=../CityOfIdeas.db")
		            .UseLazyLoadingProxies()
		            .UseLoggerFactory(new LoggerFactory(
			            new[]
			            {
				            new DebugLoggerProvider(
					            (category, level) => category == DbLoggerCategory.Database.Command.Name
					                                 && level == LogLevel.Information
				            )
			            }
		            )));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
