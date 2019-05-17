using System.Collections.Generic;

namespace COI.UI.MVC.Models.DTO.Questionnaire
{
	public class MultipleAnswersDto
	{
		public List<ChoiceAnswerDto> Choices { get; set; }
		public List<OpenQuestionAnswerDto> OpenAnswers { get; set; }
	}
}