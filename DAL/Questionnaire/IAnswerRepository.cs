using System.Collections.Generic;
using COI.BL.Domain.Questionnaire;

namespace COI.DAL.Questionnaire
{
	public interface IAnswerRepository
	{
//		IEnumerable<Answer> ReadAnswersForQuestion(int questionId);
		Answer ReadAnswer(int answerId);
		Answer CreateAnswer(Answer answer);
		Answer UpdateAnswer(Answer answer);
		Answer DeleteAnswer(int answerId);
	}
}