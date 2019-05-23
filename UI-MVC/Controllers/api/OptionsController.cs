using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using COI.BL;
using COI.BL.Domain.Questionnaire;
using COI.BL.Questionnaire;
using COI.UI.MVC.Authorization;
using COI.UI.MVC.Models;
using COI.UI.MVC.Models.DTO.Questionnaire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers.api
{
    [Authorize(Policy = AuthConstants.UserInOrgOrSuperadminPolicy)]
    [Authorize(AuthenticationSchemes = AuthenticationConstants.AuthSchemes)]
	[ApiController]
	[Route("api/{orgId}/[controller]")]
	public class OptionsController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly IQuestionnaireManager _questionnaireManager;
		private readonly IUnitOfWorkManager _unitOfWorkManager;

		public OptionsController(IMapper mapper, IQuestionnaireManager questionnaireManager, IUnitOfWorkManager unitOfWorkManager)
		{
			_mapper = mapper;
			_questionnaireManager = questionnaireManager;
			_unitOfWorkManager = unitOfWorkManager;
		}

		[AllowAnonymous]
		[HttpGet("{id}")]
		public IActionResult GetOption(int id)
		{
			try
			{
				var option = _questionnaireManager.GetOption(id);
				if (option == null)
				{
					return NotFound("Option not found.");
				}
				return Ok(_mapper.Map<OptionDto>(option));
			}
			catch (Exception e)
			{
				return BadRequest($"Something went wrong in getting the option: {e.Message}.");
			}
		}
		
		[AllowAnonymous]
		[HttpGet("{id}/Answers")]
		public IActionResult GetAnswersForOption(int id)
		{
			var answers = _questionnaireManager.GetAnswersForOption(id);
			var response = _mapper.Map<List<AnswerDto>>(answers);

			return Ok(response);
		}

		[HttpPost]
		public IActionResult PostNewOption(NewOptionDto newOption, [FromRoute] string orgId)
		{
			try
			{
				// UoW is started here to make sure the request either fails or succeeds fully
				_unitOfWorkManager.StartUnitOfWork();
				Option createdOption = _questionnaireManager.AddOption(
					newOption.Content, newOption.QuestionId);
				_unitOfWorkManager.EndUnitOfWork();

				return CreatedAtAction(
					"GetOption",
					new {orgId, id = createdOption.OptionId},
					_mapper.Map<OptionMinDto>(createdOption));
			}
			catch (ValidationException ve)
			{
				return UnprocessableEntity($"Invalid input data: {ve.ValidationResult.ErrorMessage}");
			}
			catch (Exception e)
			{
				return BadRequest($"Something went wrong in creation the option: {e.Message}");
			}
		}

		[HttpPut("{id}")]
		public IActionResult UpdateOption(int id, NewOptionDto updatedValues)
		{
			try
			{
				_unitOfWorkManager.StartUnitOfWork();
				Option updatedOption = _questionnaireManager.ChangeOption(
					id,
					updatedValues.Content,
					updatedValues.QuestionId);
				_unitOfWorkManager.EndUnitOfWork();

				if (updatedOption == null)
				{
					return BadRequest("Something went wrong while updating the option.");
				}

				return Ok(_mapper.Map<OptionMinDto>(updatedOption));
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
					case "questionId":
						return UnprocessableEntity(e.Message);
					default:
						return BadRequest(e.Message);
				}
			}
		}
	}
}