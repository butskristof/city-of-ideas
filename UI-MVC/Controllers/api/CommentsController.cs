using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using COI.BL.Application;
using COI.BL.Domain.Ideation;
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
		private readonly IMapper _mapper;
		private readonly IIdeationManager _ideationManager;
		private readonly ICityOfIdeasController _coiCtrl;

		public CommentsController(IMapper mapper, IIdeationManager ideationManager, ICityOfIdeasController coiCtrl)
		{
			_mapper = mapper;
			_ideationManager = ideationManager;
			_coiCtrl = coiCtrl;
		}

		[HttpGet("{id}")]
		public IActionResult GetComment(int id)
		{
			try
			{
				var comment = _ideationManager.GetComment(id);
				if (comment == null)
				{
					return NotFound("Comment not found.");
				}
				return Ok(_mapper.Map<CommentDto>(comment));
			}
			catch (Exception e)
			{
				return BadRequest($"Something went wrong in getting the comment: {e.Message}.");
			}
		}
		
		[HttpPost]
		public IActionResult PostNewIdeaComment(NewIdeaCommentDto comment)
		{
			try
			{
				var fields = _mapper.Map<List<Field>>(comment.Content);
				Comment createdComment = _coiCtrl.AddCommentToIdea(
					fields, 
					comment.UserId, 
					comment.IdeaId);

				return CreatedAtAction(
					"GetComment", 
					new {id = createdComment.CommentId},
					_mapper.Map<CommentDto>(createdComment));
			}
			catch (ValidationException ve)
			{
				return UnprocessableEntity($"Invalid input data: {ve.ValidationResult.ErrorMessage}");
			}
			catch (ArgumentException e)
			{
				switch (e.ParamName)
				{
					case "ideaId":
						return UnprocessableEntity(e.Message);
						break;
					case "userId":
						return UnprocessableEntity(e.Message);
						break;
					default:
						return BadRequest(e.Message);
						break;
				}
			}
		}
		
		[HttpDelete("{id}")]
		public IActionResult DeleteComment(int id)
		{
			try
			{
				Comment deleted = _ideationManager.RemoveComment(id);
				if (deleted == null)
				{
					return NotFound("Comment to delete not found.");
				}

				return Ok(_mapper.Map<CommentDto>(deleted));
			}
			catch (ArgumentException)
			{
				return NotFound("Comment to delete not found.");
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
				Vote createdVote = _coiCtrl.AddVoteToComment(
					vote.Value, 
					vote.UserId, 
					vote.CommentId);

				// TODO update response
//				return CreatedAtAction();
				return Ok();
			}
			catch (ArgumentException e)
			{
				switch (e.ParamName)
				{
					case "commentId":
						return UnprocessableEntity(e.Message);
						break;
					case "userId":
						return UnprocessableEntity(e.Message);
						break;
					default:
						return BadRequest(e.Message);
						break;
				}
			}
		}
	}
}