using System;
using COI.BL.Organisation;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers
{
	[Route("{orgId}/[controller]")]
	public class ManagementController : Controller
	{
		private readonly IOrganisationManager _organisationManager;

		public ManagementController(IOrganisationManager organisationManager)
		{
			_organisationManager = organisationManager;
		}
		
		[HttpGet]
		public IActionResult Organisation()
		{
			return View();
		}
		
		[HttpGet]
		public IActionResult NewOrganisation()
		{
			return View();
		}
	}
}