using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using AutoMapper;
using COI.BL.Application;
using COI.BL.Domain.Ideation;
using COI.BL.Domain.Questionnaire;
using COI.BL.Domain.User;
using COI.BL.Questionnaire;
using COI.UI.MVC.Models.DTO.Ideation;
using COI.UI.MVC.Models.DTO.Questionnaire;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers.api
{
	[ApiController]
	[Route("api/[controller]")]
	public class QuestionsController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly IQuestionnaireManager _questionnaireManager;
		private readonly ICityOfIdeasController _coiCtrl;

		public QuestionsController(IMapper mapper, IQuestionnaireManager questionnaireManager, ICityOfIdeasController coiCtrl)
		{
			_mapper = mapper;
			_questionnaireManager = questionnaireManager;
			_coiCtrl = coiCtrl;
		}
		
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
				return Ok(_mapper.Map<QuestionDto>(question));
			}
			catch (Exception e)
			{
				return BadRequest($"Something went wrong in getting the question: {e.Message}.");
			}
		}
		
		[HttpPost]
		public IActionResult PostNewQuestion(NewQuestionDto question)
		{
			try
			{
				Question createdQuestion = _questionnaireManager.AddQuestion(
					question.Inquiry, 
					question.QuestionType,
					question.QuestionnaireId);
				
				return CreatedAtAction(
					"GetQuestion", 
					new {id = createdQuestion.QuestionId},
					_mapper.Map<QuestionDto>(createdQuestion));
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
				Question updatedQuestion = _questionnaireManager.ChangeQuestion(
					id, 
					updatedValues.Inquiry, 
					updatedValues.QuestionType,
					updatedValues.QuestionnaireId);

				if (updatedQuestion == null)
				{
					return BadRequest("Something went wrong while updating the question.");
				}

				return Ok(_mapper.Map<QuestionDto>(updatedQuestion));
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
		
		[HttpDelete("{id}")]
		public IActionResult DeleteQuestion(int id)
		{
			try
			{
				Question deleted = _questionnaireManager.RemoveQuestion(id);
				if (deleted == null)
				{
					return BadRequest("Something went wrong while deleting the question.");
				}

				return Ok(_mapper.Map<QuestionDto>(deleted));
			}
			catch (ArgumentException)
			{
				return NotFound("Question to delete not found.");
			}
		}

		[HttpGet("{id}/Options")]
		public IActionResult GetOptionsForQuestionnaire(int id)
		{
			var options = _questionnaireManager.GetOptionsForQuestion(id);
			var response = _mapper.Map<List<OptionDto>>(options);

			return Ok(response);
		}

		[HttpPost]
		public IActionResult PostAnswers(List<NewAnswerDto> answers)
		{
			try
			{
				answers.ForEach(a => PostAnswer(a.QuestionId, a));
				return NoContent();
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[HttpPost("{id}")]
		public IActionResult PostAnswer(int id, NewAnswerDto answer)
		{
			try
			{
				Answer createdAnswer = null;
				if (answer.OptionId != 0)
				{
					createdAnswer = _coiCtrl.AddAnswerToOption(answer.UserId, answer.OptionId);
				}
				else
				{
					createdAnswer = _coiCtrl.AddAnswerToQuestion(answer.Content, answer.UserId, id);
				}

				if (createdAnswer != null)
				{
					return Ok(_mapper.Map<AnswerDto>(createdAnswer));
				}
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}

			return BadRequest("Something went wrong while creating the answer.");
		}
		
		// POST: api/Questions
//		[HttpPost]
//		public IActionResult PostMultipleAnswers(MultipleAnswersDto answers)
//		{
//			// TODO exception handling
//			
//			foreach (ChoiceAnswerDto choice in answers.Choices)
//			{
//				this.PostAnswerToChoice(choice);
//			}
//
//			foreach (OpenQuestionAnswerDto answer in answers.OpenAnswers)
//			{
//				this.PostAnswerToOpenQuestion(answer);
//			}
//			
//			// TODO answer with CreatedAtAction
//			return NoContent();
//		}
//
//		// POST: api/Questions/Open
//		[HttpPost("Open")]
//		public IActionResult PostAnswerToOpenQuestion(OpenQuestionAnswerDto answer)
//		{
//			try
//			{
//				Answer createdAnswer = _coiCtrl.AnswerOpenQuestion(answer.UserId, answer.QuestionId, answer.Content);
//				// TODO answer with CreatedAtAction
//				return NoContent();
//			}
//			catch (ArgumentException e)
//			{
//				return BadRequest(e.Message);
//			}
//		}
//		
//		// POST: api/Questions/Choice
//		[HttpPost("Choice")]
//		public IActionResult PostAnswerToChoice(ChoiceAnswerDto answer)
//		{
//			try
//			{
//				Answer createdAnswer = _coiCtrl.AnswerChoiceQuestion(answer.UserId, answer.OptionId);
//
//				// TODO answer with CreatedAtAction
//				return NoContent();
//			}
//			catch (ArgumentException e)
//			{
//				return BadRequest(e.Message);
//			}
//		}
	}
}