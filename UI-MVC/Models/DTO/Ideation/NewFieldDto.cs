using COI.BL.Domain.Ideation;

namespace COI.UI.MVC.Models.DTO.Ideation
{
	public class NewFieldDto
	{
		public string Content { get; set; }
		public FieldType FieldType { get; set; }

		// values will be checked for HasValue and != 0 in this order 
		public int? IdeaId { get; set; }
		public int? IdeationId { get; set; }
		public int? CommentId { get; set; }
		public int? ProjectId { get; set; }
	}
}