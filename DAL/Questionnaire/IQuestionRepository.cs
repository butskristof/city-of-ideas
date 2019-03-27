using System.Collections.Generic;
using COI.BL.Domain.Questionnaire;

namespace COI.DAL.Questionnaire
{
	public interface IQuestionRepository
	{
		IEnumerable<Question> ReadQuestions();
		OpenQuestion ReadOpenQuestion(int questionId);
		Choice ReadChoice(int choiceId);
		Option ReadOption(int optionId);
		
		void UpdateQuestion(Question question);
		void UpdateOption(Option option);
	}
}