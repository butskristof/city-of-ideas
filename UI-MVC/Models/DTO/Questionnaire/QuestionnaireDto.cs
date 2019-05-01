using System.Collections.Generic;

namespace COI.UI.MVC.Models.DTO.Questionnaire
{
	public class QuestionnaireDto
	{
		public int QuestionnaireId { get; set; }
		
		public string Title { get; set; }
		public string Description { get; set; }

		public int ProjectPhaseId { get; set; }

		public List<QuestionDto> Questions { get; set; }
	}
}