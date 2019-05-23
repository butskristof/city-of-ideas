using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using COI.BL;
using COI.BL.Domain.Ideation;
using COI.BL.Ideation;
using COI.UI.MVC.Authorization;
using COI.UI.MVC.Models;
using COI.UI.MVC.Models.DTO.Ideation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers.api
{
    [Authorize(Policy = AuthConstants.UserInOrgOrSuperadminPolicy)]
    [Authorize(AuthenticationSchemes = AuthenticationConstants.AuthSchemes)]
	[ApiController]
	[Route("api/{orgId}/[controller]")]
	public class FieldsController : Controller
	{
		private readonly IMapper _mapper;
		private readonly IIdeationManager _ideationManager;
		private readonly IUnitOfWorkManager _unitOfWorkManager;

		public FieldsController(IMapper mapper, IIdeationManager ideationManager, IUnitOfWorkManager unitOfWorkManager)
		{
			_mapper = mapper;
			_ideationManager = ideationManager;
			_unitOfWorkManager = unitOfWorkManager;
		}

		[AllowAnonymous]
		[HttpGet("{id}")]
		public IActionResult GetField(int id)
		{
			try
			{
				var field = _ideationManager.GetField(id);
				if (field == null)
				{
					return NotFound("Field not found.");
				}

				return Ok(_mapper.Map<FieldDto>(field));
			}
			catch (Exception e)
			{
				return BadRequest($"Something went wrong in getting the field: {e.Message}");
			}
		}

		[HttpPut("{id}")]
		public IActionResult UpdateField(int id, NewFieldDto updatedValues)
		{
			try
			{
				// UoW is started here to make sure the request either fails or succeeds fully
				_unitOfWorkManager.StartUnitOfWork();

				Field updatedField = null;
				if (updatedValues.IdeaId.HasValue && updatedValues.IdeaId != 0)
				{
					updatedField = _ideationManager.ChangeIdeaField(id, updatedValues.FieldType, updatedValues.Content,
						updatedValues.IdeaId.Value);
				}
				else if (updatedValues.IdeationId.HasValue && updatedValues.IdeationId != 0)
				{
					updatedField = _ideationManager.ChangeIdeationField(id, updatedValues.FieldType,
						updatedValues.Content, updatedValues.IdeationId.Value);
				}
				else if (updatedValues.CommentId.HasValue && updatedValues.CommentId != 0)
				{
					updatedField = _ideationManager.ChangeCommentField(id, updatedValues.FieldType,
						updatedValues.Content, updatedValues.CommentId.Value);
				}
				else if (updatedValues.ProjectId.HasValue && updatedValues.ProjectId != 0)
				{
					updatedField = _ideationManager.ChangeProjectField(id, updatedValues.FieldType,
						updatedValues.Content, updatedValues.ProjectId.Value);
				}
				else
				{
					return BadRequest("Idea, ideation or comment ID should be given.");
				}

				_unitOfWorkManager.EndUnitOfWork();

				if (updatedField == null)
				{
					return BadRequest("Something went wrong while updating the field.");
				}

				return Ok(_mapper.Map<FieldDto>(updatedField));

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
					case "ideaId":
						return UnprocessableEntity(e.Message);
					case "ideationId":
						return UnprocessableEntity(e.Message);
					case "commentId":
						return UnprocessableEntity(e.Message);
					default:
						return BadRequest(e.Message);
				}
			}
		}
    }
}