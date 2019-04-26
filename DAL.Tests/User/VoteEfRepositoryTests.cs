using System;
using System.Collections.Generic;
using System.Linq;
using COI.BL.Domain.User;
using COI.DAL.User.EF;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace COI.DAL.Tests.User
{
	public class VoteEfRepositoryTests
	{
		private Vote testVote;

		public VoteEfRepositoryTests()
		{
			testVote = new Vote()
			{
				Value = 1
			};
		}

		[Fact]
		public void ReadVotes_WithEmptyDb_ReturnsEmptyList()
		{
			// arrange
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				IEnumerable<Vote> votes = null;
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new VoteRepository(ctx);
					votes = repo.ReadVotes().ToList();
				}

				// assert
				Assert.NotNull(votes);
				Assert.Empty(votes);
			}
		}

		[Fact]
		public void ReadVotes_WithData_ReturnsList()
		{
			// arrange
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				IEnumerable<Vote> votes = null;
				using (var ctx = factory.CreateContext())
				{
					ctx.Votes.Add(testVote);
					ctx.SaveChanges();
					
					// act
					var repo = new VoteRepository(ctx);
					votes = repo.ReadVotes().ToList();
				}
				
				// assert
				Assert.NotNull(votes);
				Assert.Single(votes);
				Assert.Equal(testVote, votes.FirstOrDefault());
			}
		}

		[Fact]
		public void ReadVote_WithValidId_ReturnsObject()
		{
			// arrange
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				Vote retrievedVote = null;
				using (var ctx = factory.CreateContext())
				{
					ctx.Votes.Add(testVote);
					ctx.SaveChanges();
					
					// act
					var id = ctx.Votes.FirstOrDefault().VoteId;
					var repo = new VoteRepository(ctx);
					retrievedVote = repo.ReadVote(id);
				}
				
				// assert
				Assert.NotNull(retrievedVote);
				Assert.Equal(testVote, retrievedVote);
			}
		}
		
		[Fact]
		public void ReadVote_WithInvalidId_ReturnsNull()
		{
			var invalidId = 0;
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				Vote retrievedVote = null;
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new VoteRepository(ctx);
					retrievedVote = repo.ReadVote(invalidId);
				}
				
				// assert
				Assert.Null(retrievedVote);
			}
		}

		[Fact]
		public void CreateVote_WithValidObject_ReturnsObjectWithId()
		{
			// arrange
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				Vote retrievedVote = null;
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new VoteRepository(ctx);
					retrievedVote = repo.CreateVote(testVote);
				}
				
				// assert
				Assert.NotNull(retrievedVote);
				Assert.Equal(testVote, retrievedVote);
				Assert.Equal(1, retrievedVote.VoteId);
			}
		}
		
		[Fact]
		public void CreateVote_WithDuplicateObject_Throws()
		{
			// arrange
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				Action duplicateAdd = null;
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new VoteRepository(ctx);
					repo.CreateVote(testVote);
					// this will throw an exception
					duplicateAdd = () => repo.CreateVote(testVote);
					
					// assert
					var exception = Assert.Throws<ArgumentException>(duplicateAdd);
					Assert.Equal("Vote already in database.", exception.Message);
				}
			}
		}
		
		[Fact]
		public void UpdateVote_WithValidObject_ReturnsObjectWithId()
		{
			// arrange
			var newValue = -1;
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				Vote retrievedVote = null;
				using (var ctx = factory.CreateContext())
				{
					ctx.Votes.Add(testVote);
					ctx.SaveChanges();

					testVote.Value = newValue;
					
					// act
					var repo = new VoteRepository(ctx);
					retrievedVote = repo.UpdateVote(testVote);
				}
				
				// assert
				Assert.NotNull(retrievedVote);
				Assert.Equal(testVote, retrievedVote);
				Assert.Equal(newValue, retrievedVote.Value);
			}
		}

		[Fact]
		public void UpdateVote_WithInvalidId_Throws()
		{
			// arrange
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				using (var ctx = factory.CreateContext())
				{
					ctx.Votes.Add(testVote);
					ctx.SaveChanges();

					testVote.VoteId = 0;

					// act
					var repo = new VoteRepository(ctx);
					Action result = () => repo.UpdateVote(testVote);

					// assert
					var exception = Assert.Throws<ArgumentException>(result);
					Assert.Equal("Vote to update not found.", exception.Message);
				}
			}
		}

		[Fact]
		public void DeleteVote_WithValidId_DeletesVote()
		{
			// arrange
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				Vote retrievedVote0 = null;
				Vote retrievedVote1 = null;
				using (var ctx = factory.CreateContext())
				{
					ctx.Votes.Add(testVote);
					ctx.SaveChanges();
					
					// act
					var id = ctx.Votes.FirstOrDefault().VoteId;
					var repo = new VoteRepository(ctx);
					retrievedVote0 = repo.DeleteVote(id);
					retrievedVote1 = repo.ReadVote(id);
				}
				
				// assert
				Assert.NotNull(retrievedVote0);
				Assert.Equal(testVote, retrievedVote0);
				Assert.Null(retrievedVote1);
			}
		}

		[Fact]
		public void DeleteVote_WithInvalidId_Throws()
		{
			// arrange
			var invalidId = 0;
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new VoteRepository(ctx);
					Action result = () => repo.DeleteVote(invalidId);
					
					// assert
					var exception = Assert.Throws<ArgumentException>(result);
					Assert.Equal("Vote to delete not found.", exception.Message);
				}
			}
		}
	}
}