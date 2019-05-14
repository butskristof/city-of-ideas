using System;
using System.Collections.Generic;
using System.Linq;
using COI.BL.Domain.Ideation;
using COI.BL.Domain.Project;
using COI.BL.Ideation;
using COI.BL.Project;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers
{
    public class SuperadminController : Controller
    {
        private readonly IProjectManager _projectManager;
        private readonly IIdeationManager _ideationManager;
        
        public SuperadminController(IProjectManager projectManager, IIdeationManager ideationManager)
        {
            _projectManager = projectManager;
            _ideationManager = ideationManager;
        }
        
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ActiveProjects()
        {
            IEnumerable<Project> activeProjects = _projectManager.GetProjects().ToList();
            return View(activeProjects);
        }
        public IActionResult ClosedProjects()
        {
            IEnumerable<Project> closedProjects = _projectManager.GetProjects().ToList();
            return View(closedProjects);
        }
        
        [HttpGet]
        public IActionResult LatestActivities()
        {
            IEnumerable<Idea> latestIdea = _ideationManager.GetIdeas().ToList();
            return View(latestIdea);
        }
        
        public IActionResult URL()
        {
            return View();
        }
    }
}