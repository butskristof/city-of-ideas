//using System.Collections.Generic;
//using System.Linq;
//using COI.BL.Domain.Ideation;
//using COI.DAL.Ideation.EF;
//using COI.DAL.Questionnaire.EF;
//using Xunit;
//
//namespace COI.DAL.Tests.Questionnaire
//{
//	public class QuestionnaireEfRepositoryTests
//	{
//		[Fact]
//		public void ReadQuestionnaires_WithNoQuestionnairesInDb_ReturnsEmptyList()
//		{
//			// arrange
//			using (var factory = new CityOfIdeasDbContextFactory())
//			{
//				IEnumerable<BL.Domain.Questionnaire.Questionnaire> questionnaires = null;
//				using (var ctx = factory.CreateContext(false))
//				{
//					// act
//					var repo = new QuestionnaireRepository(ctx);
//					questionnaires = repo.ReadQuestionnaires().ToList();
//				}
//				
//				// assert
//				Assert.NotNull(questionnaires);
//				Assert.Empty(questionnaires);
//			}
//		}
//
//		[Fact]
//		public void ReadQuestionnaires_WithQuestionnairesInDb_ReturnsListOfQuestionnaires()
//		{
//			// arrange
//			using (var factory = new CityOfIdeasDbContextFactory())
//			{
//				IEnumerable<BL.Domain.Questionnaire.Questionnaire> result = null;
//				using (var ctx = factory.CreateContext(true))
//				{
//					// act
//					var repo = new QuestionnaireRepository(ctx);
//					result = repo.ReadQuestionnaires().ToList();
//				}
//				
//				// assert
//				Assert.NotNull(result);
//				Assert.Single(result);
//			}
//		}
//
//		[Fact]
//		public void ReadQuestionnaire_WithInvalidId_ReturnsNull()
//		{
//			// arrange
//			using (var factory = new CityOfIdeasDbContextFactory())
//			{
//				const int id = 0;
//				BL.Domain.Questionnaire.Questionnaire result = null;
//				using (var ctx = factory.CreateContext(false))
//				{
//					// act
//					var repo = new QuestionnaireRepository(ctx);
//					result = repo.ReadQuestionnaire(id);
//				}
//				
//				// assert
//				Assert.Null(result);
//			}
//		}
//
//		[Fact]
//		public void ReadQuestionnaire_WithValidId_ReturnsQuestionnaire()
//		{
//			// arrange
//			using (var factory = new CityOfIdeasDbContextFactory())
//			{
//				const int id = 1;
//				BL.Domain.Questionnaire.Questionnaire result = null;
//				using (var ctx = factory.CreateContext(true))
//				{
//					// act
//					var repo = new QuestionnaireRepository(ctx);
//					result = repo.ReadQuestionnaire(id);
//					
//					// assert
//					Assert.NotNull(result);
//					Assert.Equal(id, result.QuestionnaireId);
//					Assert.Equal("Usability evaluation", result.Title);
//					Assert.NotNull(result.Description);
//					Assert.Equal(4, result.Questions.Count);
//				}
//			}
//		}
//	}
//}