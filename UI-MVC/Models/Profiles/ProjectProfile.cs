using AutoMapper;
using COI.BL.Domain.Project;
using COI.UI.MVC.Models.DTO.Project;

namespace COI.UI.MVC.Models.Profiles
{
	public class ProjectProfile : Profile
	{
		public ProjectProfile()
		{
			CreateMap<Project, ProjectDto>()
				.ForMember(p => p.OrganisationId,
					opt => opt.MapFrom(
						p => p.Organisation.OrganisationId));
			
			CreateMap<Project, ProjectMinDto>()
				.ForMember(p => p.OrganisationId,
					opt => opt.MapFrom(
						p => p.Organisation.OrganisationId));
		}
	}

	public class ProjectPhaseProfile : Profile
	{
		public ProjectPhaseProfile()
		{
			CreateMap<ProjectPhase, ProjectPhaseDto>()
				.ForMember(p => p.ProjectId,
					opt => opt.MapFrom(
						p => p.Project.ProjectId));
			
			CreateMap<ProjectPhase, ProjectPhaseMinDto>()
				.ForMember(p => p.ProjectId,
					opt => opt.MapFrom(
						p => p.Project.ProjectId));
		}
	}
}