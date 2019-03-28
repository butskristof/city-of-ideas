using System.Collections.Generic;
using System.Linq;
using COI.DAL.EF;

namespace COI.DAL.Questionnaire.EF
{
	public class QuestionnaireRepository : EfRepository, IQuestionnaireRepository
	{
		public QuestionnaireRepository(CityOfIdeasDbContext ctx) : base(ctx)
		{
		}

		public QuestionnaireRepository(UnitOfWork uow) : base(uow)
		{
		}

		public IEnumerable<BL.Domain.Questionnaire.Questionnaire> ReadQuestionnaires()
		{
			return _ctx.Questionnaires.AsEnumerable();
		}

		public BL.Domain.Questionnaire.Questionnaire ReadQuestionnaire(int questionnaireId)
		{
			return _ctx.Questionnaires.Find(questionnaireId);
		}
	}
}