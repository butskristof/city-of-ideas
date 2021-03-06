using System.Collections.Generic;
using COI.BL.Domain.Project;

namespace COI.DAL.Project
{
	public interface IProjectRepository
	{
		IEnumerable<BL.Domain.Project.Project> ReadProjectsForOrganisation(string organisation);
		IEnumerable<BL.Domain.Project.Project> ReadLastNProjects(string organisation, int numberOfProjectsToGet);
		IEnumerable<BL.Domain.Project.Project> ReadLastNProjects(string organisation, int numberOfProjectsToGet, ProjectState state);
		BL.Domain.Project.Project ReadLastProjectWithState(ProjectState state);
		BL.Domain.Project.Project ReadProject(int id);
		IEnumerable<ProjectPhase> ReadPhasesForProject(int projectId);
		BL.Domain.Project.Project CreateProject(BL.Domain.Project.Project project);
		BL.Domain.Project.Project UpdateProject(BL.Domain.Project.Project project);
		BL.Domain.Project.Project DeleteProject(int id);
	}
}