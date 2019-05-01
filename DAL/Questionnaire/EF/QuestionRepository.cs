using System;
using System.Collections.Generic;
using System.Linq;
using COI.BL.Domain.Questionnaire;
using COI.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace COI.DAL.Questionnaire.EF
{
	public class QuestionRepository : EfRepository, IQuestionRepository
	{
		public QuestionRepository(CityOfIdeasDbContext ctx) : base(ctx)
		{
		}

		public IEnumerable<Question> ReadQuestions()
		{
			return _ctx.Questions.AsEnumerable();
		}

		public Question ReadQuestion(int questionId)
		{
			return _ctx.Questions.Find(questionId);
		}

		public Question CreateQuestion(Question question)
		{
			if (ReadQuestion(question.QuestionId) != null)
			{
				throw new ArgumentException("Question already in database.");
			}

			try
			{
				_ctx.Questions.Add(question);
				_ctx.SaveChanges();

				return question;
			}
			catch (DbUpdateException exception)
			{
				var msg = exception.InnerException == null ? "Invalid object." : exception.InnerException.Message;
				throw new ArgumentException(msg);
			}
		}

		public Question UpdateQuestion(Question updatedQuestion)
		{
			var entryToUpdate = ReadQuestion(updatedQuestion.QuestionId);

			if (entryToUpdate == null)
			{
				throw new ArgumentException("Question to update not found.");
			}

			_ctx.Entry(entryToUpdate).CurrentValues.SetValues(updatedQuestion);
			_ctx.SaveChanges();

			return ReadQuestion(updatedQuestion.QuestionId);
		}

		public Question DeleteQuestion(int questionId)
		{
			var toDelete = ReadQuestion(questionId);
			if (toDelete == null)
			{
				throw new ArgumentException("Question to delete not found.");
			}

			_ctx.Questions.Remove(toDelete);
			_ctx.SaveChanges();

			return toDelete;
		}

		public IEnumerable<Option> ReadOptionsForQuestion(int questionId)
		{
			return _ctx
				.Options
				.Where(o => o.Question.QuestionId == questionId)
				.AsEnumerable();
		}

		public Option ReadOption(int optionId)
		{
			return _ctx.Options.Find(optionId);
		}

		public void UpdateOption(Option option)
		{
			var optionToUpdate = this.ReadOption(option.OptionId);
			_ctx.Entry(optionToUpdate).CurrentValues.SetValues(option);
			_ctx.SaveChanges();
		}
	}
}