using System;
using System.Collections.Generic;
using COI.BL.Domain.Project;

namespace COI.BL.Project
{
	public interface IProjectManager
	{
		IEnumerable<Domain.Project.Project> GetProjects(string organisation);
		IEnumerable<Domain.Project.Project> GetLastNProjects(string organisation, int numberOfProjectsToGet);
		IEnumerable<Domain.Project.Project> GetLastNProjects(string organisation, int numberOfProjectsToGet, ProjectState state);
		Domain.Project.Project GetProject(int id);

		IEnumerable<ProjectPhase> GetPhasesForProject(int projectId);

		Domain.Project.Project AddProject(
			string title, DateTime start, DateTime end, int organisationId);

		Domain.Project.Project ChangeProject(
			int id, string title, DateTime start, DateTime end,
			int organisationId);

		Domain.Project.Project RemoveProject(int id);
		
		ProjectPhase GetProjectPhase(int projectPhaseId);
		ProjectPhase AddProjectPhase(string title, string description, DateTime start, DateTime end, int projectId);
		ProjectPhase ChangeProjectPhase(int id, string title, string description, DateTime start, DateTime end, int projectId);
		ProjectPhase RemoveProjectPhase(int projectPhaseId);
	}
}