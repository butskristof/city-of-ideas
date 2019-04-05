using System.Collections.Generic;

namespace COI.UI.MVC.Models.DTO.Ideation
{
	public class NewCommentDto
	{
		public int UserId { get; set; }
		public int IdeaId { get; set; }
		public ICollection<FieldDto> Content { get; set; }
	}
}