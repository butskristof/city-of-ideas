using System;
using System.IO;
using COI.BL.Domain.Ideation;
using COI.BL.Domain.Organisation;
using COI.BL.Domain.Platform;
using COI.BL.Domain.Project;
using COI.BL.Domain.Questionnaire;
using COI.BL.Domain.Relations;
using COI.BL.Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

namespace COI.DAL.EF
{
	public class CityOfIdeasDbContext : IdentityDbContext<BL.Domain.User.User>
	{
		public CityOfIdeasDbContext()
		{
		}

		public CityOfIdeasDbContext(DbContextOptions options) : base(options)
		{
		}

		private bool _delaySave;

		public void StartUnitOfWork()
		{
			this._delaySave = true;
		}

		public void EndUnitOfWork()
		{
			this.CommitChanges();
			this._delaySave = false;
		}

		#region Entities

		public DbSet<Platform> Platforms { get; set; }
		
		public DbSet<BL.Domain.Organisation.Organisation> Organisations { get; set; }

		public DbSet<BL.Domain.Project.Project> Projects { get; set; }
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
		
		#endregion

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder
					.UseSqlite("Data Source=../db/CityOfIdeas.db")
					.UseLazyLoadingProxies();
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