using System.Collections.Generic;
using COI.BL.Domain.Ideation;
using COI.UI.MVC.Models.DTO.Ideation;

namespace COI.UI.MVC.Models.ViewModels
{
	public class IdeationUserVote
	{
		public Ideation ideation;
		public IEnumerable<IdeaDto> ideas;
	}
}