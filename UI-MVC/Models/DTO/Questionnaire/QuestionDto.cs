using System.Collections.Generic;
using COI.BL.Domain.Questionnaire;

namespace COI.UI.MVC.Models.DTO.Questionnaire
{
	public class QuestionDto
	{
		public int QuestionId { get; set; }
		public string Inquiry { get; set; }
		
		public QuestionType QuestionType { get; set; }
		
		public List<AnswerDto> Answers { get; set; }
		public List<OptionDto> Options { get; set; }

		public int QuestionnaireId { get; set; }
	}
}