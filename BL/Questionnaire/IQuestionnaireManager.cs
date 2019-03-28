using System.Collections.Generic;
using COI.BL.Domain.Questionnaire;

namespace COI.BL.Questionnaire
{
	public interface IQuestionnaireManager
	{
		IEnumerable<Domain.Questionnaire.Questionnaire> GetQuestionnaires();
		Domain.Questionnaire.Questionnaire GetQuestionnaire(int questionnaireId);

		OpenQuestion GetOpenQuestion(int questionId);
		Answer AnswerOpenQuestion(int questionId, string content);
		void AddAnswerToOpenQuestion(int questionId, Answer answer);

		Choice GetChoiceQuestion(int questionId);
		Option GetOption(int optionId);
		Answer AnswerChoiceQuestion(int optionId);
		void AddAnswerToOption(int optionId, Answer answer);
	}
}