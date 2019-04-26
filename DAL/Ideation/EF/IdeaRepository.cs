using System;
using System.Collections.Generic;
using System.Linq;
using COI.BL.Domain.Ideation;
using COI.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace COI.DAL.Ideation.EF
{
	public class IdeaRepository : EfRepository, IIdeaRepository
	{
		public IdeaRepository(CityOfIdeasDbContext ctx) : base(ctx)
		{
		}

		public IEnumerable<Idea> ReadIdeas()
		{
			return _ctx.Ideas.AsEnumerable();
		}

		public Idea ReadIdea(int ideaId)
		{
			return _ctx.Ideas.Find(ideaId);
		}

		public Idea CreateIdea(Idea idea)
		{
			if (ReadIdea(idea.IdeaId) != null)
			{
				throw new ArgumentException("Idea already in database.");
			}

			try
			{
				_ctx.Ideas.Add(idea);
				_ctx.SaveChanges();

				return idea;
			}
			catch (DbUpdateException exception)
			{
				var msg = exception.InnerException == null ? "Invalid object." : exception.InnerException.Message;
				throw new ArgumentException(msg);
			}
		}

		public Idea UpdateIdea(Idea updatedIdea)
		{
			var entryToUpdate = _ctx.Ideas.Find(updatedIdea.IdeaId);
			
			if (entryToUpdate == null)
			{
				throw new ArgumentException("Idea to update not found.");
			}
			
			_ctx.Entry(entryToUpdate).CurrentValues.SetValues(updatedIdea);
			_ctx.SaveChanges();

			return ReadIdea(updatedIdea.IdeaId);
		}

		public Idea DeleteIdea(int ideaId)
		{
			var ideaToDelete = ReadIdea(ideaId);
			if (ideaToDelete == null)
			{
				throw new ArgumentException("Idea to delete not found.");
			}

			_ctx.Ideas.Remove(ideaToDelete);
			_ctx.SaveChanges();

			return ideaToDelete;
		}
	}
}