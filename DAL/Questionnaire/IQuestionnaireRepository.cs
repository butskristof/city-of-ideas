using System.Collections.Generic;
using COI.BL.Domain.Questionnaire;

namespace COI.DAL.Questionnaire
{
	public interface IQuestionnaireRepository
	{
		IEnumerable<BL.Domain.Questionnaire.Questionnaire> ReadQuestionnaires();
		BL.Domain.Questionnaire.Questionnaire ReadQuestionnaire(int questionnaireid);
	}
}