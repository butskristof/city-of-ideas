using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using COI.BL.Application;
using COI.BL.Domain.Ideation;
using COI.BL.Domain.User;
using COI.BL.Ideation;
using COI.UI.MVC.Models;
using COI.UI.MVC.Models.DTO.Ideation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers.api
{
    [Authorize(AuthenticationSchemes = JwtConstants.AuthSchemes)]
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
		
		[HttpGet]
		public IActionResult GetIdeas()
		{
			var ideas = _ideationManager.GetIdeas().ToList();
			var response = _mapper.Map<List<IdeaDto>>(ideas);
			
			return Ok(response);
		}
		
		[HttpGet("{id}")]
		public IActionResult GetIdea(int id)
		{
			try
			{
				var idea = _ideationManager.GetIdea(id);
				if (idea == null)
				{
					return NotFound("Idea not found.");
				}
				return Ok(_mapper.Map<IdeaDto>(idea));
			}
			catch (Exception e)
			{
				return BadRequest($"Something went wrong in getting the idea: {e.Message}.");
			}
		}
		
		[HttpPost]
		public IActionResult PostNewIdea(NewIdeaDto idea)
		{
			try
			{
				var fields = _mapper.Map<List<Field>>(idea.Fields);
				Idea createdIdea = _ideationManager.AddIdea(
					idea.Title, 
					fields, 
					idea.IdeationId);
				
				return CreatedAtAction(
					"GetIdea", 
					new {id = createdIdea.IdeaId},
					_mapper.Map<IdeaDto>(createdIdea));
			}
			catch (ValidationException ve)
			{
				return UnprocessableEntity($"Invalid input data: {ve.ValidationResult.ErrorMessage}");
			}
			catch (Exception e)
			{
				return BadRequest($"Something went wrong in creating the idea: {e.Message}.");
			}
		}
		
		[HttpPut("{id}")]
		public IActionResult UpdateIdea(int id, NewIdeaDto updatedValues)
		{
			try
			{
				Idea updatedIdea = _ideationManager.ChangeIdea(
					id, 
					updatedValues.Title, 
					updatedValues.IdeationId);

				if (updatedIdea == null)
				{
					return BadRequest("Something went wrong while updating the idea.");
				}

				return Ok(_mapper.Map<IdeaDto>(updatedIdea));
			}
			catch (ValidationException ve)
			{
				return UnprocessableEntity($"Invalid input data: {ve.ValidationResult.ErrorMessage}");
			}
			catch (ArgumentException e)
			{
				switch (e.ParamName)
				{
					case "id":
						return NotFound(e.Message);
					case "ideationId":
						return UnprocessableEntity(e.Message);
					default:
						return BadRequest(e.Message);
				}
			}
		}
		
		[HttpDelete("{id}")]
		public IActionResult DeleteIdea(int id)
		{
			try
			{
				Idea deleted = _ideationManager.RemoveIdea(id);
				if (deleted == null)
				{
					return BadRequest("Something went wrong while deleting the idea.");
				}

				return Ok(_mapper.Map<IdeaDto>(deleted));
			}
			catch (ArgumentException)
			{
				return NotFound("Idea to delete not found.");
			}
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
				return BadRequest(e.Message); // idea not found
			}
		}
		
		[HttpGet("{id}/Comments")]
		public IActionResult GetCommentsForIdea(int id)
		{
			var comments = _ideationManager.GetCommentsForIdea(id).ToList();
			var response = _mapper.Map<List<CommentDto>>(comments);
			
			return Ok(response);
		}
		
		// POST: api/Ideas/Vote
		[HttpPost("Vote")]
		public IActionResult PostIdeaVote(NewIdeaVoteDto vote)
		{
			try
			{
				Vote createdVote = _coiCtrl.AddVoteToIdea(
					vote.Value, 
					vote.UserId, 
					vote.IdeaId);

				// TODO update response
//				return CreatedAtAction();
				return Ok();
			}
			catch (ArgumentException e)
			{
				switch (e.ParamName)
				{
					case "ideaId":
						return UnprocessableEntity(e.Message);
					case "userId":
						return UnprocessableEntity(e.Message);
					default:
						return BadRequest(e.Message);
				}
			}
		}
	}
}