using System.Collections.Generic;

namespace COI.UI.MVC.Models.DTO.Questionnaire
{
	public class MultipleAnswersDto
	{
		public ICollection<ChoiceAnswerDto> Choices { get; set; }
		public ICollection<OpenQuestionAnswerDto> OpenAnswers { get; set; }
	}
}