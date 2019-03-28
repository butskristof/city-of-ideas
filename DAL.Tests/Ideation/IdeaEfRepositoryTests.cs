using System;
using System.Collections.Generic;
using System.Linq;
using COI.BL.Domain.Ideation;
using COI.DAL.Ideation.EF;
using Xunit;

namespace COI.DAL.Tests.Ideation
{
	public class IdeaEfRepositoryTests
	{
		[Fact]
		public void ReadIdeas_WithNoIdeasInDb_ReturnsEmptyList()
		{
			// arrange
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				IEnumerable<Idea> ideas = null;
				using (var ctx = factory.CreateContext(false))
				{
					// act
					var repo = new IdeaRepository(ctx);
					ideas = repo.ReadIdeas().ToList();
				}
				
				// assert
				Assert.NotNull(ideas);
				Assert.Empty(ideas);
			}
		}

		[Fact]
		public void ReadIdeas_WithIdeasInDb_ReturnsListOfIdeas()
		{
			// arrange
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				IEnumerable<Idea> ideas = null;
				using (var ctx = factory.CreateContext(true))
				{
					// act
					var repo = new IdeaRepository(ctx);
					ideas = repo.ReadIdeas().ToList();
				}
				
				// assert
				Assert.NotNull(ideas);
				Assert.Equal(3, ideas.Count());
			}
		}

		[Fact]
		public void ReadIdea_WithInvalidId_ReturnsNull()
		{
			// arrange
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				const int id = 0;
				Idea result = null;
				using (var ctx = factory.CreateContext(false))
				{
					// act
					var repo = new IdeaRepository(ctx);
					result = repo.ReadIdea(id);
				}
				
				// assert
				Assert.Null(result);
			}
		}

		[Fact]
		public void ReadIdea_WithValidId_ReturnsIdea()
		{
			// arrange
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				const int id = 1;
				Idea result = null;
				using (var ctx = factory.CreateContext(true))
				{
					// act
					var repo = new IdeaRepository(ctx);
					result = repo.ReadIdea(id);
					
					// assert
					Assert.NotNull(result);
					Assert.Equal(id, result.IdeaId);
					Assert.Equal("Nieuwe bankjes rond het meer", result.Title);
					Assert.Equal(3, result.Comments.Count);
					Assert.Equal(4, result.Votes.Count);
					Assert.Equal(2, result.GetScore());
					Assert.Equal(5, result.CreatedBy.UserId);
				}
			}
		}

		[Fact]
		public void UpdateIdea_WithModifiedValues_UpdatesIdea()
		{
			// arrange
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				const int id = 1;
				String newTitle = "New Title";
				
				// act
				using (var ctx = factory.CreateContext(true))
				{
					var repo = new IdeaRepository(ctx);
					Idea idea = repo.ReadIdea(id);
					idea.Title = newTitle;
					
					repo.UpdateIdea(idea);
				}
				
				// assert
				Idea result = null;
				using (var ctx = factory.CreateContext(false))
				{
					var repo = new IdeaRepository(ctx);
					result = repo.ReadIdea(id);
					
					Assert.NotNull(result);
					Assert.Equal(id, result.IdeaId);
					Assert.Equal(newTitle, result.Title);
					Assert.Equal(3, result.Comments.Count);
					Assert.Equal(4, result.Votes.Count);
					Assert.Equal(2, result.GetScore());
					Assert.Equal(5, result.CreatedBy.UserId);
				}
			}
		}
	}
}