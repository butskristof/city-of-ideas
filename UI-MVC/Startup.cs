using System.Text;
using AutoMapper;
using COI.BL;
using COI.BL.Application;
using COI.BL.Domain.User;
using COI.BL.Ideation;
using COI.BL.Organisation;
using COI.BL.Project;
using COI.BL.Questionnaire;
using COI.BL.User;
using COI.DAL;
using COI.DAL.EF;
using COI.DAL.Ideation;
using COI.DAL.Ideation.EF;
using COI.DAL.Organisation;
using COI.DAL.Organisation.EF;
using COI.DAL.Project;
using COI.DAL.Project.EF;
using COI.DAL.Questionnaire;
using COI.DAL.Questionnaire.EF;
using COI.DAL.User;
using COI.DAL.User.EF;
using COI.UI.MVC.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace COI.UI.MVC
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});
			
			services.AddAutoMapper(); // for conversion from domain model to DTOs

			// TODO differentiate between dev and production environments
			services.AddDbContext<CityOfIdeasDbContext>(options =>
				options
					.UseSqlite(Configuration["Sqlite:ConnectionString"])
					.UseLazyLoadingProxies()
			);

			#region Authentication setup

			// set identity preferences
			services.Configure<IdentityOptions>(options =>
			{
				options.User.RequireUniqueEmail = true;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
				options.SignIn.RequireConfirmedEmail = true; // comment to disable email confirmation
			});
			
			// add identity framework and set correct dbcontext to use
			services.AddIdentity<User, IdentityRole>()
				.AddEntityFrameworkStores<CityOfIdeasDbContext>()
				.AddDefaultTokenProviders();
			
			services.AddAuthentication()
				.AddCookie(cfg => cfg.SlidingExpiration = true) // still support default identity behaviour
				.AddJwtBearer(cfg => // add JWT for Android app etc
				{
					cfg.TokenValidationParameters = new TokenValidationParameters()
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						ValidIssuer = Configuration["Jwt:Issuer"],
						ValidAudience = Configuration["Jwt:Audience"],
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
					};
				});

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
			
			#endregion

			#region Dependency Injection

			// configure implementations used throughout the project

			#region Repositories

			services.AddScoped<ICommentRepository, CommentRepository>();
			services.AddScoped<IIdeaRepository, IdeaRepository>();
			services.AddScoped<IAnswerRepository, AnswerRepository>();
			services.AddScoped<IQuestionnaireRepository, QuestionnaireRepository>();
			services.AddScoped<IQuestionRepository, QuestionRepository>();
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<IVoteRepository, VoteRepository>();
			services.AddScoped<IIdeationRepository, IdeationRepository>();
			services.AddScoped<IOrganisationRepository, OrganisationRepository>();
			services.AddScoped<IProjectRepository, ProjectRepository>();
			services.AddScoped<IProjectPhaseRepository, ProjectPhaseRepository>();
			services.AddScoped<IOptionRepository, OptionRepository>();

			#endregion

			#region Managers

			// managers
			services.AddScoped<IIdeationManager, IdeationManager>();
			services.AddScoped<IOrganisationManager, OrganisationManager>();
			services.AddScoped<IProjectManager, ProjectManager>();
			services.AddScoped<IQuestionnaireManager, QuestionnaireManager>();
			services.AddScoped<IUserManager, UserManager>();
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped<IUnitOfWorkManager, UnitOfWorkManager>();
			services.AddScoped<ICityOfIdeasController, CityOfIdeasController>();

			#endregion

			#region Others
			
			services.AddTransient<IEmailSender, EmailSender>();

			#endregion

			#endregion
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

			app.UseAuthentication();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
