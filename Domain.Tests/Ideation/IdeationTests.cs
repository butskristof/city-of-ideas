using Xunit;
using COI.BL.Domain.Ideation;
using COI.BL.Domain.User;

namespace COI.BL.Domain.Tests.Ideation
{
	public class IdeationTests
	{
		[Fact]
		public void GetScore_WithoutVotes_ReturnsZero()
		{
			// arrange
			var ideation = new Domain.Ideation.Ideation();
			
			// act
			var score = ideation.GetScore();
			
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
			var ideation = new Domain.Ideation.Ideation();
			
			// act
			foreach (int val in vals)
			{
				ideation.Votes.Add(new Vote() { Value = val });
			}
			
			// assert
			var score = ideation.GetScore();
			Assert.Equal(expected, score);
		}
	}
}