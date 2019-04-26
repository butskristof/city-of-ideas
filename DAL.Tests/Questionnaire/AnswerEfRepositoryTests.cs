using System;
using System.Linq;
using COI.BL.Domain.Questionnaire;
using COI.DAL.Questionnaire.EF;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace COI.DAL.Tests.Questionnaire
{
	public class AnswerEfRepositoryTests
	{
		[Fact]
		public void ReadAnswer_WithValidId_ReturnsObject()
		{
			// arrange
			var testAnswer = new Answer()
			{
				Content = "Test"
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				Answer retrievedAnswer = null;
				using (var ctx = factory.CreateContext())
				{
					ctx.Answers.Add(testAnswer);
					ctx.SaveChanges();
					
					// act
					var id = ctx.Answers.FirstOrDefault().AnswerId;
					var repo = new AnswerRepository(ctx);
					retrievedAnswer = repo.ReadAnswer(id);
				}
				
				// assert
				Assert.NotNull(retrievedAnswer);
				Assert.Equal(testAnswer, retrievedAnswer);
			}
		}
		
		[Fact]
		public void ReadAnswer_WithInvalidId_ReturnsNull()
		{
			var invalidId = 0;
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				Answer retrievedAnswer = null;
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new AnswerRepository(ctx);
					retrievedAnswer = repo.ReadAnswer(invalidId);
				}
				
				// assert
				Assert.Null(retrievedAnswer);
			}
		}

		[Fact]
		public void CreateAnswer_WithValidObject_ReturnsObjectWithId()
		{
			// arrange
			var testAnswer = new Answer()
			{
				Content = "Test"
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				Answer retrievedAnswer = null;
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new AnswerRepository(ctx);
					retrievedAnswer = repo.CreateAnswer(testAnswer);
				}
				
				// assert
				Assert.NotNull(retrievedAnswer);
				Assert.Equal(testAnswer, retrievedAnswer);
				Assert.Equal(1, retrievedAnswer.AnswerId);
			}
		}
		
		[Fact]
		public void CreateAnswer_WithDuplicateObject_Throws()
		{
			// arrange
			var testAnswer = new Answer()
			{
				Content = "Test"
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				Action duplicateAdd = null;
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new AnswerRepository(ctx);
					repo.CreateAnswer(testAnswer);
					// this will throw an exception
					duplicateAdd = () => repo.CreateAnswer(testAnswer);
					
					// assert
					var exception = Assert.Throws<ArgumentException>(duplicateAdd);
					Assert.Equal("Answer already in database.", exception.Message);
				}
			}
		}

		[Fact]
		public void UpdateAnswer_WithValidObject_ReturnsObjectWithId()
		{
			// arrange
			var testAnswer = new Answer()
			{
				Content = "Test"
			};
			var newContent = "New Test";
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				Answer retrievedAnswer = null;
				using (var ctx = factory.CreateContext())
				{
					ctx.Answers.Add(testAnswer);
					ctx.SaveChanges();

					testAnswer.Content = newContent;
					
					// act
					var repo = new AnswerRepository(ctx);
					retrievedAnswer = repo.UpdateAnswer(testAnswer);
				}
				
				// assert
				Assert.NotNull(retrievedAnswer);
				Assert.Equal(testAnswer, retrievedAnswer);
				Assert.Equal(newContent, retrievedAnswer.Content);
			}
		}

		[Fact]
		public void UpdateAnswer_WithInvalidId_Throws()
		{
			// arrange
			var testAnswer = new Answer()
			{
				Content = "Test",
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				using (var ctx = factory.CreateContext())
				{
					ctx.Answers.Add(testAnswer);
					ctx.SaveChanges();

					testAnswer.AnswerId = 0;

					// act
					var repo = new AnswerRepository(ctx);
					Action result = () => repo.UpdateAnswer(testAnswer);

					// assert
					var exception = Assert.Throws<ArgumentException>(result);
					Assert.Equal("Answer to update not found.", exception.Message);
				}
			}
		}

		[Fact]
		public void DeleteAnswer_WithValidId_DeletesAnswer()
		{
			// arrange
			var testAnswer = new Answer()
			{
				Content = "Test",
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				Answer retrievedAnswer0 = null;
				Answer retrievedAnswer1 = null;
				using (var ctx = factory.CreateContext())
				{
					ctx.Answers.Add(testAnswer);
					ctx.SaveChanges();
					
					// act
					var id = ctx.Answers.FirstOrDefault().AnswerId;
					var repo = new AnswerRepository(ctx);
					retrievedAnswer0 = repo.DeleteAnswer(id);
					retrievedAnswer1 = repo.ReadAnswer(id);
				}
				
				// assert
				Assert.NotNull(retrievedAnswer0);
				Assert.Equal(testAnswer, retrievedAnswer0);
				Assert.Null(retrievedAnswer1);
			}
		}

		[Fact]
		public void DeleteAnswer_WithInvalidId_Throws()
		{
			// arrange
			var invalidId = 0;
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new AnswerRepository(ctx);
					Action result = () => repo.DeleteAnswer(invalidId);
					
					// assert
					var exception = Assert.Throws<ArgumentException>(result);
					Assert.Equal("Answer to delete not found.", exception.Message);
				}
			}
		}
	}
}