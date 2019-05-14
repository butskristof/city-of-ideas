using System;
using System.Linq;
using COI.BL.Organisation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers
{
	public class OrganisationController : Controller
	{
		private readonly IOrganisationManager _organisationManager;
		
		public OrganisationController(IOrganisationManager organisationManager)
		{
			_organisationManager = organisationManager;
		}
		
		[HttpGet]
		public IActionResult Index()
		{
			var organisations = _organisationManager.GetOrganisations().ToList();
			return View(organisations);
		}

		[HttpPost]
		[Route("/organisation/{id}")]
		public IActionResult Index(int id)
		{
			var cookieOptions = new CookieOptions()
			{
				Path = "/", HttpOnly = false, IsEssential = true,
				Expires = DateTime.Now.AddMonths(1)
			};
			Response.Cookies.Append("organisation", id.ToString(), cookieOptions);
			return Redirect("/");
		}

		[HttpGet]
		public IActionResult Details(int id)
		{
			return View();
		}
	}
}