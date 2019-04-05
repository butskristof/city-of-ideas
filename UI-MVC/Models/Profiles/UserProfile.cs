using AutoMapper;
using COI.BL.Domain.User;
using COI.UI.MVC.Models.DTO.User;

namespace COI.UI.MVC.Models.Profiles
{
	public class UserProfile : Profile
	{
		public UserProfile()
		{
			CreateMap<User, UserDto>();
		}
	}
}