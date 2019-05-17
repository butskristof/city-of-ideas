using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using COI.BL;
using COI.BL.Domain.Questionnaire;
using COI.BL.Questionnaire;
using COI.UI.MVC.Models;
using COI.UI.MVC.Models.DTO.Questionnaire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers.api
{
    [Authorize(AuthenticationSchemes = JwtConstants.AuthSchemes)]
	[ApiController]
	[Route("api/[controller]")]
	public class QuestionnairesController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly IQuestionnaireManager _questionnaireManager;
		private readonly IUnitOfWorkManager _unitOfWorkManager;

		public QuestionnairesController(IMapper mapper, IQuestionnaireManager questionnaireManager, IUnitOfWorkManager unitOfWorkManager)
		{
			_mapper = mapper;
			_questionnaireManager = questionnaireManager;
			_unitOfWorkManager = unitOfWorkManager;
		}

		[AllowAnonymous]
		[HttpGet("{id}/Questions")]
		public IActionResult GetQuestionsForQuestionnaire(int id)
		{
			var questions = _questionnaireManager.GetQuestionsForQuestionnaire(id).ToList();
			var response = _mapper.Map<List<QuestionMinDto>>(questions);

			return Ok(response);
		}
		
		[AllowAnonymous]
		[HttpGet("{id}")]
		public IActionResult GetQuestionnaire(int id)
		{
			try
			{
				var questionnaire = _questionnaireManager.GetQuestionnaire(id);
				if (questionnaire == null)
				{
					return NotFound("Questionnaire not found.");
				}
				
				return Ok(_mapper.Map<QuestionnaireDto>(questionnaire));
			}
			catch (Exception e)
			{
				return BadRequest($"Something went wrong in getting the questionnaire: {e.Message}.");
			}
		}
		
		[HttpPost]
		public IActionResult PostNewQuestionnaire(NewQuestionnaireDto newQuestionnaire)
		{
			try
			{
				_unitOfWorkManager.StartUnitOfWork();
				Questionnaire questionnaire = _questionnaireManager.AddQuestionnaire(newQuestionnaire.Title, newQuestionnaire.Description, newQuestionnaire.ProjectPhaseId);
				_unitOfWorkManager.EndUnitOfWork();

				return CreatedAtAction(
					"GetQuestionnaire", 
					new {id = questionnaire.QuestionnaireId},
					_mapper.Map<QuestionnaireDto>(questionnaire)
				);
			}
			catch (ValidationException ve)
			{
				return UnprocessableEntity($"Invalid input data: {ve.ValidationResult.ErrorMessage}");
			}
			catch (Exception e)
			{
				return BadRequest($"Something went wrong in creating the questionnaire: {e.Message}.");
			}
		}
		
		[HttpPut("{id}")]
		public IActionResult UpdateQuestionnaire(int id, NewQuestionnaireDto updatedValues)
		{
			try
			{
				_unitOfWorkManager.StartUnitOfWork();
				Questionnaire updatedQuestionnaire = _questionnaireManager.ChangeQuestionnaire(
					id, 
					updatedValues.Title,
					updatedValues.Description,
					updatedValues.ProjectPhaseId);
				_unitOfWorkManager.EndUnitOfWork();

				if (updatedQuestionnaire == null)
				{
					return BadRequest("Something went wrong while updating the questionnaire.");
				}

				return Ok(_mapper.Map<QuestionnaireDto>(updatedQuestionnaire));
			}
			catch (ValidationException ve)
			{
				return UnprocessableEntity($"Invalid input data: {ve.ValidationResult.ErrorMessage}");
			}
			catch (ArgumentException e)
			{
				return NotFound(e.Message);
			}
		}
	
		[HttpDelete("{id}")]
		public IActionResult DeleteQuestionnaire(int id)
		{
			try
			{
				_unitOfWorkManager.StartUnitOfWork();
				Questionnaire deleted = _questionnaireManager.RemoveQuestionnaire(id);
				_unitOfWorkManager.EndUnitOfWork();
				
				if (deleted == null)
				{
					return BadRequest("Something went wrong while deleting the questionnaire.");
				}
				
				return Ok(_mapper.Map<QuestionnaireDto>(deleted));
			}
			catch (ArgumentException)
			{
				return NotFound("Questionnaire to delete not found.");
			}
		}
	}
}