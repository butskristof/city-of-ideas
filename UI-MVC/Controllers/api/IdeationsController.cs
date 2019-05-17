using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Castle.Core.Internal;
using COI.BL;
using COI.BL.Application;
using COI.BL.Domain.Ideation;
using COI.BL.Ideation;
using COI.UI.MVC.Models;
using COI.UI.MVC.Models.DTO.Ideation;
using COI.UI.MVC.Models.DTO.User;
using COI.UI.MVC.Services;
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
		private readonly IUnitOfWorkManager _unitOfWorkManager;
		private readonly IFileService _fileService;

		public IdeationsController(IMapper mapper, IIdeationManager ideationManager, IUnitOfWorkManager unitOfWorkManager, IFileService fileService)
		{
			_mapper = mapper;
			_ideationManager = ideationManager;
			_unitOfWorkManager = unitOfWorkManager;
			_fileService = fileService;
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
		public async Task<IActionResult> PostNewIdeation([FromForm]NewIdeationDto ideation)
		{
			if (ideation.Texts.IsNullOrEmpty() && ideation.Images.IsNullOrEmpty())
			{
				return BadRequest("Either images or text content should be provided.");
			}
			
			try
			{
				_unitOfWorkManager.StartUnitOfWork();
				
				Ideation createdIdeation = _ideationManager.AddIdeation(
					ideation.Title, 
					ideation.ProjectPhaseId);
				
				foreach (var video in ideation.Videos)
				{
					string imgLocation = await _fileService.ConvertFileToLocation(video);
					_ideationManager.AddFieldToIdeation(FieldType.Video, imgLocation, createdIdeation.IdeationId);
				}
				
				foreach (var image in ideation.Images)
				{
					string imgLocation = await _fileService.ConvertFileToLocation(image);
					_ideationManager.AddFieldToIdeation(FieldType.Picture, imgLocation, createdIdeation.IdeationId);
				}

				foreach (var textfield in ideation.Texts)
				{
					_ideationManager.AddFieldToIdeation(FieldType.Text, textfield, createdIdeation.IdeationId);
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

		[HttpPost("{id}/Open")]
		public IActionResult OpenIdeation(int id)
		{
			return ChangeIdeationState(id, true);
		}
		
		[HttpPost("{id}/Close")]
		public IActionResult CloseIdeation(int id)
		{
			return ChangeIdeationState(id, false);
		}

		private IActionResult ChangeIdeationState(int id, bool newState)
		{
			try
			{
				_unitOfWorkManager.StartUnitOfWork();
				Ideation updatedIdeation = _ideationManager.ChangeIdeationState(id, newState);
				_unitOfWorkManager.EndUnitOfWork();

				if (updatedIdeation == null)
				{
					return BadRequest("Something went wrong while updating the ideation state.");
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
					default:
						return BadRequest(e.Message);
				}
			}
		}
	}
}