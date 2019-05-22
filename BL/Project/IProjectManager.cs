using System;
using System.Collections.Generic;
using COI.BL.Domain.Project;

namespace COI.BL.Project
{
	public interface IProjectManager
	{
		IEnumerable<Domain.Project.Project> GetProjects(int organisationId);
		IEnumerable<Domain.Project.Project> GetLastNProjects(int organisationId, int numberOfProjectsToGet);
		IEnumerable<Domain.Project.Project> GetLastNProjects(int organisationId, int numberOfProjectsToGet, ProjectState state);
//		BL.Domain.Project.Project GetLastProjectWithState(ProjectState state);
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