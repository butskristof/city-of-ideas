using System.Collections.Generic;
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
				.ForMember(o => o.ProjectState, opt => opt.MapFrom(s => s.State.ToString()));
			// todo check whether phases get added correctly
		}
	}

	public class ProjectPhaseProfile : Profile
	{
		public ProjectPhaseProfile()
		{
			CreateMap<ProjectPhase, ProjectPhaseDto>()
				.ForMember(o => o.State, opt => opt.MapFrom(s => s.State.ToString()));
		}
	}
}