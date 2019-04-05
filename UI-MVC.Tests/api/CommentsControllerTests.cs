using System;
using COI.BL.Application;
using COI.BL.Ideation;
using COI.UI.MVC.Controllers.api;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace COI.UI.MVC.Tests.api
{
	public class CommentsControllerTests
	{
		[Fact]
		public void GetCommentScore_WithValidIDs_GivesCorrectScore()
		{
			// arrange
			var commentId = 1;
			var score = 2;
			var ideationMgr = new Mock<IIdeationManager>();
			ideationMgr
				.Setup(x => x.GetCommentScore(commentId))
				.Returns(score);
			var coiCtrl = new Mock<ICityOfIdeasController>();
			var ctrl = new CommentsController(ideationMgr.Object, coiCtrl.Object);
			
			// act
			IActionResult result = ctrl.GetCommentScore(commentId);
			
			// assert
			Assert.IsType<OkObjectResult>(result);
			Assert.Equal(score, ((ObjectResult) result).Value);
		}

		[Fact]
		public void GetCommentScore_WithInvalidIDs_Throws()
		{
			// arrange
			var commentId = 1;
			var ideationMgr = new Mock<IIdeationManager>();
			ideationMgr
				.Setup(x => x.GetCommentScore(It.IsAny<int>()))
				.Throws<ArgumentException>();
			var coiCtrl = new Mock<ICityOfIdeasController>();
			var ctrl = new CommentsController(ideationMgr.Object, coiCtrl.Object);
			
			// act
			IActionResult result = ctrl.GetCommentScore(commentId);
			
			// assert
			Assert.IsType<BadRequestObjectResult>(result);
		}

//		[Fact]
//		public void PostCommentVote_WithValidValues_AddsVote()
//		{
//			// arrange
//			var commentid = 1;
//			var userId = 1;
//			var value = 1;
//			var ctrl = new CityOfIdeasController();
//		}
	}
}