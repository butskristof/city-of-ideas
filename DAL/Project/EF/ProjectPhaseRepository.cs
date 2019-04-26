using System;
using System.Linq;
using COI.BL.Domain.Project;
using COI.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace COI.DAL.Project.EF
{
	public class ProjectPhaseRepository : EfRepository, IProjectPhaseRepository
	{
		public ProjectPhaseRepository(CityOfIdeasDbContext ctx) : base(ctx)
		{
		}

		public ProjectPhase ReadProjectPhase(int projectPhaseId)
		{
			return _ctx.ProjectPhases.Find(projectPhaseId);
		}

		public ProjectPhase CreateProjectPhase(ProjectPhase projectPhase)
		{
			if (ReadProjectPhase(projectPhase.ProjectPhaseId) != null)
			{
				throw new ArgumentException("Project already in database.");
			}
			
			try
			{
				_ctx.ProjectPhases.Add(projectPhase);
				_ctx.SaveChanges();

				return projectPhase;
			}
			catch (DbUpdateException exception)
			{
				var msg = exception.InnerException == null ? "Invalid object." : exception.InnerException.Message;
				throw new ArgumentException(msg);
			}
		}

		public ProjectPhase UpdateProjectPhase(ProjectPhase projectPhase)
		{
			var entryToUpdate = ReadProjectPhase(projectPhase.ProjectPhaseId);
			
			if (entryToUpdate == null)
			{
				throw new ArgumentException("Project phase to update not found.");
			}
			_ctx.Entry(entryToUpdate).CurrentValues.SetValues(projectPhase);
			_ctx.SaveChanges();

			return ReadProjectPhase(projectPhase.ProjectPhaseId);
		}

		public ProjectPhase DeleteProjectPhase(int projectPhaseId)
		{
			var phaseToDelete = ReadProjectPhase(projectPhaseId);
			if (phaseToDelete == null)
			{
				throw new ArgumentException("Project phase to delete not found.");
			}

			_ctx.ProjectPhases.Remove(phaseToDelete);
			_ctx.SaveChanges();

			return phaseToDelete;
		}
	}
}