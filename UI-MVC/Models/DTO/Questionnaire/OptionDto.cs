using System.Collections.Generic;

namespace COI.UI.MVC.Models.DTO.Questionnaire
{
	public class OptionDto
	{
		public int OptionId { get; set; }
		
		public string Content { get; set; }
		public int AnswerCount { get; set; }

		public int QuestionId { get; set; }
	}
}