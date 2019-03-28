using COI.BL.Domain.Questionnaire;
using COI.DAL.EF;

namespace COI.DAL.Questionnaire.EF
{
	public class AnswerRepository : EfRepository, IAnswerRepository
	{
		public AnswerRepository(CityOfIdeasDbContext ctx) : base(ctx)
		{
		}

		public AnswerRepository(UnitOfWork uow) : base(uow)
		{
		}

		public Answer CreateAnswer(Answer answer)
		{
			_ctx.Answers.Add(answer);
			_ctx.SaveChanges();

			return answer;
		}
	}
}