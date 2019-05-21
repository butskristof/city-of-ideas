using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using COI.BL.Application;
using COI.BL.Domain.Ideation;
using COI.BL.Domain.User;
using COI.BL.Ideation;
using COI.BL.User;
using COI.UI.MVC.Helpers;
using COI.UI.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers
{
    [Authorize(AuthenticationSchemes = JwtConstants.AuthSchemes)]
	public class IdeaController : Controller
	{
		private readonly IIdeationManager _ideationManager;
		private readonly UserManager<User> _userManager;
		private readonly IIdeasHelper _ideasHelper;

		public IdeaController(IIdeationManager ideationManager, IIdeasHelper ideasHelper, UserManager<User> userManager)
		{
			_ideationManager = ideationManager;
			_userManager = userManager;
			_ideasHelper = ideasHelper;
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
			var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
			var idea = _ideasHelper.GetIdea(id, userId);
			return View(idea);
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult Vote(int id)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
			var idea = _ideasHelper.GetIdea(id, userId);
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