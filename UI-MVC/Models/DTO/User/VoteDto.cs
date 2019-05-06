namespace COI.UI.MVC.Models.DTO.User
{
	public class VoteDto
	{
		public int VoteId { get; set; }
		public int Value { get; set; }
		public string UserId { get; set; }
		public int? IdeationId { get; set; }
		public int? IdeaId { get; set; }
		public int? CommentId { get; set; }
	}
}