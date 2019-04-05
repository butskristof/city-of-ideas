using System;
using AutoMapper;
using COI.BL.Application;
using COI.BL.Domain.Questionnaire;
using COI.UI.MVC.Models.DTO.Questionnaire;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers.api
{
	[ApiController]
	[Route("api/[controller]")]
	public class QuestionsController : Controller
	{
		private readonly IMapper _mapper;
		private readonly ICityOfIdeasController _coiCtrl;

		public QuestionsController(IMapper mapper, ICityOfIdeasController coiCtrl)
		{
			_mapper = mapper;
			_coiCtrl = coiCtrl;
		}
		
		// POST: api/Questions
		[HttpPost]
		public IActionResult PostMultipleAnswers(MultipleAnswersDto answers)
		{
			// TODO exception handling
			
			foreach (ChoiceAnswerDto choice in answers.Choices)
			{
				this.PostAnswerToChoice(choice);
			}

			foreach (OpenQuestionAnswerDto answer in answers.OpenAnswers)
			{
				this.PostAnswerToOpenQuestion(answer);
			}
			
			// TODO answer with CreatedAtAction
			return NoContent();
		}

		// POST: api/Questions/Open
		[HttpPost("Open")]
		public IActionResult PostAnswerToOpenQuestion(OpenQuestionAnswerDto answer)
		{
			try
			{
				Answer createdAnswer = _coiCtrl.AnswerOpenQuestion(answer.UserId, answer.QuestionId, answer.Content);
				// TODO answer with CreatedAtAction
				return NoContent();
			}
			catch (ArgumentException e)
			{
				return BadRequest(e.Message);
			}
		}
		
		// POST: api/Questions/Choice
		[HttpPost("Choice")]
		public IActionResult PostAnswerToChoice(ChoiceAnswerDto answer)
		{
			try
			{
				Answer createdAnswer = _coiCtrl.AnswerChoiceQuestion(answer.UserId, answer.OptionId);

				// TODO answer with CreatedAtAction
				return NoContent();
			}
			catch (ArgumentException e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}