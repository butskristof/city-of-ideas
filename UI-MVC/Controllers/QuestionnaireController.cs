using System.Collections.Generic;
using COI.BL.Domain.Questionnaire;
using COI.BL.Questionnaire;
using COI.UI.MVC.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers
{
	[Route("{orgId}/[controller]")]
    [Authorize(Policy = AuthConstants.AdminPolicy)]
	public class QuestionnaireController : Controller
	{
		private readonly IQuestionnaireManager _questionnaireManager;

		public QuestionnaireController(IQuestionnaireManager questionnaireManager)
		{
			_questionnaireManager = questionnaireManager;
		}
		
		[AllowAnonymous]
		public IActionResult Index()
		{
			IEnumerable<Questionnaire> questionnaires = _questionnaireManager.GetQuestionnaires();
			return View(questionnaires);
		}
		
		[HttpGet]
		[Route("Details/{id}")]
		[AllowAnonymous]
		public IActionResult Details(int id)
		{
			Questionnaire questionnaire = _questionnaireManager.GetQuestionnaire(id);
			return View(questionnaire);
		}
		
		[HttpGet]
		[Route("Results/{id}")]
		[AllowAnonymous]
		public IActionResult Results(int id)
		{
			Questionnaire questionnaire = _questionnaireManager.GetQuestionnaire(id);
			return View(questionnaire);
		}

		[HttpGet]
		[Route("Thanks")]
		[AllowAnonymous]
		public IActionResult Thanks([FromQuery] string qId)
		{
			ViewBag.questionnaireId = qId;
			return View();
		}
		
		[HttpGet]
		[Route("Create/{phaseId}")]
		public IActionResult Create(int phaseId)
		{
			return View(phaseId);
		}
	}
}