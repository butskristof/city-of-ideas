using System;
using System.Collections.Generic;
using System.Linq;
using COI.BL.Domain.Ideation;
using COI.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace COI.DAL.Questionnaire.EF
{
	public class QuestionnaireRepository : EfRepository, IQuestionnaireRepository
	{
		public QuestionnaireRepository(CityOfIdeasDbContext ctx) : base(ctx)
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

		public BL.Domain.Questionnaire.Questionnaire CreateQuestionnaire(BL.Domain.Questionnaire.Questionnaire questionnaire)
		{
			if (ReadQuestionnaire(questionnaire.QuestionnaireId) != null)
			{
				throw new ArgumentException("Questionnaire already in database.");
			}

			try
			{
				_ctx.Questionnaires.Add(questionnaire);
				_ctx.SaveChanges();

				return questionnaire;
			}
			catch (DbUpdateException exception)
			{
				var msg = exception.InnerException == null ? "Invalid object." : exception.InnerException.Message;
				throw new ArgumentException(msg);
			}
		}

		public BL.Domain.Questionnaire.Questionnaire UpdateQuestionnaire(BL.Domain.Questionnaire.Questionnaire questionnaire)
		{
			var entryToUpdate = ReadQuestionnaire(questionnaire.QuestionnaireId);

			if (entryToUpdate == null)
			{
				throw new ArgumentException("Questionnaire to update not found.");
			}

			_ctx.Entry(entryToUpdate).CurrentValues.SetValues(questionnaire);
			_ctx.SaveChanges();

			return ReadQuestionnaire(questionnaire.QuestionnaireId);
		}

		public BL.Domain.Questionnaire.Questionnaire DeleteQuestionnaire(int questionnaireId)
		{
			var questionnaireToDelete = ReadQuestionnaire(questionnaireId);
			if (questionnaireToDelete == null)
			{
				throw new ArgumentException("Questionnaire to delete not found.");
			}

			_ctx.Questionnaires.Remove(questionnaireToDelete);
			_ctx.SaveChanges();

			return questionnaireToDelete;
		}
	}
}