using COI.BL.Domain.Ideation;
using COI.BL.Domain.User;
using Xunit;

namespace COI.BL.Domain.Tests.Ideation
{
	public class CommentTests
	{
		[Fact]
		public void GetScore_WithoutVotes_ReturnsZero()
		{
			// arrange
			var comment = new Comment();
			
			// act
			var score = comment.GetScore();
			
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
			var comment = new Comment();
			
			// act
			foreach (int val in vals)
			{
				comment.Votes.Add(new Vote() { Value = val });
			}
			
			// assert
			var score = comment.GetScore();
			Assert.Equal(expected, score);
		}
	}
}