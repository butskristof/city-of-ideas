using AutoMapper;
using COI.BL.Domain.User;
using COI.UI.MVC.Models.DTO.User;

namespace COI.UI.MVC.Models.Profiles
{
	public class UserProfile : Profile
	{
		public UserProfile()
		{
			CreateMap<User, UserDto>()
				.ForMember(u => u.UserId,
					opt => opt.MapFrom(
						m => m.Id));

			CreateMap<Vote, VoteDto>()
				.ForMember(v => v.IdeationId,
					opt => opt.MapFrom(
						m => m.Ideation.IdeationId))
				.ForMember(v => v.IdeaId,
					opt => opt.MapFrom(
						m => m.Idea.IdeaId))
				.ForMember(v => v.CommentId,
					opt => opt.MapFrom(
						m => m.Comment.CommentId));
		}
	}
}