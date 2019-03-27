using System.Collections.Generic;
using COI.BL.Domain.User;

namespace COI.DAL.User
{
	public interface IVoteRepository
	{
		IEnumerable<Vote> ReadVotes();

		Vote CreateVote(Vote v);
	}
}