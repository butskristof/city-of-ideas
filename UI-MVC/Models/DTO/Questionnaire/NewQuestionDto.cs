using COI.BL.Domain.Questionnaire;

namespace COI.UI.MVC.Models.DTO.Questionnaire
{
	public class NewQuestionDto
	{
		public string Inquiry { get; set; }
		public QuestionType QuestionType { get; set; }
		public int QuestionnaireId { get; set; }
	}
}