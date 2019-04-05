using System;
using System.Collections.Generic;
using System.Linq;
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
	public class IdeasController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly IIdeationManager _ideationManager;
		private readonly ICityOfIdeasController _coiCtrl;

		public IdeasController(IIdeationManager ideationManager, IMapper mapper, ICityOfIdeasController coiCtrl)
		{
			_ideationManager = ideationManager;
			_mapper = mapper;
			_coiCtrl = coiCtrl;
		}

		// GET: api/Ideas/{id}/Score
		[HttpGet("{id}/Score")]
		public IActionResult GetIdeaScore(int id)
		{
			try
			{
				var score = _ideationManager.GetIdeaScore(id);
				return Ok(score);
			}
			catch (ArgumentException e)
			{
				return BadRequest("Idea not found.");
			}
		}
		
		// GET: api/Ideas/{id}/Comments
		[HttpGet("{id}/Comments")]
		public IActionResult GetCommentsForIdea(int id)
		{
			var comments = _ideationManager.GetCommentsForIdea(id);
			var response = new List<CommentDto>();
			
			foreach (Comment comment in comments.ToList())
			{
				response.Add(_mapper.Map<CommentDto>(comment));
			}

			return Ok(response);
		}
		
		// POST: api/Ideas/Vote
		[HttpPost("Vote")]
		public IActionResult PostIdeaVote(NewIdeaVoteDto vote)
		{
			try
			{
				Vote createdVote = _coiCtrl.AddVoteToIdea(vote.UserId, vote.IdeaId, vote.Value);
				return CreatedAtAction("GetIdeaScore", new {id = createdVote.Idea.IdeaId}, createdVote.Idea.GetScore());
			}
			catch (ArgumentException e)
			{
				return BadRequest(e.Message);
			}
		}
		
		// POST: api/Ideations/Comment
		[HttpPost("Comment")]
		public IActionResult PostNewComment(NewCommentDto comment)
		{
			try
			{
				var content = new List<Field>();
				foreach (FieldDto field in comment.Content)
				{
					content.Add(_mapper.Map<Field>(field));
				}
				
				Comment createdComment = _coiCtrl.AddComment(comment.UserId, comment.IdeaId, content);
				// TODO change to CreatedAtAction (no action for a single comment exists yet)

				return CreatedAtAction("GetComment", "Comments", new {id = createdComment.CommentId},
					_mapper.Map<CommentDto>(createdComment));
				return NoContent();
			}
			catch (ArgumentException e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}