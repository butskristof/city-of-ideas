using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using COI.BL.Domain.Project;
using COI.BL.Domain.User;
using COI.BL.Project;
using COI.UI.MVC.Models;
using COI.UI.MVC.Models.DTO.Ideation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers
{
    [Authorize(AuthenticationSchemes = JwtConstants.AuthSchemes)]
    public class ProjectsController : Controller
    {
        private readonly IProjectManager _projectManager;

        public ProjectsController(IProjectManager projectManager)
        {
            _projectManager = projectManager;
        }
        
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

//        [Authorize(Roles=["test"])]
//		TODO: Authorization
        public IActionResult Create()
        {
	        return View();
        }
    }
}