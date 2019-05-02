using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using COI.BL;
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
		private readonly IUnitOfWorkManager _unitOfWorkManager;

		public IdeasController(IMapper mapper, IIdeationManager ideationManager, ICityOfIdeasController coiCtrl, IUnitOfWorkManager unitOfWorkManager)
		{
			_mapper = mapper;
			_ideationManager = ideationManager;
			_coiCtrl = coiCtrl;
			_unitOfWorkManager = unitOfWorkManager;
		}

		[AllowAnonymous]
		[HttpGet]
		public IActionResult GetIdeas()
		{
			var ideas = _ideationManager.GetIdeas().ToList();
			var response = _mapper.Map<List<IdeaDto>>(ideas);
			
			return Ok(response);
		}
		
		[AllowAnonymous]
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
				_unitOfWorkManager.StartUnitOfWork();
				Idea createdIdea = _ideationManager.AddIdea(
					idea.Title, 
					fields, 
					idea.IdeationId);
				_unitOfWorkManager.EndUnitOfWork();
				
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
				_unitOfWorkManager.StartUnitOfWork();
				Idea updatedIdea = _ideationManager.ChangeIdea(
					id, 
					updatedValues.Title, 
					updatedValues.IdeationId);
				_unitOfWorkManager.EndUnitOfWork();

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
				_unitOfWorkManager.StartUnitOfWork();
				Idea deleted = _ideationManager.RemoveIdea(id);
				_unitOfWorkManager.EndUnitOfWork();
				
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

		[AllowAnonymous]
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
		
		[AllowAnonymous]
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