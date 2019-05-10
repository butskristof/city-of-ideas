using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using COI.BL.Application;
using COI.BL.Domain.Ideation;
using COI.BL.Ideation;
using COI.UI.MVC.Models;
using COI.UI.MVC.Models.DTO.Ideation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers
{
    [Authorize(AuthenticationSchemes = JwtConstants.AuthSchemes)]
	public class IdeaController : Controller
	{
		private readonly IIdeationManager _ideationManager;
		private readonly IMapper _mapper;
		private readonly CityOfIdeasController _coiCtrl;

		public IdeaController(IIdeationManager ideationManager, IMapper mapper)
		{
			_ideationManager = ideationManager;
			_mapper = mapper;
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult Index()
		{
			IEnumerable<Idea> ideas = _ideationManager.GetIdeas();
			return View(ideas);
		}
	
		[HttpGet]
		[AllowAnonymous]
		public IActionResult Details(int id)
		{
			Idea idea = _ideationManager.GetIdea(id);
			return View(idea);
		}
	}
}