using AutoMapper;
using COI.BL.Domain.Ideation;
using COI.UI.MVC.Models.DTO.Ideation;

namespace COI.UI.MVC.Models.Profiles
{
	public class CommentProfile : Profile
	{
		public CommentProfile()
		{
			CreateMap<Comment, CommentDto>()
				.ForMember(p => p.IdeaId,
					opt => opt.MapFrom(
						p => p.Idea.IdeaId))
				.ForMember(c => c.Score, opt => opt.MapFrom(o => o.GetScore()));

			CreateMap<Field, FieldDto>()
				.ForMember(f => f.Content, opt => opt.MapFrom(s => s.Content));
			
		}
	}
}