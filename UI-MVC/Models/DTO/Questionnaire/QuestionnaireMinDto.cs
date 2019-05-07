namespace COI.UI.MVC.Models.DTO.Questionnaire
{
	public class QuestionnaireMinDto
	{
		public int QuestionnaireId { get; set; }
		
		public string Title { get; set; }
		public string Description { get; set; }

		public int ProjectPhaseId { get; set; }
	}
}