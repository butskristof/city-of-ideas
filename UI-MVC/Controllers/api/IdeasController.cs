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
using COI.BL.User;
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
    [Authorize(Policy = AuthConstants.UserInOrgOrSuperadminPolicy)]
    [Authorize(AuthenticationSchemes = AuthenticationConstants.AuthSchemes)]
	[ApiController]
	[Route("api/{orgId}/[controller]")]
	public class IdeasController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly IIdeationManager _ideationManager;
		private readonly IUserManager _userManager;
		private readonly IUnitOfWorkManager _unitOfWorkManager;
		private readonly IFileService _fileService;

		private readonly ICommentsHelper _commentsHelper;
		private readonly IIdeasHelper _ideasHelper;

		public IdeasController(IMapper mapper, IIdeationManager ideationManager, IUserManager userManager, IUnitOfWorkManager unitOfWorkManager, IFileService fileService, ICommentsHelper commentsHelper, IIdeasHelper ideasHelper)
		{
			_mapper = mapper;
			_ideationManager = ideationManager;
			_userManager = userManager;
			_unitOfWorkManager = unitOfWorkManager;
			_fileService = fileService;
			_commentsHelper = commentsHelper;
			_ideasHelper = ideasHelper;
		}

		[AllowAnonymous]
		[HttpGet]
		public IActionResult GetIdeas([FromQuery] string userId)
		{
			return Ok(_ideasHelper.GetIdeas(userId));
		}
		
		[AllowAnonymous]
		[HttpGet("{id}")]
		public IActionResult GetIdea(int id, [FromQuery] string userId)
		{
			try
			{
				return Ok(_ideasHelper.GetIdea(id, userId));
			}
			catch (ArgumentException e)
			{
				return NotFound(e.Message);
			}
			catch (Exception e)
			{
				return BadRequest($"Something went wrong in getting the idea: {e.Message}.");
			}
		}
		
		[HttpPost]
		public async Task<IActionResult> PostNewIdea([FromForm]NewIdeaDto idea, [FromRoute] string orgId)
		{
			if (idea.Texts.IsNullOrEmpty() && idea.Images.IsNullOrEmpty())
			{
				return BadRequest("Either images or text content should be provided.");
			}
			
			try
			{
				// UoW is started here to make sure the request either fails or succeeds fully
				_unitOfWorkManager.StartUnitOfWork();
				
				Idea createdIdea = _ideationManager.AddIdea(
					idea.Title, 
					idea.IdeationId);
				_userManager.AddIdeaToUser(createdIdea, idea.UserId);

				foreach (var video in idea.Videos)
				{
					string imgLocation = await _fileService.ConvertFileToLocation(video);
					_ideationManager.AddFieldToIdea(FieldType.Video, imgLocation, createdIdea.IdeaId);
				}

				foreach (var image in idea.Images)
				{
					string imgLocation = await _fileService.ConvertFileToLocation(image);
					_ideationManager.AddFieldToIdea(FieldType.Picture, imgLocation, createdIdea.IdeaId);
				}

				foreach (var textfield in idea.Texts)
				{
					_ideationManager.AddFieldToIdea(FieldType.Text, textfield, createdIdea.IdeaId);
				}
				
				foreach (var location in idea.Locations)
				{
					_ideationManager.AddFieldToIdea(FieldType.Location, location, createdIdea.IdeaId);
				}

				foreach (var link in idea.Links)
				{
					_ideationManager.AddFieldToIdea(FieldType.Link, link, createdIdea.IdeaId);
				}
				
				_unitOfWorkManager.EndUnitOfWork();
				
				return CreatedAtAction(
					"GetIdea", 
					new {orgId, id = createdIdea.IdeaId},
					_mapper.Map<IdeaMinDto>(createdIdea));
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

				return Ok(_mapper.Map<IdeaMinDto>(updatedIdea));
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

		[AllowAnonymous]
		[HttpGet("{id}/VoteCount")]
		public IActionResult GetIdeaScore(int id)
		{
			try
			{
				var score = _ideationManager.GetIdeaScore(id);
				return Ok(new VoteCountDto {VoteCount = score});
			}
			catch (ArgumentException e)
			{
				return BadRequest(e.Message); // idea not found
			}
		}
		
		[AllowAnonymous]
		[HttpGet("{id}/Comments")]
		public IActionResult GetCommentsForIdea(int id, [FromQuery] string userId)
		{
			return Ok(_commentsHelper.GetCommentsForIdea(id, userId));
		}
	}
}