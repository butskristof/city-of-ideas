using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using COI.BL.Domain.Project;
using COI.BL.Project;
using COI.UI.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers
{
    [Authorize(AuthenticationSchemes = JwtConstants.AuthSchemes)]
    public class ProjectController : Controller
    {
        private readonly IProjectManager _projectManager;
        private readonly IMapper _mapper;

        public ProjectController(IProjectManager projectManager, IMapper mapper)
        {
            _projectManager = projectManager;
            _mapper = mapper;
        }
        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            IEnumerable<Project> projects = _projectManager.GetProjects().ToList();
            return View(projects);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            Project project = _projectManager.GetProject(id);
            return View(project);
        }

//		TODO: Authorization
        public IActionResult Create()
        {
	        return View();
        }

        public IActionResult CreatePhase()
        {
	        return View();
        }
    }
}