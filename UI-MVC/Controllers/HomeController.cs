﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using COI.BL.Domain.Project;
using COI.BL.Project;
using COI.UI.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProjectManager _projectManager;

        public HomeController(IProjectManager projectManager)
        {
            _projectManager = projectManager;
        }
        
        public IActionResult Index()
        {
            IEnumerable<Project> projects = _projectManager.GetLastNProjects(6).ToList();
            return View(projects);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
