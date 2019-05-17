using COI.BL.Domain.Ideation;

namespace COI.UI.MVC.Models.DTO.Ideation
{
	public class NewFieldDto
	{
		public string Content { get; set; }
		public FieldType FieldType { get; set; }

		public int? IdeaId { get; set; }
		public int? IdeationId { get; set; }
		public int? CommentId { get; set; }
		public int? ProjectId { get; set; }
	}
}