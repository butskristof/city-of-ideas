using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using AutoMapper;
using COI.BL.Organisation;
using COI.UI.MVC.Models.DTO.Organisation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace COI.UI.MVC.Controllers
{
	public class OrganisationController : Controller
	{
		private readonly IOrganisationManager _organisationManager;
		private readonly IMapper _mapper;
		
		public OrganisationController(IOrganisationManager organisationManager, IMapper mapper)
		{
			_organisationManager = organisationManager;
			_mapper = mapper;
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
			var org = _organisationManager.GetOrganisation(id);
			var noRefOrg = _mapper.Map<NoRefOrganisationDto>(org);
			var orgJson = JsonConvert.SerializeObject(noRefOrg);
			Response.Cookies.Append("organisation", orgJson, cookieOptions);
			return Redirect("/");
		}

		[HttpGet]
		public IActionResult Details(int id)
		{
			return View();
		}
	}
}