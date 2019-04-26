using System.Collections.Generic;
using COI.BL.Domain.User;

namespace COI.DAL.User
{
	public interface IVoteRepository
	{
		IEnumerable<Vote> ReadVotes();
		Vote ReadVote(int voteId);
		Vote CreateVote(Vote vote);
		Vote UpdateVote(Vote vote);
		Vote DeleteVote(int voteId);
	}
}