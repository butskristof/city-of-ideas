using System;
using AutoMapper;
using COI.BL;
using COI.BL.Application;
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
	public class AnswersController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly IUnitOfWorkManager _unitOfWorkManager;
		private readonly IQuestionnaireManager _questionnaireManager;
		private readonly ICityOfIdeasController _cityOfIdeasController;

		public AnswersController(IMapper mapper, IUnitOfWorkManager unitOfWorkManager, IQuestionnaireManager questionnaireManager, ICityOfIdeasController cityOfIdeasController)
		{
			_mapper = mapper;
			_unitOfWorkManager = unitOfWorkManager;
			_questionnaireManager = questionnaireManager;
			_cityOfIdeasController = cityOfIdeasController;
		}

		[AllowAnonymous]
		[HttpGet("{id}")]
		public IActionResult GetAnswer(int id)
		{
			try
			{
				var answer = _questionnaireManager.GetAnswer(id);
				if (answer == null)
				{
					return NotFound("Answer not found.");
				}

				return Ok(_mapper.Map<AnswerDto>(answer));
			}
			catch (Exception e)
			{
				return BadRequest($"Something went wrong in getting the answer: {e.Message}");
			}
		}

		[HttpPost]
		public IActionResult PostNewAnswer(NewAnswerDto answer)
		{
			try
			{
				Answer createdAnswer = null;
				if (answer.OptionId != null && answer.OptionId != 0)
				{
					createdAnswer = _cityOfIdeasController.AddAnswerToOption(answer.UserId, answer.OptionId.Value);
				}
				else if (answer.QuestionId != null && answer.QuestionId != 0)
				{
					createdAnswer =
						_cityOfIdeasController.AddAnswerToQuestion(answer.Content, answer.UserId,
							answer.QuestionId.Value);
				}
				else
				{
					return BadRequest("Either option or question id should be given.");
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
	}
}