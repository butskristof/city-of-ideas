using System.Collections.Generic;
using COI.BL.Domain.Questionnaire;

namespace COI.DAL.Questionnaire
{
	public interface IQuestionnaireRepository
	{
		IEnumerable<BL.Domain.Questionnaire.Questionnaire> ReadQuestionnaires();
		BL.Domain.Questionnaire.Questionnaire ReadQuestionnaire(int questionnaireId);
		BL.Domain.Questionnaire.Questionnaire CreateQuestionnaire(BL.Domain.Questionnaire.Questionnaire questionnaire);
		BL.Domain.Questionnaire.Questionnaire UpdateQuestionnaire(BL.Domain.Questionnaire.Questionnaire questionnaire);
		BL.Domain.Questionnaire.Questionnaire DeleteQuestionnaire(int questionnaireId);
	}
}