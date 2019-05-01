using System.Collections.Generic;
using COI.BL.Domain.Questionnaire;

namespace COI.BL.Questionnaire
{
	public interface IQuestionnaireManager
	{
		IEnumerable<Domain.Questionnaire.Questionnaire> GetQuestionnaires();
		Domain.Questionnaire.Questionnaire GetQuestionnaire(int questionnaireId);
		Domain.Questionnaire.Questionnaire AddQuestionnaire(string title, string description, int projectPhaseId);
		Domain.Questionnaire.Questionnaire ChangeQuestionnaire(int id, string title, string description, int projectPhaseId);
		Domain.Questionnaire.Questionnaire RemoveQuestionnaire(int id);

		IEnumerable<Question> GetQuestionsForQuestionnaire(int questionnaireId);
		Question GetQuestion(int questionId);
		Question AddQuestion(string inquiry, QuestionType type, int questionnaireId);
		Question ChangeQuestion(int id, string inquiry, QuestionType type, int questionnaireId);
		Question RemoveQuestion(int id);

		IEnumerable<Option> GetOptionsForQuestion(int questionId);
		Option GetOption(int optionId);
		Option AddOption(string content, int questionId);
		Option ChangeOption(int id, string content, int questionId);
		Option RemoveOption(int optionId);
		
//		OpenQuestion GetOpenQuestion(int questionId);
		Answer AnswerQuestion(string content, int questionId);
		Answer AnswerOption(int optionId);
//		void AddAnswerToOpenQuestion(int questionId, Answer answer);
//
//		Choice GetChoiceQuestion(int questionId);
//		Option GetOption(int optionId);
//		Answer AnswerChoiceQuestion(int optionId);
//		void AddAnswerToOption(int optionId, Answer answer);
	}
}