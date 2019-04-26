using System;
using System.Collections.Generic;
using System.Linq;
using COI.BL.Domain.Ideation;
using COI.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace COI.DAL.Ideation.EF
{
	public class IdeationRepository : EfRepository, IIdeationRepository
	{
		public IdeationRepository(CityOfIdeasDbContext ctx) : base(ctx)
		{
		}

		public IEnumerable<BL.Domain.Ideation.Ideation> ReadIdeations()
		{
			return _ctx.Ideations.AsEnumerable();
		}

		public BL.Domain.Ideation.Ideation ReadIdeation(int ideationId)
		{
			return this._ctx.Ideations.Find(ideationId);
		}

		public IEnumerable<Idea> ReadIdeasForIdeation(int ideationId)
		{
			return _ctx
				.Ideas
				.Where(i => i.Ideation.IdeationId == ideationId)
				.AsEnumerable();
		}

		public BL.Domain.Ideation.Ideation CreateIdeation(BL.Domain.Ideation.Ideation ideation)
		{
			if (ReadIdeation(ideation.IdeationId) != null)
			{
				throw new ArgumentException("Ideation already in database.");
			}

			try
			{
				_ctx.Ideations.Add(ideation);
				_ctx.SaveChanges();

				return ideation;
			}
			catch (DbUpdateException exception)
			{
				var msg = exception.InnerException == null ? "Invalid object." : exception.InnerException.Message;
				throw new ArgumentException(msg);
			}
		}

		public BL.Domain.Ideation.Ideation UpdateIdeation(BL.Domain.Ideation.Ideation updatedIdeation)
		{
			var entryToUpdate = ReadIdeation(updatedIdeation.IdeationId);
			
			if (entryToUpdate == null)
			{
				throw new ArgumentException("Ideation to update not found.");
			}
			
			_ctx.Entry(entryToUpdate).CurrentValues.SetValues(updatedIdeation);
			_ctx.SaveChanges();

			return ReadIdeation(updatedIdeation.IdeationId);
		}

		public BL.Domain.Ideation.Ideation DeleteIdeation(int ideationId)
		{
			var ideationToDelete = ReadIdeation(ideationId);
			
			if (ideationToDelete == null)
			{
				throw new ArgumentException("Ideation to delete not found.");
			}
			
			_ctx.Ideations.Remove(ideationToDelete);
			_ctx.SaveChanges();
			
			return ideationToDelete;
		}
	}
}