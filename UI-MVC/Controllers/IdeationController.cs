using COI.BL.Domain.Ideation;
using COI.BL.Ideation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers
{
	public class IdeationController : Controller
	{
		private readonly IIdeationManager _ideationManager;

		public IdeationController(IIdeationManager ideationManager)
		{
			_ideationManager = ideationManager;
		}
		
		[HttpGet]
		public IActionResult Details(int id)
		{
			Ideation ideation = _ideationManager.GetIdeation(id);
			return View(ideation);
		}
		
		[Authorize(Roles="Admin,Superadmin")]
		public IActionResult Create()
		{
			return View();
		}
	}
}