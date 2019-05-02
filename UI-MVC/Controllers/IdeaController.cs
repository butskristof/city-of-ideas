using System.Collections.Generic;
using AutoMapper;
using COI.BL.Domain.Ideation;
using COI.BL.Ideation;
using COI.UI.MVC.Models.DTO.Ideation;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers
{
	public class IdeaController : Controller
	{
		private readonly IIdeationManager _ideationManager;
		private readonly IMapper _mapper;

		public IdeaController(IIdeationManager ideationManager, IMapper mapper)
		{
			_ideationManager = ideationManager;
			_mapper = mapper;
		}

		// GET
		public IActionResult Index()
		{
			IEnumerable<Idea> ideas = _ideationManager.GetIdeas();
			return View(ideas);
		}

		// GET
		public IActionResult Details(int id)
		{
			Idea idea = _ideationManager.GetIdea(id);
			return View(_mapper.Map<IdeaDto>(idea));
		}
	}
}