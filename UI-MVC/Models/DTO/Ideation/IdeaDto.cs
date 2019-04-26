using System.Collections.Generic;

namespace COI.UI.MVC.Models.DTO.Ideation
{
	public class IdeaDto
	{
		public int IdeaId { get; set; }
		public string Title { get; set; }

		public List<FieldDto> Fields { get; set; }

		public int VoteCount { get; set; }
	}
}