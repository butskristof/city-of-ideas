using AutoMapper;
using COI.BL.Domain.Ideation;
using COI.UI.MVC.Models.DTO.Ideation;

namespace COI.UI.MVC.Models.Profiles
{
	public class IdeationProfile : Profile
	{
		public IdeationProfile()
		{
			CreateMap<Ideation, IdeationDto>()
				.ForMember(p => p.ProjectPhaseId,
					opt => opt.MapFrom(
						p => p.ProjectPhase.ProjectPhaseId))
				.ForMember(c => c.VoteCount, 
					opt => opt.MapFrom(
						o => o.GetScore()));
		}
	}

	public class IdeaProfile : Profile
	{
		public IdeaProfile()
		{
			CreateMap<Idea, IdeaDto>()
				.ForMember(p => p.IdeationId,
					opt => opt.MapFrom(
						p => p.Ideation.IdeationId))
				.ForMember(c => c.VoteCount,
					opt => opt.MapFrom(
						o => o.GetScore()));
		}
	}
}