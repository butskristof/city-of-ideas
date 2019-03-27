using COI.BL.Domain.Ideation;
using COI.BL.Domain.User;
using Xunit;

namespace COI.BL.Domain.Tests.Ideation
{
	public class IdeaTests
	{
		[Fact]
		public void GetScore_WithoutVotes_ReturnsZero()
		{
			// arrange
			var idea = new Idea();
			
			// act
			var score = idea.GetScore();
			
			// assert
			Assert.Equal(0, score);
		}
		
		[Theory]
		[InlineData( new[] {1, 1, 1}, 3 )]
		[InlineData( new[] {1, -1, -1}, -1 )]
		public void GetScore_WithVotes_ReturnsCorrectScore(
			int[] vals, int expected
		)
		{
			// arrange
			var idea = new Idea();
			
			// act
			foreach (int val in vals)
			{
				idea.Votes.Add(new Vote() { Value = val });
			}
			
			// assert
			var score = idea.GetScore();
			Assert.Equal(expected, score);
		}
	}
}