using System.Collections.Generic;
using System.Security.Claims;
using COI.BL.Domain.Ideation;
using COI.BL.Domain.User;
using COI.BL.Ideation;
using COI.UI.MVC.Authorization;
using COI.UI.MVC.Helpers;
using COI.UI.MVC.Models.DTO.Demo;
using COI.UI.MVC.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers
{
    [Authorize(Policy = AuthConstants.AdminPolicy)]
	[Route("{orgId}/[controller]")]
	public class IdeationController : Controller
	{
		private readonly IIdeationManager _ideationManager;
		private readonly IIdeasHelper _ideasHelper;
		private readonly SignInManager<User> _signInManager;

		public IdeationController(IIdeationManager ideationManager, IIdeasHelper ideasHelper, SignInManager<User> signInManager)
		{
			_ideationManager = ideationManager;
			_ideasHelper = ideasHelper;
			_signInManager = signInManager;
		}
		
		[HttpGet]
        [Route("Details/{id}")]
		[AllowAnonymous]
		public IActionResult Details(int id)
		{
			string userId = null;
			if (_signInManager.IsSignedIn(User))
			{
				userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
			}
			Ideation ideation = _ideationManager.GetIdeation(id);
			var ideas = _ideasHelper.GetIdeas(userId, ideation.IdeationId);
			
			Dictionary<int, SAVotes> ideaVotes = new Dictionary<int, SAVotes>();

			foreach (var idea in ideas)
			{
				var anVotes = 0;
				var veVotes = 0;
				var usVotes = 0;
				var realIdea = _ideationManager.GetIdea(idea.IdeaId);
				foreach (var vote in realIdea.Votes)
				{
					if (vote.User != null)
					{
						usVotes += vote.Value;
					} else if (vote.Email != null)
					{
						veVotes += vote.Value;
					}
					else
					{
						anVotes += vote.Value;
					}
				}
				ideaVotes.Add(realIdea.IdeaId, new SAVotes()
				{
					anVotes = anVotes,
					veVotes = veVotes,
					usVotes = usVotes
				});
			}
			
			var model = new IdeationUserVote
			{
				ideas = ideas,
				ideation = ideation,
				ideaVotes = ideaVotes
			};
			return View(model);
		}
		
		[Route("Create/{phaseId}")]
		public IActionResult Create(int phaseId)
		{
			return View(phaseId);
		}
	}
}