using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using COI.BL;
using COI.BL.Application;
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
	public class QuestionsController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly IQuestionnaireManager _questionnaireManager;
		private readonly IUnitOfWorkManager _unitOfWorkManager;

		public QuestionsController(IMapper mapper, IQuestionnaireManager questionnaireManager, IUnitOfWorkManager unitOfWorkManager)
		{
			_mapper = mapper;
			_questionnaireManager = questionnaireManager;
			_unitOfWorkManager = unitOfWorkManager;
		}

		[AllowAnonymous]
		[HttpGet("{id}")]
		public IActionResult GetQuestion(int id)
		{
			try
			{
				var question = _questionnaireManager.GetQuestion(id);
				if (question == null)
				{
					return NotFound("Question not found.");
				}
				return Ok(_mapper.Map<QuestionMinDto>(question));
			}
			catch (Exception e)
			{
				return BadRequest($"Something went wrong in getting the question: {e.Message}.");
			}
		}
		
		[AllowAnonymous]
		[HttpGet("{id}/Options")]
		public IActionResult GetOptionsForQuestion(int id)
		{
			var options = _questionnaireManager.GetOptionsForQuestion(id);
			var response = _mapper.Map<List<OptionMinDto>>(options);

			return Ok(response);
		}

		[AllowAnonymous]
		[HttpGet("{id}/Answers")]
		public IActionResult GetAnswersForQuestion(int id)
		{
			var answers = _questionnaireManager.GetAnswersForQuestion(id);
			var response = _mapper.Map<List<AnswerDto>>(answers);

			return Ok(response);
		}

		[HttpPost]
		public IActionResult PostNewQuestion(NewQuestionDto question, [FromRoute] string orgId)
		{
			try
			{
				_unitOfWorkManager.StartUnitOfWork();
				Question createdQuestion = _questionnaireManager.AddQuestion(
					question.Inquiry, 
					question.Required,
					question.QuestionType,
					question.QuestionnaireId);
				_unitOfWorkManager.EndUnitOfWork();
				
				return CreatedAtAction(
					"GetQuestion", 
					new {orgId, id = createdQuestion.QuestionId},
					_mapper.Map<QuestionMinDto>(createdQuestion));
			}
			catch (ValidationException ve)
			{
				return UnprocessableEntity($"Invalid input data: {ve.ValidationResult.ErrorMessage}");
			}
			catch (Exception e)
			{
				return BadRequest($"Something went wrong in creating the question: {e.Message}.");
			}
		}
		
		[HttpPut("{id}")]
		public IActionResult UpdateQuestion(int id, NewQuestionDto updatedValues)
		{
			try
			{
				_unitOfWorkManager.StartUnitOfWork();
				Question updatedQuestion = _questionnaireManager.ChangeQuestion(
					id, 
					updatedValues.Inquiry, 
					updatedValues.QuestionType,
					updatedValues.QuestionnaireId);
				_unitOfWorkManager.EndUnitOfWork();

				if (updatedQuestion == null)
				{
					return BadRequest("Something went wrong while updating the question.");
				}

				return Ok(_mapper.Map<QuestionMinDto>(updatedQuestion));
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
					case "questiontionId":
						return UnprocessableEntity(e.Message);
					default:
						return BadRequest(e.Message);
				}
			}
		}
	}
}