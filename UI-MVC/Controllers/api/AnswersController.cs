using System;
using System.Collections.Generic;
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
		public IActionResult PostNewAnswer(List<NewAnswerDto> answerInputs)
		{
			try
			{
				// UoW is started here to make sure the request either fails or succeeds fully
                _unitOfWorkManager.StartUnitOfWork();
                List<Answer> answers = new List<Answer>();
                
                answerInputs.ForEach(answer =>
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
						throw new Exception("Invalid answer: either option or question id should be given.");
                    }

                    if (createdAnswer != null)
                    {
						answers.Add(createdAnswer);
                    }
                });
                
                _unitOfWorkManager.EndUnitOfWork();

                return Ok(_mapper.Map<List<AnswerDto>>(answers));
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}