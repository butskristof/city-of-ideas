using System.Collections.Generic;
using System.Linq;
using COI.BL.Domain.User;
using COI.DAL.EF;

namespace COI.DAL.User.EF
{
	public class VoteRepository : EfRepository, IVoteRepository
	{
		public VoteRepository(CityOfIdeasDbContext ctx) : base(ctx)
		{
		}

		public VoteRepository(UnitOfWork uow) : base(uow)
		{
		}

		public IEnumerable<Vote> ReadVotes()
		{
			return _ctx.Votes.AsEnumerable();
		}

		public Vote CreateVote(Vote v)
		{
			_ctx.Votes.Add(v);
			_ctx.SaveChanges();

			return v;
		}
	}
}