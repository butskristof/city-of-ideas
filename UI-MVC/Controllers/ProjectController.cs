using System.Collections.Generic;
using System.Linq;
using COI.BL.Domain.Project;
using COI.BL.Project;
using COI.UI.MVC.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers
{
    [Authorize(Policy = AuthConstants.AdminPolicy)]
    [Route("{orgId}/[controller]")]
    public class ProjectController : Controller
    {
        private readonly IProjectManager _projectManager;

        public ProjectController(IProjectManager projectManager)
        {
            _projectManager = projectManager;
        }
        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index([FromQuery] bool open, [FromQuery] bool showLimited, [FromRoute] string orgId)
        {
	        IEnumerable<Project> projects = null;
	        if (!showLimited)
	        {
		        projects = _projectManager.GetLastNProjects(orgId, 6).ToList();
	        }
	        else
	        {
		        ProjectState projectStateShown = open ? ProjectState.Open : ProjectState.Closed;
		        projects = _projectManager.GetLastNProjects(orgId, 6, projectStateShown).ToList();
	        }
	        ViewBag.open = open;
	        ViewBag.showLimited = showLimited;
            return View(projects);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Details/{id}")]
        public IActionResult Details(int id)
        {
            Project project = _projectManager.GetProject(id);
            return View(project);
        }

		[Route("Create")]
	    public IActionResult Create()
        {
	        return View();
        }

		[Route("CreatePhase/{projectId}")]
        public IActionResult CreatePhase(int projectId)
        {
	        return View(projectId);
        }
    }
}