using System;
using COI.BL.Domain.Questionnaire;
using COI.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace COI.DAL.Questionnaire.EF
{
	public class OptionRepository : EfRepository, IOptionRepository
	{
		public OptionRepository(CityOfIdeasDbContext ctx) : base(ctx)
		{
		}

		public Option ReadOption(int optionId)
		{
			return _ctx.Options.Find(optionId);
		}

		public Option CreateOption(Option option)
		{
			if (ReadOption(option.OptionId) != null)
			{
				throw new ArgumentException("Option already in database.");
			}

			try
			{
				_ctx.Options.Add(option);
				_ctx.SaveChanges();

				return option;
			}
			catch (DbUpdateException exception)
			{
				var msg = exception.InnerException == null ? "Invalid object." : exception.InnerException.Message;
				throw new ArgumentException(msg);
			}
		}

		public Option UpdateOption(Option option)
		{
			var entryToUpdate = ReadOption(option.OptionId);

			if (entryToUpdate == null)
			{
				throw new ArgumentException("Option to update not found.");
			}

			_ctx.Entry(entryToUpdate).CurrentValues.SetValues(option);
			_ctx.SaveChanges();

			return ReadOption(option.OptionId);
		}

		public Option DeleteOption(int optionId)
		{
			var toDelete = ReadOption(optionId);
			if (toDelete == null)
			{
				throw new ArgumentException("Option to delete not found.");
			}

			_ctx.Options.Remove(toDelete);
			_ctx.SaveChanges();
			return toDelete;
		}
	}
}