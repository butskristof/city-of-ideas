using System;
using COI.BL.Domain.Questionnaire;
using COI.DAL.Questionnaire.EF;
using Xunit;

namespace COI.DAL.Tests.Questionnaire
{
	public class AnswerEfRepositoryTests
	{
		[Fact]
		public void CreateAnswer_WithAnswer_ReturnsAnswerWithId()
		{
			// arrange
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				const int expectedId = 1;
				const String newContent = "Answer content";
				Answer newAnswer = new Answer()
				{
					Content = newContent
				};
				Answer returnedAnswer = null;
				using (var ctx = factory.CreateContext(false))
				{
					// act
					var repo = new AnswerRepository(ctx);
					returnedAnswer = repo.CreateAnswer(newAnswer);
				}

				// assert
				Assert.NotNull(returnedAnswer);
				Assert.NotEqual(0, returnedAnswer.AnswerId);
				Assert.Equal(expectedId, returnedAnswer.AnswerId);
				Assert.Equal(newContent, returnedAnswer.Content);
			}
		}
	}
}