using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using COI.BL.Domain.Project;
using COI.BL.Project;
using COI.UI.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers
{
	[Route("{orgId}/[controller]")]
    public class OrganisationController : Controller
    {
        private readonly IProjectManager _projectManager;

        public OrganisationController(IProjectManager projectManager)
        {
            _projectManager = projectManager;
        }
        
        [HttpGet]
        public IActionResult Index([FromRoute] string orgId)
        {
            IEnumerable<Project> projects = _projectManager.GetLastNProjects(orgId, 6).ToList();
            return View(projects);
        }

        [HttpGet]
        [Route("/Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

    }
}
