using System.Collections.Generic;
using COI.BL.Domain.User;

namespace COI.DAL.User
{
	public interface IVoteRepository
	{
		IEnumerable<Vote> ReadVotes();
		IEnumerable<Vote> ReadVotesForIdea(int ideaId);
		IEnumerable<Vote> ReadVotesForIdeation(int ideationId);
		IEnumerable<Vote> ReadVotesForComment(int commentId);
		Vote ReadVote(int voteId);
		Vote CreateVote(Vote vote);
		Vote UpdateVote(Vote vote);
		Vote DeleteVote(int voteId);
	}
}