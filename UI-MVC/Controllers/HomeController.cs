using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using COI.BL.Domain.Organisation;
using COI.BL.Domain.Project;
using COI.BL.Organisation;
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
            IEnumerable<Project> projects = _projectManager.GetProjects().ToList();
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
