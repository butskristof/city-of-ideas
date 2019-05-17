using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
	public class CommentsController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly IIdeationManager _ideationManager;
		private readonly ICityOfIdeasController _coiCtrl;
		private readonly IUnitOfWorkManager _unitOfWorkManager;
		private readonly IFileService _fileService;

		public CommentsController(IMapper mapper, IIdeationManager ideationManager, ICityOfIdeasController coiCtrl, IUnitOfWorkManager unitOfWorkManager, IFileService fileService)
		{
			_mapper = mapper;
			_ideationManager = ideationManager;
			_coiCtrl = coiCtrl;
			_unitOfWorkManager = unitOfWorkManager;
			_fileService = fileService;
		}

		[AllowAnonymous]
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
		public async Task<IActionResult> PostNewIdeaComment([FromForm] NewIdeaCommentDto comment)
		{
			if (comment.Texts.IsNullOrEmpty() && comment.Images.IsNullOrEmpty())
			{
				return BadRequest("Either images or text content should be provided.");
			}
			
			try
			{
//				var fields = _mapper.Map<List<Field>>(comment.Content);
				_unitOfWorkManager.StartUnitOfWork();
				
				Comment createdComment = _coiCtrl.AddCommentToIdea(
					comment.UserId, 
					comment.IdeaId);
				
				List<Field> fields = new List<Field>();

				foreach (var video in comment.Videos)
				{
					string imgLocation = await _fileService.ConvertFileToLocation(video);
					_ideationManager.AddFieldToComment(FieldType.Video, imgLocation, createdComment.CommentId);
				}

				foreach (var image in comment.Images)
				{
					string imgLocation = await _fileService.ConvertFileToLocation(image);
					_ideationManager.AddFieldToComment(FieldType.Picture, imgLocation, createdComment.CommentId);
				}

				foreach (var textfield in comment.Texts)
				{
					_ideationManager.AddFieldToComment(FieldType.Text, textfield, createdComment.CommentId);
				}
				
				_unitOfWorkManager.EndUnitOfWork();

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
					case "userId":
						return UnprocessableEntity(e.Message);
					default:
						return BadRequest(e.Message);
				}
			}
		}
		
		[HttpDelete("{id}")]
		public IActionResult DeleteComment(int id)
		{
			try
			{
				_unitOfWorkManager.StartUnitOfWork();
				Comment deleted = _ideationManager.RemoveComment(id);
				_unitOfWorkManager.EndUnitOfWork();
				
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

		[AllowAnonymous]
		[HttpGet("{id}/VoteCount")]
		public IActionResult GetCommentScore(int id)
		{
			try
			{
				var score = _ideationManager.GetCommentScore(id);
				return Ok(new VoteCountDto {VoteCount = score});
			}
			catch (ArgumentException e)
			{
				return NotFound(e.Message);
			}
		}
	}
}