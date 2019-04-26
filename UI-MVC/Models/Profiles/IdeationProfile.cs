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
				.ForMember(c => c.VoteCount, opt => opt.MapFrom(o => o.GetScore()));
		}
	}

	public class IdeaProfile : Profile
	{
		public IdeaProfile()
		{
			CreateMap<Idea, IdeaDto>()
				.ForMember(c => c.VoteCount,
					opt => opt.MapFrom(
						o => o.GetScore()));
		}
	}
}