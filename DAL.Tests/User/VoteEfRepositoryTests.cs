using System.Collections.Generic;
using System.Linq;
using COI.BL.Domain.User;
using COI.DAL.User.EF;
using Xunit;

namespace COI.DAL.Tests.User
{
	public class VoteEfRepositoryTests
	{
		[Fact]
		public void ReadVotes_WithNoVotesInDb_ReturnsEmptyList()
		{
			// arrange
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				IEnumerable<Vote> result = null;
				using (var ctx = factory.CreateContext(false))
				{
					// act
					var repo = new VoteRepository(ctx);
					result = repo.ReadVotes().ToList();
				}
				
				// assert
				Assert.NotNull(result);
				Assert.Empty(result);
			}
		}

		[Fact]
		public void ReadVotes_WithVotesInDb_ReturnsListOfVotes()
		{
			// arrange
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				IEnumerable<Vote> result = null;
				using (var ctx = factory.CreateContext(true))
				{
					// act
					var repo = new VoteRepository(ctx);
					result = repo.ReadVotes().ToList();
				}
				
				// assert
				Assert.NotNull(result);
				Assert.Equal(4, result.Count());
			}
		}
		
		[Fact]
		public void CreateVote_WithVote_ReturnsVoteWithId()
		{
			// arrange
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				const int expectedId = 1;
				Vote newVote = new Vote();
				Vote returnedVote = null;
				using (var ctx = factory.CreateContext(false))
				{
					// act
					var repo = new VoteRepository(ctx);
					returnedVote = repo.CreateVote(newVote);
				}

				// assert
				Assert.NotNull(returnedVote);
				Assert.NotEqual(0, returnedVote.VoteId);
				Assert.Equal(expectedId, returnedVote.VoteId);
			}
		}
	}
}