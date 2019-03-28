using System.Collections.Generic;
using System.Linq;
using COI.BL.Domain.Questionnaire;
using COI.DAL.EF;

namespace COI.DAL.Questionnaire.EF
{
	public class QuestionRepository : EfRepository, IQuestionRepository
	{
		public QuestionRepository(CityOfIdeasDbContext ctx) : base(ctx)
		{
		}

		public QuestionRepository(UnitOfWork uow) : base(uow)
		{
		}

		public IEnumerable<Question> ReadQuestions()
		{
			return _ctx.Questions.AsEnumerable();
		}

		public OpenQuestion ReadOpenQuestion(int questionId)
		{
			return _ctx.Questions.Find(questionId) as OpenQuestion;
		}

		public Choice ReadChoice(int choiceId)
		{
			return _ctx.Questions.Find(choiceId) as Choice;
		}

		public Option ReadOption(int optionId)
		{
			return _ctx.Options.Find(optionId);
		}

		public void UpdateQuestion(Question question)
		{
			var questionToUpdate = _ctx.Questions.Find(question.QuestionId);
			_ctx.Entry(questionToUpdate).CurrentValues.SetValues(question);
			_ctx.SaveChanges();
		}

		public void UpdateOption(Option option)
		{
			var optionToUpdate = this.ReadOption(option.OptionId);
			_ctx.Entry(optionToUpdate).CurrentValues.SetValues(option);
			_ctx.SaveChanges();
		}
	}
}