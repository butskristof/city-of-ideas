using System;
using System.Collections.Generic;
using System.Linq;
using COI.BL.Domain.User;
using COI.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace COI.DAL.User.EF
{
	public class VoteRepository : EfRepository, IVoteRepository
	{
		public VoteRepository(CityOfIdeasDbContext ctx) : base(ctx)
		{
		}

		public IEnumerable<Vote> ReadVotes()
		{
			return _ctx.Votes.AsEnumerable();
		}

		public Vote ReadVote(int voteId)
		{
			return _ctx.Votes.Find(voteId);
		}

		public Vote CreateVote(Vote vote)
		{
			
			if (ReadVote(vote.VoteId) != null)
			{
				throw new ArgumentException("Vote already in database.");
			}

			try
			{
				_ctx.Votes.Add(vote);
				_ctx.SaveChanges();

				return vote;
			}
			catch (DbUpdateException exception)
			{
				var msg = exception.InnerException == null ? "Invalid object." : exception.InnerException.Message;
				throw new ArgumentException(msg);
			}
		}

		public Vote UpdateVote(Vote vote)
		{
			var entryToUpdate = ReadVote(vote.VoteId);

			if (entryToUpdate == null)
			{
				throw new ArgumentException("Vote to update not found.");
			}

			_ctx.Entry(entryToUpdate).CurrentValues.SetValues(vote);
			_ctx.SaveChanges();

			return ReadVote(vote.VoteId);
		}

		public Vote DeleteVote(int voteId)
		{
			var voteToDelete = ReadVote(voteId);
			if (voteToDelete == null)
			{
				throw new ArgumentException("Vote to delete not found.");
			}

			_ctx.Votes.Remove(voteToDelete);
			_ctx.SaveChanges();

			return voteToDelete;
		}
	}
}