//using System;
//using System.Collections.Generic;
//using System.Linq;
//using COI.BL.Domain.Ideation;
//using COI.BL.Domain.Questionnaire;
//using COI.DAL.Ideation.EF;
//using COI.DAL.Questionnaire.EF;
//using Xunit;
//
//namespace COI.DAL.Tests.Questionnaire
//{
//	public class QuestionRepositoryTests
//	{
//		[Fact]
//		public void ReadQuestions_WithNoQuestionsInDb_ReturnsEmptyList()
//		{
//			// arrange
//			using (var factory = new CityOfIdeasDbContextFactory())
//			{
//				IEnumerable<Question> result = null;
//				using (var ctx = factory.CreateContext(false))
//				{
//					// act
//					var repo = new QuestionRepository(ctx);
//					result = repo.ReadQuestions().ToList();
//				}
//				
//				// assert
//				Assert.NotNull(result);
//				Assert.Empty(result);
//			}
//		}
//
//		[Fact]
//		public void ReadQuestions_WithQuestionsInDb_ReturnsListOfQuestions()
//		{
//			
//			// arrange
//			using (var factory = new CityOfIdeasDbContextFactory())
//			{
//				IEnumerable<Question> result = null;
//				using (var ctx = factory.CreateContext(true))
//				{
//					// act
//					var repo = new QuestionRepository(ctx);
//					result = repo.ReadQuestions().ToList();
//				}
//				
//				// assert
//				Assert.NotNull(result);
//				Assert.Equal(4, result.Count());
//			}
//		}
//
//		[Fact]
//		public void ReadOpenQuestion_WithInvalidId_ReturnsNull()
//		{
//			// arrange
//			using (var factory = new CityOfIdeasDbContextFactory())
//			{
//				const int id = 0;
//				OpenQuestion result = null;
//				using (var ctx = factory.CreateContext(false))
//				{
//					// act
//					var repo = new QuestionRepository(ctx);
//					result = repo.ReadOpenQuestion(id);
//				}
//				
//				// assert
//				Assert.Null(result);
//			}
//		}
//
//		[Fact]
//		public void ReadOpenQuestion_WithValidId_ReturnsOpenQuestion()
//		{
//			// arrange
//			using (var factory = new CityOfIdeasDbContextFactory())
//			{
//				const int id = 4;
//				OpenQuestion result = null;
//				using (var ctx = factory.CreateContext(true))
//				{
//					// act
//					var repo = new QuestionRepository(ctx);
//					result = repo.ReadOpenQuestion(id);
//					
//					// assert
//					Assert.NotNull(result);
//					Assert.Equal(id, result.QuestionId);
//					Assert.IsAssignableFrom<OpenQuestion>(result);
//					Assert.Equal("Wat vindt u van het design van onze website?", result.Inquiry);
//					Assert.Empty(result.Answers);
//				}
//			}
//		}
//		
//		[Fact]
//		public void ReadOpenQuestion_WithChoiceId_ReturnsNull()
//		{
//			// arrange
//			using (var factory = new CityOfIdeasDbContextFactory())
//			{
//				const int id = 1;
//				OpenQuestion result = null;
//				using (var ctx = factory.CreateContext(true))
//				{
//					// act
//					var repo = new QuestionRepository(ctx);
//					result = repo.ReadOpenQuestion(id);
//					
//					// assert
//					Assert.Null(result);
//				}
//			}
//		}
//		
//		[Fact]
//		public void ReadChoice_WithInvalidId_ReturnsNull()
//		{
//			// arrange
//			using (var factory = new CityOfIdeasDbContextFactory())
//			{
//				const int id = 0;
//				Choice result = null;
//				using (var ctx = factory.CreateContext(false))
//				{
//					// act
//					var repo = new QuestionRepository(ctx);
//					result = repo.ReadChoice(id);
//				}
//				
//				// assert
//				Assert.Null(result);
//			}
//		}
//
//		[Fact]
//		public void ReadChoice_WithValidId_ReturnsChoice()
//		{
//			// arrange
//			using (var factory = new CityOfIdeasDbContextFactory())
//			{
//				const int id = 1;
//				using (var ctx = factory.CreateContext(true))
//				{
//					// act
//					var repo = new QuestionRepository(ctx);
//					Choice result = repo.ReadChoice(id);
//					
//					// assert
//					Assert.NotNull(result);
//					Assert.Equal(id, result.QuestionId);
//					Assert.IsAssignableFrom<Choice>(result);
//					Assert.Equal("Hoe heeft u onze website gevonden?", result.Inquiry);
//					Assert.True(result.IsMultipleChoice);
//					Assert.Equal(4, result.Options.Count);
//				}
//			}
//		}
//		
//		[Fact]
//		public void ReadChoice_WithOpenQuestionId_ReturnsNull()
//		{
//			// arrange
//			using (var factory = new CityOfIdeasDbContextFactory())
//			{
//				const int id = 4;
//				using (var ctx = factory.CreateContext(true))
//				{
//					// act
//					var repo = new QuestionRepository(ctx);
//					Choice result = repo.ReadChoice(id);
//					
//					// assert
//					Assert.Null(result);
//				}
//			}
//		}
//		
//		[Fact]
//		public void ReadOption_WithInvalidId_ReturnsNull()
//		{
//			// arrange
//			using (var factory = new CityOfIdeasDbContextFactory())
//			{
//				const int id = 0;
//				Option result = null;
//				using (var ctx = factory.CreateContext(false))
//				{
//					// act
//					var repo = new QuestionRepository(ctx);
//					result = repo.ReadOption(id);
//				}
//				
//				// assert
//				Assert.Null(result);
//			}
//		}
//
//		[Fact]
//		public void ReadOption_WithValidId_ReturnsOption()
//		{
//			// arrange
//			using (var factory = new CityOfIdeasDbContextFactory())
//			{
//				const int id = 1;
//				using (var ctx = factory.CreateContext(true))
//				{
//					Option result = null;
//					
//					// act
//					var repo = new QuestionRepository(ctx);
//					result = repo.ReadOption(id);
//					
//					// assert
//					Assert.NotNull(result);
//					Assert.Equal(id, result.OptionId);
//					Assert.Equal("Zoekmachine", result.Content);
//					Assert.Empty(result.Answers);
//				}
//			}
//		}
//
//		[Fact]
//		public void UpdateQuestion_WithChoice_UpdatesQuestion()
//		{
//			// arrange
//			using (var factory = new CityOfIdeasDbContextFactory())
//			{
//				const int id = 2;
//				String newInquiry = "New Inquiry";
//				bool newIsMultipleChoice = true;
//				
//				// act
//				using (var ctx = factory.CreateContext(true))
//				{
//					var repo = new QuestionRepository(ctx);
//					Choice question = repo.ReadChoice(id);
//					question.Inquiry = newInquiry;
//					question.IsMultipleChoice = newIsMultipleChoice;
//					
//					repo.UpdateQuestion(question);
//				}
//				
//				// assert
//				using (var ctx = factory.CreateContext(false))
//				{
//					Choice result = null;
//					
//					var repo = new QuestionRepository(ctx);
//					result = repo.ReadChoice(id);
//					
//					Assert.NotNull(result);
//					Assert.Equal(id, result.QuestionId);
//					Assert.Equal(newInquiry, result.Inquiry);
//					Assert.Equal(2, result.Options.Count);
//				}
//			}
//		}
//		
//		[Fact]
//		public void UpdateQuestion_WithOpenQuestion_UpdatesQuestion()
//		{
//			// arrange
//			using (var factory = new CityOfIdeasDbContextFactory())
//			{
//				const int id = 4;
//				String newInquiry = "New Inquiry";
//				
//				// act
//				using (var ctx = factory.CreateContext(true))
//				{
//					var repo = new QuestionRepository(ctx);
//					OpenQuestion question = repo.ReadOpenQuestion(id);
//					question.Inquiry = newInquiry;
//					
//					repo.UpdateQuestion(question);
//				}
//				
//				// assert
//				using (var ctx = factory.CreateContext(false))
//				{
//					OpenQuestion result = null;
//					
//					var repo = new QuestionRepository(ctx);
//					result = repo.ReadOpenQuestion(id);
//					
//					Assert.NotNull(result);
//					Assert.Equal(id, result.QuestionId);
//					Assert.Equal(newInquiry, result.Inquiry);
//				}
//			}
//		}
//		
//		[Fact]
//		public void UpdateOption_WithModifiedValues_UpdatesOption()
//		{
//			// arrange
//			using (var factory = new CityOfIdeasDbContextFactory())
//			{
//				const int id = 2;
//				const String newContent = "New Content";
//				
//				// act
//				using (var ctx = factory.CreateContext(true))
//				{
//					var repo = new QuestionRepository(ctx);
//					Option result = repo.ReadOption(id);
//					result.Content = newContent;
//					
//					repo.UpdateOption(result);
//				}
//				
//				// assert
//				using (var ctx = factory.CreateContext(false))
//				{
//					Option result = null;
//					
//					var repo = new QuestionRepository(ctx);
//					result = repo.ReadOption(id);
//					
//					Assert.NotNull(result);
//					Assert.Equal(id, result.OptionId);
//					Assert.Equal(newContent, result.Content);
//					Assert.Equal(1, result.Choice.QuestionId);
//				}
//			}
//		}
//	}
//
//}