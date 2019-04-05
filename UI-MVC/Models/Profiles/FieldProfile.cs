using AutoMapper;
using COI.BL.Domain.Ideation;
using COI.UI.MVC.Models.DTO.Ideation;

namespace COI.UI.MVC.Models.Profiles
{
	public class FieldProfile : Profile
	{
		public FieldProfile()
		{
			CreateMap<FieldDto, Field>();
		}
	}
}