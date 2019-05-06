namespace COI.UI.MVC.Models.DTO.Ideation
{
	public class NewVoteDto
	{
		public string UserId { get; set; }
		public int Value { get; set; }
		public int? IdeaId { get; set; }
		public int? IdeationId { get; set; }
		public int? CommentId { get; set; }
	}
}