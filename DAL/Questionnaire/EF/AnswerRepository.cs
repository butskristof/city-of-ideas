using System;
using System.Collections.Generic;
using System.Linq;
using COI.BL.Domain.Questionnaire;
using COI.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace COI.DAL.Questionnaire.EF
{
	public class AnswerRepository : EfRepository, IAnswerRepository
	{
		public AnswerRepository(CityOfIdeasDbContext ctx) : base(ctx)
		{
		}

		public IEnumerable<Answer> ReadAnswersForQuestion(int questionId)
		{
			return _ctx
				.Answers
				.Where(a => a.Question.QuestionId == questionId)
				.AsEnumerable();
		}

		public IEnumerable<Answer> ReadAnswersForOption(int optionId)
		{
			return _ctx
				.Answers
				.Where(a => a.Option.OptionId == optionId)
				.AsEnumerable();
		}

		public Answer ReadAnswer(int answerId)
		{
			return _ctx.Answers.Find(answerId);
		}

		public Answer CreateAnswer(Answer answer)
		{
			if (ReadAnswer(answer.AnswerId) != null)
			{
				throw new ArgumentException("Answer already in database.");
			}

			try
			{
				_ctx.Answers.Add(answer);
				_ctx.SaveChanges();

				return answer;
			}
			catch (DbUpdateException exception)
			{
				var msg = exception.InnerException == null ? "Invalid object." : exception.InnerException.Message;
				throw new ArgumentException(msg);
			}
		}

		public Answer UpdateAnswer(Answer answer)
		{
			var entryToUpdate = ReadAnswer(answer.AnswerId);

			if (entryToUpdate == null)
			{
				throw new ArgumentException("Answer to update not found.");
			}

			_ctx.Entry(entryToUpdate).CurrentValues.SetValues(answer);
			_ctx.SaveChanges();

			return ReadAnswer(answer.AnswerId);
		}

		public Answer DeleteAnswer(int answerId)
		{
			var answerToDelete = ReadAnswer(answerId);
			if (answerToDelete == null)
			{
				throw new ArgumentException("Answer to delete not found.");
			}

			_ctx.Answers.Remove(answerToDelete);
			_ctx.SaveChanges();

			return answerToDelete;
		}
	}
}