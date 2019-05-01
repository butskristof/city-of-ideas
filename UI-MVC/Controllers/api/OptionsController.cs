using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using COI.BL.Domain.Ideation;
using COI.BL.Domain.Questionnaire;
using COI.BL.Questionnaire;
using COI.UI.MVC.Models.DTO.Questionnaire;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers.api
{
	[ApiController]
	[Route("api/[controller]")]
	public class OptionsController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly IQuestionnaireManager _questionnaireManager;

		public OptionsController(IMapper mapper, IQuestionnaireManager questionnaireManager)
		{
			_mapper = mapper;
			_questionnaireManager = questionnaireManager;
		}
		
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
		
		[HttpDelete("{id}")]
		public IActionResult DeleteOption(int id)
		{
			try
			{
				Option deleted = _questionnaireManager.RemoveOption(id);
				if (deleted == null)
				{
					return NotFound("Option to delete not found.");
				}

				return Ok(_mapper.Map<OptionDto>(deleted));
			}
			catch (ArgumentException)
			{
				return NotFound("Option to delete not found.");
			}
		}
	}
}