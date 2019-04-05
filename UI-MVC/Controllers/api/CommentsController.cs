using System;
using COI.BL.Application;
using COI.BL.Domain.User;
using COI.BL.Ideation;
using COI.UI.MVC.Models.DTO.Ideation;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers.api
{
	[ApiController]
	[Route("api/[controller]")]
	public class CommentsController : ControllerBase
	{
//		private readonly IMapper _mapper;
		private readonly IIdeationManager _ideationManager;
		private readonly ICityOfIdeasController _coiCtrl;

		public CommentsController(IIdeationManager ideationManager, ICityOfIdeasController coiCtrl)
		{
			_ideationManager = ideationManager;
			_coiCtrl = coiCtrl;
		}
		
		// GET: api/Comments/{id}
		[HttpGet("{id}")]
		public IActionResult GetComment(int id)
		{
			try
			{
				var comment = _ideationManager.GetComment(id);
				return Ok(comment);
			}
			catch (ArgumentException e)
			{
				return BadRequest("Comment not found.");
			}
		}

		// GET: api/Comments/{id}/Score
		[HttpGet("{id}/Score")]
		public IActionResult GetCommentScore(int id)
		{
			try
			{
				var score = _ideationManager.GetCommentScore(id);
				return Ok(score);
			}
			catch (ArgumentException e)
			{
				return BadRequest("Comment not found.");
			}
		}
		
		// POST: api/Comments/Vote
		[HttpPost("Vote")]
		public IActionResult PostCommentVote(NewCommentVoteDto vote)
		{
			try
			{
				Vote createdVote = _coiCtrl.AddVoteToComment(vote.UserId, vote.CommentId, vote.Value);
				return CreatedAtAction("GetCommentScore", new {id = createdVote.Comment.CommentId}, createdVote.Comment.GetScore());
			}
			catch (ArgumentException e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}