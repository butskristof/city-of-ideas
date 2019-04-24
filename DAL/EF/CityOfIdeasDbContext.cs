using System;
using System.IO;
using COI.BL.Domain.Ideation;
using COI.BL.Domain.Organisation;
using COI.BL.Domain.Platform;
using COI.BL.Domain.Project;
using COI.BL.Domain.Questionnaire;
using COI.BL.Domain.Relations;
using COI.BL.Domain.User;
using Microsoft.EntityFrameworkCore;
namespace COI.DAL.EF
{
	public class CityOfIdeasDbContext : DbContext
	{
		private bool _delaySave;

		public CityOfIdeasDbContext()
		{
			this.Initialise();
		}

		public CityOfIdeasDbContext(DbContextOptions options, bool addTestData = true)
			: base(options)
		{
			this.Initialise(addTestData);
		}
		// TODO extract private method for init and call from both ctors

		private void Initialise(bool addTestData = true)
		{
			CityOfIdeasInitializer.Initialize(this, true, addTestData);
		}

		public void StartUnitOfWork()
		{
			this._delaySave = true;
		}

		public void EndUnitOfWork()
		{
			this.CommitChanges();
			this._delaySave = false;
		}

		public DbSet<Platform> Platforms { get; set; }
		
		public DbSet<Organisation> Organisations { get; set; }

		public DbSet<Project> Projects { get; set; }
		public DbSet<ProjectPhase> ProjectPhases { get; set; }

		public DbSet<BL.Domain.Ideation.Ideation> Ideations { get; set; }
		public DbSet<Idea> Ideas { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<Field> Fields { get; set; }
		
		public DbSet<BL.Domain.Questionnaire.Questionnaire> Questionnaires { get; set; }
		public DbSet<Question> Questions { get; set; }
		public DbSet<Option> Options { get; set; }
		public DbSet<Answer> Answers { get; set; }
		
		public DbSet<BL.Domain.User.User> Users { get; set; }
		public DbSet<Vote> Votes { get; set; }
		public DbSet<Share> Shares { get; set; }
		public DbSet<Flag> Flags { get; set; }
		
		public DbSet<OrganisationUser> OrganisationUsers { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder
					.UseMySQL("server=34.76.147.236;database=city-of-ideas-db;user=emre;password=emre")
					.UseLazyLoadingProxies();
//					.UseLoggerFactory(new LoggerFactory(
//						new[]
//						{
//							new DebugLoggerProvider(
//								(category, level) => category == DbLoggerCategory.Database.Command.Name
//													 && level == LogLevel.Information
//							)
//						}
//					));
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			
			modelBuilder.Entity<OrganisationUser>().Property<int>("OrganisationId");
			modelBuilder.Entity<OrganisationUser>().Property<int>("UserId");
			modelBuilder.Entity<OrganisationUser>().HasKey("OrganisationId", "UserId");
		}

		public override int SaveChanges()
		{
			if (_delaySave) return -1; // function normally returns number of rows modified
			return base.SaveChanges();
		}

		private int CommitChanges()
		{
			if (_delaySave)
			{
				return base.SaveChanges();
			}
			throw new InvalidOperationException("No Unit of Work present, use SaveChanges instead.");
		}
	}
}