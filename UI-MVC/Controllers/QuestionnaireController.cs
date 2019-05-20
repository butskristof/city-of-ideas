using System.Collections.Generic;
using COI.BL.Domain.Questionnaire;
using COI.BL.Project;
using COI.BL.Questionnaire;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers
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
		
		// GET
		public IActionResult Details(int id)
		{
			Questionnaire questionnaire = _questionnaireManager.GetQuestionnaire(id);
			return View(questionnaire);
		}
		
		// GET
		public IActionResult Results(int id)
		{
			Questionnaire questionnaire = _questionnaireManager.GetQuestionnaire(id);
			return View(questionnaire);
		}

		public IActionResult Thanks()
		{
			return View();
		}
	}
}