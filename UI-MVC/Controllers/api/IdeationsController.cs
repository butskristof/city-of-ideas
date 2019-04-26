using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
	public class IdeationsController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly IIdeationManager _ideationManager;
		private readonly ICityOfIdeasController _coiCtrl;

		public IdeationsController(IMapper mapper, IIdeationManager ideationManager, ICityOfIdeasController coiCtrl)
		{
			_mapper = mapper;
			_ideationManager = ideationManager;
			_coiCtrl = coiCtrl;
		}

		[HttpGet]
		public IActionResult GetIdeations()
		{
			var ideations = _ideationManager.GetIdeations().ToList();
			var response = _mapper.Map<List<IdeationDto>>(ideations);
			
			return Ok(response);
		}
		
		[HttpGet("{id}/ideas")]
		public IActionResult GetIdeasForIdeation(int id)
		{
			var ideas = _ideationManager.GetIdeasForIdeation(id).ToList();
			var response = _mapper.Map<List<IdeaDto>>(ideas);
			
			return Ok(response);
		}
		
		[HttpGet("{id}")]
		public IActionResult GetIdeation(int id)
		{
			try
			{
				var ideation = _ideationManager.GetIdeation(id);
				if (ideation == null)
				{
					return NotFound("Ideation not found.");
				}
				return Ok(_mapper.Map<IdeationDto>(ideation));
			}
			catch (Exception e)
			{
				return BadRequest($"Something went wrong in getting the ideation: {e.Message}.");
			}
		}
		
		[HttpPost]
		public IActionResult PostNewIdeation(NewIdeationDto ideation)
		{
			try
			{
				Ideation createdIdeation = _ideationManager.AddIdeation(
					ideation.Title, 
					ideation.ProjectPhaseId);
				
				return CreatedAtAction(
					"GetIdeation", 
					new {id = createdIdeation.IdeationId},
					_mapper.Map<IdeationDto>(createdIdeation));
			}
			catch (ValidationException ve)
			{
				return UnprocessableEntity($"Invalid input data: {ve.ValidationResult.ErrorMessage}");
			}
			catch (Exception e)
			{
				return BadRequest($"Something went wrong in creating the ideation: {e.Message}.");
			}
		}
		
		[HttpPut("{id}")]
		public IActionResult UpdateIdeation(int id, NewIdeationDto updatedIdeationValues)
		{
			try
			{
				Ideation updatedIdeation = _ideationManager.ChangeIdeation(
					id, 
					updatedIdeationValues.Title, 
					updatedIdeationValues.ProjectPhaseId);

				if (updatedIdeation == null)
				{
					return BadRequest("Something went wrong while updating the ideation.");
				}

				return Ok(_mapper.Map<IdeationDto>(updatedIdeation));
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
						break;
					case "projectPhaseId":
						return UnprocessableEntity(e.Message);
						break;
					default:
						return BadRequest(e.Message);
						break;
				}
			}
		}
		
		[HttpDelete("{id}")]
		public IActionResult DeleteIdeation(int id)
		{
			try
			{
				Ideation deleted = _ideationManager.RemoveIdeation(id);
				if (deleted == null)
				{
					return BadRequest("Something went wrong while deleting the ideation.");
				}

				return Ok(_mapper.Map<IdeationDto>(deleted));
			}
			catch (ArgumentException)
			{
				return NotFound("Ideation to delete not found.");
			}
		}

		[HttpPost("Vote")]
		public IActionResult PostIdeationVote(NewIdeationVoteDto vote)
		{
			try
			{
				Vote createdVote = _coiCtrl.AddVoteToIdeation(
					vote.Value, 
					vote.UserId, 
					vote.IdeationId);
				
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