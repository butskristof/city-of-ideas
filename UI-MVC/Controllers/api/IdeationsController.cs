using System;
using System.Collections;
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
using COI.UI.MVC.Models.DTO.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers.api
{
    [Authorize(AuthenticationSchemes = JwtConstants.AuthSchemes)]
	[ApiController]
	[Route("api/[controller]")]
	public class IdeationsController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly IIdeationManager _ideationManager;
		private readonly ICityOfIdeasController _coiCtrl;
		private readonly IUnitOfWorkManager _unitOfWorkManager;

		public IdeationsController(IMapper mapper, IIdeationManager ideationManager, ICityOfIdeasController coiCtrl, IUnitOfWorkManager unitOfWorkManager)
		{
			_mapper = mapper;
			_ideationManager = ideationManager;
			_coiCtrl = coiCtrl;
			_unitOfWorkManager = unitOfWorkManager;
		}

		[AllowAnonymous]
		[HttpGet]
		public IActionResult GetIdeations()
		{
			var ideations = _ideationManager.GetIdeations().ToList();
			var response = _mapper.Map<List<IdeationMinDto>>(ideations);
			
			return Ok(response);
		}
		
		[AllowAnonymous]
		[HttpGet("{id}/ideas")]
		public IActionResult GetIdeasForIdeation(int id)
		{
			var ideas = _ideationManager.GetIdeasForIdeation(id).ToList();
			var response = _mapper.Map<List<IdeaMinDto>>(ideas);
			
			return Ok(response);
		}
		
		[AllowAnonymous]
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
//				var fields = _mapper.Map<List<Field>>(ideation.Fields);
				_unitOfWorkManager.StartUnitOfWork();
				Ideation createdIdeation = _ideationManager.AddIdeation(
					ideation.Title, 
					ideation.ProjectPhaseId);
				
//				List<Field> fields = new List<Field>();
				foreach (FieldDto field in ideation.Fields)
				{
					_ideationManager.AddFieldToIdeation(field.FieldType, field.Content, createdIdeation.IdeationId);
				}
				
				_unitOfWorkManager.EndUnitOfWork();
				
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
				_unitOfWorkManager.StartUnitOfWork();
				Ideation updatedIdeation = _ideationManager.ChangeIdeation(
					id, 
					updatedIdeationValues.Title, 
					updatedIdeationValues.ProjectPhaseId);
				_unitOfWorkManager.EndUnitOfWork();

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
					case "projectPhaseId":
						return UnprocessableEntity(e.Message);
					default:
						return BadRequest(e.Message);
				}
			}
		}
		
		[HttpDelete("{id}")]
		public IActionResult DeleteIdeation(int id)
		{
			try
			{
				_unitOfWorkManager.StartUnitOfWork();
				Ideation deleted = _ideationManager.RemoveIdeation(id);
				_unitOfWorkManager.EndUnitOfWork();
				
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

		[AllowAnonymous]
		[HttpGet("{id}/VoteCount")]
		public IActionResult GetIdeationScore(int id)
		{
			try
			{
				var score = _ideationManager.GetIdeationScore(id);
				return Ok(new VoteCountDto {VoteCount = score});
			}
			catch (ArgumentException e)
			{
				return BadRequest(e.Message); // ideation not found
			}
		}

//		[HttpPost("Vote")]
//		public IActionResult PostIdeationVote(NewIdeationVoteDto vote)
//		{
//			try
//			{
//				Vote createdVote = _coiCtrl.AddVoteToIdeation(
//					vote.Value, 
//					vote.UserId, 
//					vote.IdeationId);
//				
//				// TODO update response
////				return CreatedAtAction();
//				return Ok();
//			}
//			catch (ArgumentException e)
//			{
//				switch (e.ParamName)
//				{
//					case "commentId":
//						return UnprocessableEntity(e.Message);
//					case "userId":
//						return UnprocessableEntity(e.Message);
//					default:
//						return BadRequest(e.Message);
//				}
//			}
//		}
	}
}