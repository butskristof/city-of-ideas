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
using COI.BL.Domain.User;
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
	public class IdeasController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly IIdeationManager _ideationManager;
		private readonly ICityOfIdeasController _coiCtrl;
		private readonly IUnitOfWorkManager _unitOfWorkManager;
		private readonly IFileService _fileService;

		public IdeasController(IMapper mapper, IIdeationManager ideationManager, ICityOfIdeasController coiCtrl, IUnitOfWorkManager unitOfWorkManager, IFileService fileService)
		{
			_mapper = mapper;
			_ideationManager = ideationManager;
			_coiCtrl = coiCtrl;
			_unitOfWorkManager = unitOfWorkManager;
			_fileService = fileService;
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
		public async Task<IActionResult> PostNewIdea([FromForm]NewIdeaDto idea)
		{
			if (idea.Texts.IsNullOrEmpty() && idea.Images.IsNullOrEmpty())
			{
				return BadRequest("Either images or text content should be provided.");
			}
			
			try
			{
//				var fields = _mapper.Map<List<Field>>(idea.Fields);
				_unitOfWorkManager.StartUnitOfWork();
				
				Idea createdIdea = _ideationManager.AddIdea(
					idea.Title, 
					idea.IdeationId);

				List<Field> fields = new List<Field>();

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
				
				_unitOfWorkManager.EndUnitOfWork();
				
				return CreatedAtAction(
					"GetIdea", 
					new {id = createdIdea.IdeaId},
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
		public IActionResult GetCommentsForIdea(int id)
		{
			var comments = _ideationManager.GetCommentsForIdea(id).ToList();
			var response = _mapper.Map<List<CommentDto>>(comments);
			
			return Ok(response);
		}
		
		// POST: api/Ideas/Vote
//		[HttpPost("Vote")]
//		public IActionResult PostIdeaVote(NewIdeaVoteDto vote)
//		{
//			try
//			{
//				Vote createdVote = _coiCtrl.AddVoteToIdea(
//					vote.Value, 
//					vote.UserId, 
//					vote.IdeaId);
//
//				// TODO update response
////				return CreatedAtAction();
//				return Ok();
//			}
//			catch (ArgumentException e)
//			{
//				switch (e.ParamName)
//				{
//					case "ideaId":
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