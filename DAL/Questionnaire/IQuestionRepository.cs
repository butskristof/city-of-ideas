using System.Collections.Generic;
using COI.BL.Domain.Questionnaire;

namespace COI.DAL.Questionnaire
{
	public interface IQuestionRepository
	{
		IEnumerable<Question> ReadQuestions();
		Question ReadQuestion(int questionId);
		Question CreateQuestion(Question question);
		Question UpdateQuestion(Question updatedQuestion);
		Question DeleteQuestion(int questionId);
		
		IEnumerable<Option> ReadOptionsForQuestion(int questionId);
		Option ReadOption(int optionId);
		
		void UpdateOption(Option option);
	}
}