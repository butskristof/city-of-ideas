using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using COI.BL.Domain.Questionnaire;
using COI.BL.Questionnaire;
using COI.UI.MVC.Models.DTO.Questionnaire;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers.api
{
	[ApiController]
	[Route("api/[controller]")]
	public class QuestionnairesController : ControllerBase
	{
		private readonly IMapper _mapper;
		private IQuestionnaireManager _questionnaireManager;

		public QuestionnairesController(IMapper mapper, IQuestionnaireManager questionnaireManager)
		{
			_mapper = mapper;
			_questionnaireManager = questionnaireManager;
		}
		
		[HttpGet]
		public IActionResult GetQuestionnaires()
		{
			var questionnaires = _questionnaireManager.GetQuestionnaires().ToList();
			var response = _mapper.Map<List<QuestionnaireDto>>(questionnaires);

			return Ok(response);
		}

		[HttpGet("{id}/Questions")]
		public IActionResult GetQuestionsForQuestionnaire(int id)
		{
			var questions = _questionnaireManager.GetQuestionsForQuestionnaire(id).ToList();
			var response = _mapper.Map<List<QuestionDto>>(questions);

			return Ok(response);
		}
		
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
				Questionnaire questionnaire = _questionnaireManager.AddQuestionnaire(newQuestionnaire.Title, newQuestionnaire.Description, newQuestionnaire.ProjectPhaseId);

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
				Questionnaire updatedQuestionnaire = _questionnaireManager.ChangeQuestionnaire(
					id, 
					updatedValues.Title,
					updatedValues.Description,
					updatedValues.ProjectPhaseId);

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
				return NotFound("Questionnaire to update not found.");
			}
		}
	
		[HttpDelete("{id}")]
		public IActionResult DeleteQuestionnaire(int id)
		{
			try
			{
				Questionnaire deleted = _questionnaireManager.RemoveQuestionnaire(id);
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