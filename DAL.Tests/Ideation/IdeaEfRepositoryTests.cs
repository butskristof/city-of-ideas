using System;
using System.Collections.Generic;
using System.Linq;
using COI.BL.Domain.Ideation;
using COI.DAL.Ideation.EF;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace COI.DAL.Tests.Ideation
{
	public class IdeaEfRepositoryTests
	{
		[Fact]
		public void ReadIdeas_WithEmptyDb_ReturnsEmptyList()
		{
			// arrange
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				IEnumerable<Idea> ideas = null;
				using (var ctx = factory.CreateContext())
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
		public void ReadIdeas_WithData_ReturnsList()
		{
			// arrange
			var testIdea = new Idea()
			{
				Title = "Test"
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				IEnumerable<Idea> ideas = null;
				using (var ctx = factory.CreateContext())
				{
					ctx.Ideas.Add(testIdea);
					ctx.SaveChanges();
					
					// act
					var repo = new IdeaRepository(ctx);
					ideas = repo.ReadIdeas().ToList();
				}
				
				// assert
				Assert.NotNull(ideas);
				Assert.Single(ideas);
				Assert.Equal(testIdea, ideas.FirstOrDefault());
			}
		}

		[Fact]
		public void ReadIdea_WithValidId_ReturnsObject()
		{
			// arrange
			var testIdea = new Idea()
			{
				Title = "Test",
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				Idea retrievedIdea = null;
				using (var ctx = factory.CreateContext())
				{
					ctx.Ideas.Add(testIdea);
					ctx.SaveChanges();
					
					// act
					var id = ctx.Ideas.FirstOrDefault().IdeaId;
					var repo = new IdeaRepository(ctx);
					retrievedIdea = repo.ReadIdea(id);
				}
				
				// assert
				Assert.NotNull(retrievedIdea);
				Assert.Equal(testIdea, retrievedIdea);
			}
		}
		
		[Fact]
		public void ReadIdea_WithInvalidId_ReturnsNull()
		{
			var invalidId = 0;
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				Idea retrievedIdea = null;
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new IdeaRepository(ctx);
					retrievedIdea = repo.ReadIdea(invalidId);
				}
				
				// assert
				Assert.Null(retrievedIdea);
			}
		}

		[Fact]
		public void CreateIdea_WithValidObject_ReturnsObjectWithId()
		{
			// arrange
			var testIdea = new Idea()
			{
				Title = "Test",
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				Idea retrievedIdea = null;
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new IdeaRepository(ctx);
					retrievedIdea = repo.CreateIdea(testIdea);
				}
				
				// assert
				Assert.NotNull(retrievedIdea);
				Assert.Equal(testIdea, retrievedIdea);
				Assert.Equal(1, retrievedIdea.IdeaId);
			}
		}
		
		[Fact]
		public void CreateIdea_WithDuplicateObject_Throws()
		{
			// arrange
			var testIdea = new Idea()
			{
				Title = "Test",
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				Action duplicateAdd = null;
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new IdeaRepository(ctx);
					repo.CreateIdea(testIdea);
					// this will throw an exception
					duplicateAdd = () => repo.CreateIdea(testIdea);
					
					// assert
					var exception = Assert.Throws<ArgumentException>(duplicateAdd);
					Assert.Equal("Idea already in database.", exception.Message);
				}
			}
		}

        public static IEnumerable<object[]> GetIdeas()
		{
			yield return new object[]
			{
				new Idea(),
				"SQLite Error 19: 'NOT NULL constraint failed: Ideas.Title'."
			};
		}

		[Theory]
		[MemberData(nameof(GetIdeas))]
		public void CreateIdea_WithInvalidObject_Throws(
			Idea toCreate,
			string msg
		)
		{
			// arrange
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				Action result = null;
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new IdeaRepository(ctx);
					result = () => repo.CreateIdea(toCreate);
					
					// assert
					var exception = Assert.Throws<ArgumentException>(result);
					Assert.Equal(msg, exception.Message);
				}
			}
		}
		
		[Fact]
		public void UpdateIdea_WithValidObject_ReturnsObjectWithId()
		{
			// arrange
			var testIdea = new Idea()
			{
				Title = "Test",
			};
			var newTitle = "New Test";
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				Idea retrievedIdea = null;
				using (var ctx = factory.CreateContext())
				{
					ctx.Ideas.Add(testIdea);
					ctx.SaveChanges();

					testIdea.Title = newTitle;
					
					// act
					var repo = new IdeaRepository(ctx);
					retrievedIdea = repo.UpdateIdea(testIdea);
				}
				
				// assert
				Assert.NotNull(retrievedIdea);
				Assert.Equal(testIdea, retrievedIdea);
				Assert.Equal(newTitle, retrievedIdea.Title);
			}
		}

		[Fact]
		public void UpdateIdea_WithInvalidId_Throws()
		{
			// arrange
			var testIdea = new Idea()
			{
				Title = "Test",
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				using (var ctx = factory.CreateContext())
				{
					ctx.Ideas.Add(testIdea);
					ctx.SaveChanges();

					testIdea.IdeaId = 0;

					// act
					var repo = new IdeaRepository(ctx);
					Action result = () => repo.UpdateIdea(testIdea);

					// assert
					var exception = Assert.Throws<ArgumentException>(result);
					Assert.Equal("Idea to update not found.", exception.Message);
				}
			}
		}

		[Theory]
		[InlineData(null)]
		public void UpdateIdea_WithInvalidObject_Throws(
			string newTitle
		)
		{
			// arrange
			var testIdea = new Idea()
			{
				Title = "Test",
			};

			using (var factory = new CityOfIdeasDbContextFactory())
			{
				using (var ctx = factory.CreateContext())
				{
					ctx.Ideas.Add(testIdea);
					ctx.SaveChanges();

					testIdea.Title = newTitle;

					// act
					var repo = new IdeaRepository(ctx);
					Action result = () => repo.UpdateIdea(testIdea);

					// assert
					var exception = Assert.Throws<DbUpdateException>(result);
				}
			}
		}

		[Fact]
		public void DeleteIdea_WithValidId_DeletesIdea()
		{
			// arrange
			var testIdea = new Idea()
			{
				Title = "Test",
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				Idea retrievedIdea0 = null;
				Idea retrievedIdea1 = null;
				using (var ctx = factory.CreateContext())
				{
					ctx.Ideas.Add(testIdea);
					ctx.SaveChanges();
					
					// act
					var id = ctx.Ideas.FirstOrDefault().IdeaId;
					var repo = new IdeaRepository(ctx);
					retrievedIdea0 = repo.DeleteIdea(id);
					retrievedIdea1 = repo.ReadIdea(id);
				}
				
				// assert
				Assert.NotNull(retrievedIdea0);
				Assert.Equal(testIdea, retrievedIdea0);
				Assert.Null(retrievedIdea1);
			}
		}

		[Fact]
		public void DeleteIdea_WithInvalidId_Throws()
		{
			// arrange
			var invalidId = 0;
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new IdeaRepository(ctx);
					Action result = () => repo.DeleteIdea(invalidId);
					
					// assert
					var exception = Assert.Throws<ArgumentException>(result);
					Assert.Equal("Idea to delete not found.", exception.Message);
				}
			}
		}
	}
}