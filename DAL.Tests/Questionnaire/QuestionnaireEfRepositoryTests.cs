using System;
using System.Collections.Generic;
using System.Linq;
using COI.DAL.Questionnaire.EF;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace COI.DAL.Tests.Questionnaire
{
	public class QuestionnaireEfRepositoryTests
	{
		[Fact]
		public void ReadQuestionnaire_WithValidId_ReturnsObject()
		{
			// arrange
			var testQuestionnaire = new BL.Domain.Questionnaire.Questionnaire()
			{
				Title = "Test"
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				BL.Domain.Questionnaire.Questionnaire retrievedQuestionnaire = null;
				using (var ctx = factory.CreateContext())
				{
					ctx.Questionnaires.Add(testQuestionnaire);
					ctx.SaveChanges();
					
					// act
					var id = ctx.Questionnaires.FirstOrDefault().QuestionnaireId;
					var repo = new QuestionnaireRepository(ctx);
					retrievedQuestionnaire = repo.ReadQuestionnaire(id);
				}
				
				// assert
				Assert.NotNull(retrievedQuestionnaire);
				Assert.Equal(testQuestionnaire, retrievedQuestionnaire);
			}
		}
		
		[Fact]
		public void ReadQuestionnaire_WithInvalidId_ReturnsNull()
		{
			var invalidId = 0;
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				BL.Domain.Questionnaire.Questionnaire retrievedQuestionnaire = null;
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new QuestionnaireRepository(ctx);
					retrievedQuestionnaire = repo.ReadQuestionnaire(invalidId);
				}
				
				// assert
				Assert.Null(retrievedQuestionnaire);
			}
		}

		[Fact]
		public void CreateQuestionnaire_WithValidObject_ReturnsObjectWithId()
		{
			// arrange
			var testQuestionnaire = new BL.Domain.Questionnaire.Questionnaire()
			{
				Title = "Test"
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				BL.Domain.Questionnaire.Questionnaire retrievedQuestionnaire = null;
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new QuestionnaireRepository(ctx);
					retrievedQuestionnaire = repo.CreateQuestionnaire(testQuestionnaire);
				}
				
				// assert
				Assert.NotNull(retrievedQuestionnaire);
				Assert.Equal(testQuestionnaire, retrievedQuestionnaire);
				Assert.Equal(1, retrievedQuestionnaire.QuestionnaireId);
			}
		}
		
		[Fact]
		public void CreateQuestionnaire_WithDuplicateObject_Throws()
		{
			// arrange
			var testQuestionnaire = new BL.Domain.Questionnaire.Questionnaire()
			{
				Title = "Test"
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				Action duplicateAdd = null;
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new QuestionnaireRepository(ctx);
					repo.CreateQuestionnaire(testQuestionnaire);
					// this will throw an exception
					duplicateAdd = () => repo.CreateQuestionnaire(testQuestionnaire);
					
					// assert
					var exception = Assert.Throws<ArgumentException>(duplicateAdd);
					Assert.Equal("Questionnaire already in database.", exception.Message);
				}
			}
		}

        public static IEnumerable<object[]> GetQuestionnaires()
		{
			yield return new object[]
			{
				new BL.Domain.Questionnaire.Questionnaire(),
				"SQLite Error 19: 'NOT NULL constraint failed: Questionnaires.Title'."
			};
		}

		[Theory]
		[MemberData(nameof(GetQuestionnaires))]
		public void CreateQuestionnaire_WithInvalidObject_Throws(
			BL.Domain.Questionnaire.Questionnaire toCreate,
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
					var repo = new QuestionnaireRepository(ctx);
					result = () => repo.CreateQuestionnaire(toCreate);
					
					// assert
					var exception = Assert.Throws<ArgumentException>(result);
					Assert.Equal(msg, exception.Message);
				}
			}
		}
		
		[Fact]
		public void UpdateQuestionnaire_WithValidObject_ReturnsObjectWithId()
		{
			// arrange
			var testQuestionnaire = new BL.Domain.Questionnaire.Questionnaire()
			{
				Title = "Test",
			};
			var newTitle = "New Test";
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				BL.Domain.Questionnaire.Questionnaire retrievedQuestionnaire = null;
				using (var ctx = factory.CreateContext())
				{
					ctx.Questionnaires.Add(testQuestionnaire);
					ctx.SaveChanges();

					testQuestionnaire.Title = newTitle;
					
					// act
					var repo = new QuestionnaireRepository(ctx);
					retrievedQuestionnaire = repo.UpdateQuestionnaire(testQuestionnaire);
				}
				
				// assert
				Assert.NotNull(retrievedQuestionnaire);
				Assert.Equal(testQuestionnaire, retrievedQuestionnaire);
				Assert.Equal(newTitle, retrievedQuestionnaire.Title);
			}
		}

		[Fact]
		public void UpdateQuestionnaire_WithInvalidId_Throws()
		{
			// arrange
			var testQuestionnaire = new BL.Domain.Questionnaire.Questionnaire()
			{
				Title = "Test",
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				using (var ctx = factory.CreateContext())
				{
					ctx.Questionnaires.Add(testQuestionnaire);
					ctx.SaveChanges();

					testQuestionnaire.QuestionnaireId = 0;

					// act
					var repo = new QuestionnaireRepository(ctx);
					Action result = () => repo.UpdateQuestionnaire(testQuestionnaire);

					// assert
					var exception = Assert.Throws<ArgumentException>(result);
					Assert.Equal("Questionnaire to update not found.", exception.Message);
				}
			}
		}

		[Theory]
		[InlineData(null)]
		public void UpdateQuestionnaire_WithInvalidObject_Throws(
			string newTitle
		)
		{
			// arrange
			var testQuestionnaire = new BL.Domain.Questionnaire.Questionnaire()
			{
				Title = "Test",
			};

			using (var factory = new CityOfIdeasDbContextFactory())
			{
				using (var ctx = factory.CreateContext())
				{
					ctx.Questionnaires.Add(testQuestionnaire);
					ctx.SaveChanges();

					testQuestionnaire.Title = newTitle;

					// act
					var repo = new QuestionnaireRepository(ctx);
					Action result = () => repo.UpdateQuestionnaire(testQuestionnaire);

					// assert
					var exception = Assert.Throws<DbUpdateException>(result);
				}
			}
		}

		[Fact]
		public void DeleteQuestionnaire_WithValidId_DeletesQuestionnaire()
		{
			// arrange
			var testQuestionnaire = new BL.Domain.Questionnaire.Questionnaire()
			{
				Title = "Test",
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				BL.Domain.Questionnaire.Questionnaire retrievedQuestionnaire0 = null;
				BL.Domain.Questionnaire.Questionnaire retrievedQuestionnaire1 = null;
				using (var ctx = factory.CreateContext())
				{
					ctx.Questionnaires.Add(testQuestionnaire);
					ctx.SaveChanges();
					
					// act
					var id = ctx.Questionnaires.FirstOrDefault().QuestionnaireId;
					var repo = new QuestionnaireRepository(ctx);
					retrievedQuestionnaire0 = repo.DeleteQuestionnaire(id);
					retrievedQuestionnaire1 = repo.ReadQuestionnaire(id);
				}
				
				// assert
				Assert.NotNull(retrievedQuestionnaire0);
				Assert.Equal(testQuestionnaire, retrievedQuestionnaire0);
				Assert.Null(retrievedQuestionnaire1);
			}
		}

		[Fact]
		public void DeleteQuestionnaire_WithInvalidId_Throws()
		{
			// arrange
			var invalidId = 0;
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new QuestionnaireRepository(ctx);
					Action result = () => repo.DeleteQuestionnaire(invalidId);
					
					// assert
					var exception = Assert.Throws<ArgumentException>(result);
					Assert.Equal("Questionnaire to delete not found.", exception.Message);
				}
			}
		}
	}
}