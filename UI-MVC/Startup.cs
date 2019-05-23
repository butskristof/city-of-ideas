using System;
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
using COI.UI.MVC.Authorization;
using COI.UI.MVC.Helpers;
using COI.UI.MVC.Models;
using COI.UI.MVC.Services;
using Microsoft.AspNetCore.Authorization;
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
using SendGrid;

namespace COI.UI.MVC
{
	public class Startup
	{
		public Startup(IConfiguration configuration, IHostingEnvironment environment)
		{
			Configuration = configuration;
			HostingEnvironment = environment;
		}

		public IConfiguration Configuration { get; }
		public IHostingEnvironment HostingEnvironment { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});
			
			services.AddAutoMapper(); // for conversion from domain model to DTOs

			if (HostingEnvironment.IsProduction())
			{
				// get environment variables from configuration files
				var server = Configuration["prod:SQL_IP"];
				var dbname = Configuration["prod:SQL_DB"];
				var user = Configuration["prod:SQL_USER"];
				var pw = Configuration["prod:SQL_PW"];
				// build connection string
				string connectionString = $"server={server};database={dbname};user={user};password={pw}";
                services.AddDbContext<CityOfIdeasDbContext>(options =>
                    options
	                    .UseMySql(connectionString)
                        .UseLazyLoadingProxies()
                );
			}
			else
			{
                services.AddDbContext<CityOfIdeasDbContext>(options =>
                    options
                        .UseSqlite(Configuration["Sqlite:ConnectionString"])
                        .UseLazyLoadingProxies()
                );
			}

			#region Authentication and authorization setup

			// set identity preferences
			services.Configure<IdentityOptions>(options =>
			{
				options.User.RequireUniqueEmail = true;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
//				options.SignIn.RequireConfirmedEmail = true; // comment to disable email confirmation
			});
			
			// add identity framework and set correct dbcontext to use
			services.AddIdentity<User, IdentityRole>()
				.AddEntityFrameworkStores<CityOfIdeasDbContext>()
				.AddDefaultTokenProviders();
			
			// parameters are read from configuration files, see README
			services.AddAuthentication()
				.AddFacebook(options =>
				{
					options.AppId = Configuration["Facebook:AppId"];
					options.AppSecret = Configuration["Facebook:AppSecret"];
				})
				.AddGoogle(options =>
				{
					options.ClientId = Configuration["Google:ClientId"];
					options.ClientSecret = Configuration["Google:ClientSecret"];
				})
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

			// set policies
			// constants are used to eradicate typos
			services.AddAuthorization(options =>
			{
				options.AddPolicy(AuthConstants.UserInOrgOrSuperadminPolicy, 
					policy => policy.Requirements.Add(new UserInOrgOrSuperadminRequirement())
                );
				options.AddPolicy(AuthConstants.SuperadminPolicy,
					policy => policy.RequireRole(AuthConstants.Superadmin));
				options.AddPolicy(AuthConstants.AdminPolicy, 
					policy => policy.RequireRole(AuthConstants.Superadmin, AuthConstants.Admin));
				options.AddPolicy(AuthConstants.ModeratorPolicy, 
					policy => policy.RequireRole(AuthConstants.Superadmin, AuthConstants.Admin, AuthConstants.Moderator));
			});
			
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
			services.AddScoped<IFieldRepository, FieldRepository>();

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
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<ISeedService, SeedService>();
			services.AddScoped<IFileService, FileService>();
			services.AddScoped<ICommentsHelper, CommentsHelper>();
			services.AddScoped<IIdeasHelper, IdeasHelper>();
			services.AddScoped<IIdeationsHelper, IdeationsHelper>();
			services.AddScoped<IProjectPhasesHelper, ProjectPhasesHelper>();
			services.AddScoped<IOrganisationsHelper, OrganisationsHelper>();
			services.AddScoped<IAuthorizationHandler, UserInOrgHandler>();
			services.AddScoped<IAuthorizationHandler, SuperadminHandler>();
			services.AddScoped<IFieldHelper, FieldHelper>();
			services.AddScoped<IEmailService, EmailService>();

			#endregion

			#endregion
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
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

//			app.UseHttpsRedirection(); // HTTPS is handled by nginx in deployment
			app.UseStaticFiles();
			app.UseCookiePolicy();

			app.UseAuthentication();
			
			app.UseMvc(routes =>
			{
				routes.MapRoute(
					"default",
					"{controller=Home}/{action=Index}/{id?}");
			});
			
		}
	}
}
