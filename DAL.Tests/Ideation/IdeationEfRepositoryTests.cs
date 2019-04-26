using System;
using System.Collections.Generic;
using System.Linq;
using COI.DAL.Ideation.EF;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace COI.DAL.Tests.Ideation
{
	public class IdeationEfRepositoryTests
	{
		[Fact]
		public void ReadIdeations_WithEmptyDb_ReturnsEmptyList()
		{
			// arrange
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				IEnumerable<BL.Domain.Ideation.Ideation> ideations = null;
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new IdeationRepository(ctx);
					ideations = repo.ReadIdeations().ToList();
				}

				// assert
				Assert.NotNull(ideations);
				Assert.Empty(ideations);
			}
		}

		[Fact]
		public void ReadIdeations_WithData_ReturnsList()
		{
			// arrange
			var testIdeation = new BL.Domain.Ideation.Ideation()
			{
				Title = "Test"
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				IEnumerable<BL.Domain.Ideation.Ideation> ideations = null;
				using (var ctx = factory.CreateContext())
				{
					ctx.Ideations.Add(testIdeation);
					ctx.SaveChanges();
					
					// act
					var repo = new IdeationRepository(ctx);
					ideations = repo.ReadIdeations().ToList();
				}
				
				// assert
				Assert.NotNull(ideations);
				Assert.Single(ideations);
				Assert.Equal(testIdeation, ideations.FirstOrDefault());
			}
		}
		
		// todo test ReadIdeasForIdeation

		[Fact]
		public void ReadIdeation_WithValidId_ReturnsObject()
		{
			// arrange
			var testIdeation = new BL.Domain.Ideation.Ideation()
			{
				Title = "Test"
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				BL.Domain.Ideation.Ideation retrievedIdeation = null;
				using (var ctx = factory.CreateContext())
				{
					ctx.Ideations.Add(testIdeation);
					ctx.SaveChanges();
					
					// act
					var id = ctx.Ideations.FirstOrDefault().IdeationId;
					var repo = new IdeationRepository(ctx);
					retrievedIdeation = repo.ReadIdeation(id);
				}
				
				// assert
				Assert.NotNull(retrievedIdeation);
				Assert.Equal(testIdeation, retrievedIdeation);
			}
		}
		
		[Fact]
		public void ReadIdeation_WithInvalidId_ReturnsNull()
		{
			var invalidId = 0;
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				BL.Domain.Ideation.Ideation retrievedIdeation = null;
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new IdeationRepository(ctx);
					retrievedIdeation = repo.ReadIdeation(invalidId);
				}
				
				// assert
				Assert.Null(retrievedIdeation);
			}
		}

		[Fact]
		public void CreateIdeation_WithValidObject_ReturnsObjectWithId()
		{
			// arrange
			var testIdeation = new BL.Domain.Ideation.Ideation()
			{
				Title = "Test"
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				BL.Domain.Ideation.Ideation retrievedIdeation = null;
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new IdeationRepository(ctx);
					retrievedIdeation = repo.CreateIdeation(testIdeation);
				}
				
				// assert
				Assert.NotNull(retrievedIdeation);
				Assert.Equal(testIdeation, retrievedIdeation);
				Assert.Equal(1, retrievedIdeation.IdeationId);
			}
		}
		
		[Fact]
		public void CreateIdeation_WithDuplicateObject_Throws()
		{
			// arrange
			var testIdeation = new BL.Domain.Ideation.Ideation()
			{
				Title = "Test"
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				Action duplicateAdd = null;
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new IdeationRepository(ctx);
					repo.CreateIdeation(testIdeation);
					// this will throw an exception
					duplicateAdd = () => repo.CreateIdeation(testIdeation);
					
					// assert
					var exception = Assert.Throws<ArgumentException>(duplicateAdd);
					Assert.Equal("Ideation already in database.", exception.Message);
				}
			}
		}

        public static IEnumerable<object[]> GetIdeations()
		{
			yield return new object[]
			{
				new BL.Domain.Ideation.Ideation(),
				"SQLite Error 19: 'NOT NULL constraint failed: Ideations.Title'."
			};
		}

		[Theory]
		[MemberData(nameof(GetIdeations))]
		public void CreateIdeation_WithInvalidObject_Throws(
			BL.Domain.Ideation.Ideation toCreate,
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
					var repo = new IdeationRepository(ctx);
					result = () => repo.CreateIdeation(toCreate);
					
					// assert
					var exception = Assert.Throws<ArgumentException>(result);
					Assert.Equal(msg, exception.Message);
				}
			}
		}
		
		[Fact]
		public void UpdateIdeation_WithValidObject_ReturnsObjectWithId()
		{
			// arrange
			var testIdeation = new BL.Domain.Ideation.Ideation()
			{
				Title = "Test"
			};
			var newTitle = "New Test";
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				BL.Domain.Ideation.Ideation retrievedIdeation = null;
				using (var ctx = factory.CreateContext())
				{
					ctx.Ideations.Add(testIdeation);
					ctx.SaveChanges();

					testIdeation.Title = newTitle;
					
					// act
					var repo = new IdeationRepository(ctx);
					retrievedIdeation = repo.UpdateIdeation(testIdeation);
				}
				
				// assert
				Assert.NotNull(retrievedIdeation);
				Assert.Equal(testIdeation, retrievedIdeation);
				Assert.Equal(newTitle, retrievedIdeation.Title);
			}
		}

		[Fact]
		public void UpdateIdeation_WithInvalidId_Throws()
		{
			// arrange
			var testIdeation = new BL.Domain.Ideation.Ideation()
			{
				Title = "Test"
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				using (var ctx = factory.CreateContext())
				{
					ctx.Ideations.Add(testIdeation);
					ctx.SaveChanges();

					testIdeation.IdeationId = 0;

					// act
					var repo = new IdeationRepository(ctx);
					Action result = () => repo.UpdateIdeation(testIdeation);

					// assert
					var exception = Assert.Throws<ArgumentException>(result);
					Assert.Equal("Ideation to update not found.", exception.Message);
				}
			}
		}

		[Theory]
		[InlineData(null)]
		public void UpdateIdeation_WithInvalidObject_Throws(
			string newTitle
		)
		{
			// arrange
			var testIdeation = new BL.Domain.Ideation.Ideation()
			{
				Title = "Test"
			};

			using (var factory = new CityOfIdeasDbContextFactory())
			{
				using (var ctx = factory.CreateContext())
				{
					ctx.Ideations.Add(testIdeation);
					ctx.SaveChanges();

					testIdeation.Title = newTitle;

					// act
					var repo = new IdeationRepository(ctx);
					Action result = () => repo.UpdateIdeation(testIdeation);

					// assert
					var exception = Assert.Throws<DbUpdateException>(result);
				}
			}
		}

		[Fact]
		public void DeleteIdeation_WithValidId_DeletesIdeation()
		{
			// arrange
			var testIdeation = new BL.Domain.Ideation.Ideation()
			{
				Title = "Test"
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				BL.Domain.Ideation.Ideation retrievedIdeation0 = null;
				BL.Domain.Ideation.Ideation retrievedIdeation1 = null;
				using (var ctx = factory.CreateContext())
				{
					ctx.Ideations.Add(testIdeation);
					ctx.SaveChanges();
					
					// act
					var id = ctx.Ideations.FirstOrDefault().IdeationId;
					var repo = new IdeationRepository(ctx);
					retrievedIdeation0 = repo.DeleteIdeation(id);
					retrievedIdeation1 = repo.ReadIdeation(id);
				}
				
				// assert
				Assert.NotNull(retrievedIdeation0);
				Assert.Equal(testIdeation, retrievedIdeation0);
				Assert.Null(retrievedIdeation1);
			}
		}

		[Fact]
		public void DeleteIdeation_WithInvalidId_Throws()
		{
			// arrange
			var invalidId = 0;
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new IdeationRepository(ctx);
					Action result = () => repo.DeleteIdeation(invalidId);
					
					// assert
					var exception = Assert.Throws<ArgumentException>(result);
					Assert.Equal("Ideation to delete not found.", exception.Message);
				}
			}
		}
	}
}