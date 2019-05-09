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
				.ForMember(c => c.VoteCount, opt => opt.MapFrom(o => o.GetScore()));

			CreateMap<Field, FieldDto>()
				.ForMember(f => f.Content, opt => opt.MapFrom(s => s.Content))
				.ForMember(f => f.IdeaId, 
					opt => opt.MapFrom(
						s => s.Idea.IdeaId))
				.ForMember(f => f.IdeationId, 
					opt => opt.MapFrom(
						s => s.Ideation.IdeationId))
				.ForMember(f => f.CommentId, 
					opt => opt.MapFrom(
						s => s.Comment.CommentId));
		}
	}
}