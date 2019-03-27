using System.Collections.Generic;
using System.Linq;
using COI.BL.Domain.Ideation;
using COI.DAL.EF;

namespace COI.DAL.Ideation.EF
{
	public class IdeaRepository : EfRepository, IIdeaRepository
	{
		public IdeaRepository(CityOfIdeasDbContext ctx) : base(ctx)
		{
		}

		public IdeaRepository(UnitOfWork uow) : base(uow)
		{
		}

		public IEnumerable<Idea> ReadIdeas()
		{
			return this._ctx.Ideas.AsEnumerable();
		}

		public Idea ReadIdea(int ideaId)
		{
			return this._ctx.Ideas.Find(ideaId);
		}

		public void UpdateIdea(Idea idea)
		{
			var ideaToUpdate = _ctx.Ideas.Find(idea.IdeaId);
			_ctx.Entry(ideaToUpdate).CurrentValues.SetValues(idea);
			_ctx.SaveChanges();
		}
	}
}