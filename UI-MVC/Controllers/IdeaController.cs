using System.Collections.Generic;
using COI.BL.Domain.Ideation;
using COI.BL.Ideation;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers
{
	public class IdeaController : Controller
	{
		private readonly IIdeationManager _ideationManager;

		public IdeaController(IIdeationManager ideationManager)
		{
			_ideationManager = ideationManager;
		}
		
		// GET
		public IActionResult Index()
		{
			IEnumerable<Idea> ideas = _ideationManager.GetIdeas();
			return View(ideas);
		}

		// GET
		public IActionResult Details(int id)
		{
			Idea idea = _ideationManager.GetIdea(id);
			return View(idea);
		}
	}
}