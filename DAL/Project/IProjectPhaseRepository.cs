using COI.BL.Domain.Project;

namespace COI.DAL.Project
{
	public interface IProjectPhaseRepository
	{
		ProjectPhase ReadProjectPhase(int projectPhaseId);
		ProjectPhase CreateProjectPhase(ProjectPhase projectPhase);
		ProjectPhase UpdateProjectPhase(ProjectPhase projectPhase);
		ProjectPhase DeleteProjectPhase(int projectPhaseId);
	}
}