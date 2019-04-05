namespace COI.UI.MVC.Tests.api
{
//	public class IdeasControllerTests
//	{
//		[Fact]
//		public void GetIdeaScore_WithValidIDs_GivesCorrectScore()
//		{
//			// arrange
//			var ideaId = 1;
//			var score = 2;
//			var ideationMgr = new Mock<IIdeationManager>();
//			ideationMgr
//				.Setup(x => x.GetIdeaScore(ideaId))
//				.Returns(score);
//			var mapper = new Mock<IMapper>();
//			var ctrl = new IdeasController(ideationMgr.Object, mapper.Object);
//			
//			// act
//			IActionResult result = ctrl.GetIdeaScore(ideaId);
//			
//			// assert
//			Assert.IsType<OkObjectResult>(result);
//			Assert.Equal(score, ((ObjectResult) result).Value);
//		}
//
//		[Fact]
//		public void GetIdeaScore_WithInvalidIDs_Throws()
//		{
//			// arrange
//			var ideaId = 1;
//			var ideationMgr = new Mock<IIdeationManager>();
//			ideationMgr
//				.Setup(x => x.GetIdeaScore(It.IsAny<int>()))
//				.Throws<ArgumentException>();
//			var mapper = new Mock<IMapper>();
//			var ctrl = new IdeasController(ideationMgr.Object, mapper.Object);
//			
//			// act
//			IActionResult result = ctrl.GetIdeaScore(ideaId);
//			
//			// assert
//			Assert.IsType<BadRequestObjectResult>(result);
//		}
//	}
}