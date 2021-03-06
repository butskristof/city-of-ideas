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
using COI.UI.MVC.Authorization;
using COI.UI.MVC.Helpers;
using COI.UI.MVC.Models;
using COI.UI.MVC.Models.DTO.Ideation;
using COI.UI.MVC.Models.DTO.User;
using COI.UI.MVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers.api
{
    [Authorize(Policy = AuthConstants.AdminPolicy)]
    [Authorize(AuthenticationSchemes = AuthenticationConstants.AuthSchemes)]
	[ApiController]
	[Route("api/{orgId}/[controller]")]
	public class IdeationsController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly IIdeationManager _ideationManager;
		private readonly IUnitOfWorkManager _unitOfWorkManager;
		private readonly IFileService _fileService;

		private readonly IIdeasHelper _ideasHelper;
		private readonly IIdeationsHelper _ideationsHelper;

		public IdeationsController(IMapper mapper, IIdeationManager ideationManager, IUnitOfWorkManager unitOfWorkManager, IFileService fileService, IIdeasHelper ideasHelper, IIdeationsHelper ideationsHelper)
		{
			_mapper = mapper;
			_ideationManager = ideationManager;
			_unitOfWorkManager = unitOfWorkManager;
			_fileService = fileService;
			_ideasHelper = ideasHelper;
			_ideationsHelper = ideationsHelper;
		}

		[AllowAnonymous]
		[HttpGet]
		public IActionResult GetIdeations([FromQuery] string userId)
		{
			return Ok(_ideationsHelper.GetIdeations(userId));
		}
		
		[AllowAnonymous]
		[HttpGet("{id}/ideas")]
		public IActionResult GetIdeasForIdeation(int id, [FromQuery] string userId)
		{
			return Ok(_ideasHelper.GetMinIdeas(userId, id));
		}
		
		[AllowAnonymous]
		[HttpGet("{id}")]
		public IActionResult GetIdeation(int id, [FromQuery] string userId)
		{
			try
			{
				return Ok(_ideationsHelper.GetIdeation(id, userId));
			}
			catch (ArgumentException e)
			{
				return NotFound(e.Message);
			}
			catch (Exception e)
			{
				return BadRequest($"Something went wrong in getting the ideation: {e.Message}.");
			}
		}
		
		[HttpPost]
		public async Task<IActionResult> PostNewIdeation([FromForm]NewIdeationDto ideation, [FromRoute] string orgId)
		{
			if (ideation.Texts.IsNullOrEmpty() && ideation.Images.IsNullOrEmpty())
			{
				return BadRequest("Either images or text content should be provided.");
			}
			
			try
			{
				// UoW is started here to make sure the request either fails or succeeds fully
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
				
				foreach (var location in ideation.Locations)
				{
					_ideationManager.AddFieldToIdeation(FieldType.Location, location, createdIdeation.IdeationId);
				}

				foreach (var link in ideation.Links)
				{
					_ideationManager.AddFieldToIdeation(FieldType.Link, link, createdIdeation.IdeationId);
				}
				
				_unitOfWorkManager.EndUnitOfWork();
				
				return CreatedAtAction(
					"GetIdeation", 
					new {orgId, id = createdIdeation.IdeationId},
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

		/// <summary>
		///  Open an ideation for new ideas
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost("{id}/Open")]
		public IActionResult OpenIdeation(int id)
		{
			return ChangeIdeationState(id, true);
		}
		
		/// <summary>
		/// Close an ideation for new ideas
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost("{id}/Close")]
		public IActionResult CloseIdeation(int id)
		{
			return ChangeIdeationState(id, false);
		}

		/// <summary>
		/// Helper for reducing duplicate code
		/// </summary>
		/// <param name="id"></param>
		/// <param name="newState"></param>
		/// <returns></returns>
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