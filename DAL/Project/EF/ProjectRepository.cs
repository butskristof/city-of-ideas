using System;
using System.Collections.Generic;
using System.Linq;
using COI.BL.Domain.Project;
using COI.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace COI.DAL.Project.EF
{
	public class ProjectRepository : EfRepository, IProjectRepository
	{
		public ProjectRepository(CityOfIdeasDbContext ctx) : base(ctx)
		{
		}

		public IEnumerable<BL.Domain.Project.Project> ReadProjectsForOrganisation(string organisation)
		{
			return _ctx.Projects
				.Where(p => p.Organisation.Identifier == organisation)
				.AsEnumerable();
		}

		public IEnumerable<BL.Domain.Project.Project> ReadLastNProjects(string organisation, int numberOfProjectsToGet)
		{
			return _ctx.Projects
				.Where(p => p.Organisation.Identifier == organisation)
				.OrderByDescending(p => p.ProjectId)
				.Take(numberOfProjectsToGet)
				.AsEnumerable();
		}

		public IEnumerable<BL.Domain.Project.Project> ReadLastNProjects(string organisation, int numberOfProjectsToGet, ProjectState state)
		{
			return _ctx.Projects
				.Where(p => p.Organisation.Identifier == organisation && p.ProjectState == state)
				.OrderByDescending(p => p.ProjectId)
				.Take(numberOfProjectsToGet)
				.AsEnumerable();
		}

		public BL.Domain.Project.Project ReadLastProjectWithState(ProjectState state)
		{
			return _ctx.Projects
				.LastOrDefault(p => p.ProjectState == state);
		}

		public BL.Domain.Project.Project ReadProject(int id)
		{
			return _ctx.Projects.Find(id);
		}

		public IEnumerable<ProjectPhase> ReadPhasesForProject(int projectId)
		{
			return _ctx
				.ProjectPhases
				.Where(p => p.Project.ProjectId == projectId)
				.AsEnumerable();
		}

		public BL.Domain.Project.Project CreateProject(BL.Domain.Project.Project project)
		{
			if (ReadProject(project.ProjectId) != null)
			{
				throw new ArgumentException("Project already in database.");
			}

			try
			{
				_ctx.Projects.Add(project);
				_ctx.SaveChanges();

				return project;
			}
			catch (DbUpdateException exception)
			{
				var msg = exception.InnerException == null ? "Invalid object." : exception.InnerException.Message;
				throw new ArgumentException(msg);
			}
		}

		public BL.Domain.Project.Project UpdateProject(BL.Domain.Project.Project project)
		{
			var entryToUpdate = ReadProject(project.ProjectId);
			
			if (entryToUpdate == null)
			{
				throw new ArgumentException("Project to update not found.");
			}
			_ctx.Entry(entryToUpdate).CurrentValues.SetValues(project);
			_ctx.SaveChanges();

			return ReadProject(project.ProjectId);
		}

		public BL.Domain.Project.Project DeleteProject(int id)
		{
			var projToDelete = ReadProject(id);
			if (projToDelete == null)
			{
				throw new ArgumentException("Project to delete not found.");
			}

			_ctx.Projects.Remove(projToDelete);
			_ctx.SaveChanges();

			return projToDelete;
		}
	}
}