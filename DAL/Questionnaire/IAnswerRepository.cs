using System.Collections.Generic;
using COI.BL.Domain.Questionnaire;

namespace COI.DAL.Questionnaire
{
	public interface IAnswerRepository
	{
		Answer CreateAnswer(Answer answer);
	}
}