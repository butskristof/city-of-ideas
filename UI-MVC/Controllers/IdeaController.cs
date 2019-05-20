using System.Collections.Generic;
using AutoMapper;
using COI.BL.Application;
using COI.BL.Domain.Ideation;
using COI.BL.Domain.User;
using COI.BL.Ideation;
using COI.UI.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers
{
    [Authorize(AuthenticationSchemes = JwtConstants.AuthSchemes)]
	public class IdeaController : Controller
	{
		private readonly IIdeationManager _ideationManager;

		public IdeaController(IIdeationManager ideationManager)
		{
			_ideationManager = ideationManager;
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

		[HttpGet]
		[AllowAnonymous]
		public IActionResult Vote(int id)
		{
			var idea = _ideationManager.GetIdea(id);
			return View(idea);
		}
		
		[HttpGet]
		[AllowAnonymous]
		public IActionResult ConfirmVote(int id)
		{
			var idea = _ideationManager.GetIdea(id);
			return View(idea);
		}
	}
}