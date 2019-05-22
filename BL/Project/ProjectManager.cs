using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using COI.BL.Domain.Project;
using COI.BL.Organisation;
using COI.DAL.Project;

namespace COI.BL.Project
{
	public class ProjectManager : IProjectManager
	{
		private readonly IProjectRepository _projectRepository;
		private readonly IProjectPhaseRepository _projectPhaseRepository;
		private readonly IOrganisationManager _organisationManager;

		public ProjectManager(IProjectRepository projectRepository, IProjectPhaseRepository projectPhaseRepository, IOrganisationManager organisationManager)
		{
			_projectRepository = projectRepository;
			_projectPhaseRepository = projectPhaseRepository;
			_organisationManager = organisationManager;
		}

		#region Project

		public IEnumerable<Domain.Project.Project> GetProjects(string organisation)
		{
            return _projectRepository.ReadProjectsForOrganisation(organisation);
		}

		public IEnumerable<Domain.Project.Project> GetLastNProjects(string organisation, int numberOfProjectsToGet)
		{
            return _projectRepository.ReadLastNProjects(organisation, numberOfProjectsToGet);
		}

		public IEnumerable<Domain.Project.Project> GetLastNProjects(string organisation, int numberOfProjectsToGet, ProjectState state)
		{
            return _projectRepository.ReadLastNProjects(organisation, numberOfProjectsToGet, state);
		}

		public Domain.Project.Project GetProject(int id)
		{
			return _projectRepository.ReadProject(id);
		}

		public IEnumerable<ProjectPhase> GetPhasesForProject(int projectId)
		{
			return _projectRepository.ReadPhasesForProject(projectId);
		}

		public Domain.Project.Project AddProject(string title, DateTime start, DateTime end, int organisationId)
		{
			Domain.Organisation.Organisation org = _organisationManager.GetOrganisation(organisationId);

			if (org == null)
			{
				throw new ArgumentException("Organisation not found.");
			}
			
			Domain.Project.Project newProject = new Domain.Project.Project()
			{
				Title = title,
				StartDate = start,
				EndDate = end,
				Organisation = org
			};

			return AddProject(newProject);
		}

		private Domain.Project.Project AddProject(Domain.Project.Project p)
		{
			Validate(p);
			return _projectRepository.CreateProject(p);
		}

		public Domain.Project.Project ChangeProject(int id, string title, DateTime start, DateTime end, int organisationId)
		{
			Domain.Organisation.Organisation org = _organisationManager.GetOrganisation(organisationId);

			if (org == null)
			{
				throw new ArgumentException("Organisation not found.", "organisationId");
			}
			
			Domain.Project.Project p = _projectRepository.ReadProject(id);
			if (p != null)
			{
				p.Title = title;
				p.StartDate = start;
				p.EndDate = end;
				p.Organisation = org;

				Validate(p);
				return _projectRepository.UpdateProject(p);
			}
			
			throw new ArgumentException("Project not found.", "id");
		}

		public Domain.Project.Project RemoveProject(int id)
		{
			return _projectRepository.DeleteProject(id);
		}

		/**
		 * Helper method to validate the object we want to persist against the validation annotations.
		 * Will throw a ValidationException upon failing.
		 */
		private void Validate(Domain.Project.Project project)
		{
			Validator.ValidateObject(project, new ValidationContext(project), true);
		}
		
		#endregion

		#region ProjectPhases

		public ProjectPhase GetProjectPhase(int projectPhaseId)
		{
			return _projectPhaseRepository.ReadProjectPhase(projectPhaseId);
		}

		public ProjectPhase AddProjectPhase(string title, string description, DateTime start, DateTime end, int projectId)
		{
			Domain.Project.Project project = GetProject(projectId);
			if (project == null)
			{
				throw new ArgumentException("Project not found.");
			}
			
			ProjectPhase newProjectPhase = new ProjectPhase()
			{
				Title = title,
				Description = description,
				StartDate = start,
				EndDate = end,
				Project = project
			};

			return AddProjectPhase(newProjectPhase);
		}

		private ProjectPhase AddProjectPhase(ProjectPhase phase)
		{
			Validate(phase);
			return _projectPhaseRepository.CreateProjectPhase(phase);
		}

		public ProjectPhase ChangeProjectPhase(int id, string title, string description, DateTime start, DateTime end, int projectId)
		{
			ProjectPhase toChange = GetProjectPhase(id);
			if (toChange != null)
			{
				Domain.Project.Project project = GetProject(projectId);
				if (project == null)
				{
					throw new ArgumentException("Project not found.", "projectId");
				}

				toChange.Title = title;
				toChange.Description = description;
				toChange.StartDate = start;
				toChange.EndDate = end;
				toChange.Project = project;

				Validate(toChange);
				return _projectPhaseRepository.UpdateProjectPhase(toChange);
			}
			
			throw new ArgumentException("Phase not found.", "id");
		}

		public ProjectPhase RemoveProjectPhase(int projectPhaseId)
		{
			return _projectPhaseRepository.DeleteProjectPhase(projectPhaseId);
		}

		/**
		 * Helper method to validate the object we want to persist against the validation annotations.
		 * Will throw a ValidationException upon failing.
		 */
		private void Validate(ProjectPhase phase)
		{
			Validator.ValidateObject(phase, new ValidationContext(phase), true);
		}

		#endregion
	}
}