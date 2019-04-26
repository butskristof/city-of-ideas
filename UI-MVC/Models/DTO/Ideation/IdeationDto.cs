namespace COI.UI.MVC.Models.DTO.Ideation
{
	public class IdeationDto
	{
		public int IdeationId { get; set; }
		public string Title { get; set; }
		// TODO add ProjectPhaseDto
//		public int ProjectPhaseId { get; set; }
		public int VoteCount { get; set; }
//		public int ShareCount { get; set; }
	}
}