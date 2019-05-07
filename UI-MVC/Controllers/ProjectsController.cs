using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using COI.BL.Domain.Project;
using COI.BL.Project;
using COI.UI.MVC.Models.DTO.Ideation;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly IProjectManager _projectManager;
        private readonly IMapper _mapper;

        public ProjectsController(IProjectManager projectManager, IMapper mapper)
        {
            _projectManager = projectManager;
            _mapper = mapper;
        }
        
        public IActionResult Index()
        {
            IEnumerable<Project> projects = _projectManager.GetProjects().ToList();
            return View(projects);
        }

        public IActionResult Index(int dropdownFase)
        {
            IEnumerable<Project> projects = _projectManager.GetProjects().ToList();
            return View(projects);
        }

        public IActionResult Details(int id)
        {
            Project project = _projectManager.GetProject(id);
            return View(_mapper.Map<IdeaDto>(project));
        }
    }
}