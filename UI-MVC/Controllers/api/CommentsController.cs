using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
	public class CommentsController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly IIdeationManager _ideationManager;
		private readonly IUserManager _userManager;
		private readonly ICityOfIdeasController _coiCtrl;
		private readonly IUnitOfWorkManager _unitOfWorkManager;
		private readonly IFileService _fileService;

		private readonly ICommentsHelper _commentsHelper;

		public CommentsController(IMapper mapper, IIdeationManager ideationManager, IUserManager userManager, ICityOfIdeasController coiCtrl, IUnitOfWorkManager unitOfWorkManager, IFileService fileService, ICommentsHelper commentsHelper)
		{
			_mapper = mapper;
			_ideationManager = ideationManager;
			_userManager = userManager;
			_coiCtrl = coiCtrl;
			_unitOfWorkManager = unitOfWorkManager;
			_fileService = fileService;
			_commentsHelper = commentsHelper;
		}

		[AllowAnonymous]
		[HttpGet("{id}")]
		public IActionResult GetComment(int id, [FromQuery] string userId)
		{
			try
			{
				return Ok(_commentsHelper.GetComment(id, userId));
			}
			catch (ArgumentException e)
			{
				return NotFound(e.Message);
			}
			catch (Exception e)
			{
				return BadRequest($"Something went wrong in getting the comment: {e.Message}.");
			}
		}
		
		[HttpPost]
		public async Task<IActionResult> PostNewIdeaComment([FromForm] NewIdeaCommentDto comment, [FromRoute] string orgId)
		{
			if (comment.Texts.IsNullOrEmpty() && comment.Images.IsNullOrEmpty())
			{
				return BadRequest("Either images or text content should be provided.");
			}
			
			try
			{
				// UoW is started here to make sure the request either fails or succeeds fully
				_unitOfWorkManager.StartUnitOfWork();
				
				Comment createdComment = _coiCtrl.AddCommentToIdea(
					comment.UserId, 
					comment.IdeaId);
				
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
				
				foreach (var location in comment.Locations)
				{
					_ideationManager.AddFieldToComment(FieldType.Location, location, createdComment.CommentId);
				}

				foreach (var link in comment.Links)
				{
					_ideationManager.AddFieldToComment(FieldType.Link, link, createdComment.CommentId);
				}
				
				_unitOfWorkManager.EndUnitOfWork();

				return CreatedAtAction(
					"GetComment", 
					new {orgId, id = createdComment.CommentId},
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

		[AllowAnonymous]
		[HttpGet("{id}/uservote")]
		public IActionResult GetUserVoteValueForComment(int id, [FromQuery] string userId)
		{
			return Ok(new
			{
				value = _commentsHelper.GetUserVoteValueForComment(id, userId)
			});
		}
	}
}