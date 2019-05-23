using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using AutoMapper;
using COI.BL.Organisation;
using COI.UI.MVC.Models;
using COI.UI.MVC.Models.DTO.Organisation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace COI.UI.MVC.Controllers
{
	public class HomeController : Controller
	{
		private readonly IOrganisationManager _organisationManager;
		
		public HomeController(IOrganisationManager organisationManager)
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
		[Route("/{orgIdentifier}")]
		public IActionResult SetOrganisation(string orgIdentifier)
		{
			return RedirectToAction("Index", "Organisation",new {orgid = orgIdentifier} );
		}
		
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
	}
}