using System.Collections.Generic;
using COI.BL.Domain.Questionnaire;
using COI.BL.Questionnaire;
using Microsoft.AspNetCore.Mvc;

namespace UI_MVC.Controllers
{
	public class QuestionnaireController : Controller
	{
		private readonly IQuestionnaireManager _questionnaireManager;

		public QuestionnaireController(IQuestionnaireManager questionnaireManager)
		{
			_questionnaireManager = questionnaireManager;
		}
		
		// GET
		public IActionResult Index()
		{
			IEnumerable<Questionnaire> questionnaires = _questionnaireManager.GetQuestionnaires();
			return View(questionnaires);
		}
	}
}