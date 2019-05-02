using System.Collections.Generic;

namespace COI.UI.MVC.Models.DTO.Ideation
{
	public class NewIdeaDto
	{
		public string Title { get; set; }
		public List<FieldDto> Fields { get; set; }
		public int IdeationId { get; set; }
		// TODO add user createdby
	}
}