using System.Security.Claims;
using COI.BL.Domain.Ideation;
using COI.BL.Domain.User;
using COI.BL.Ideation;
using COI.UI.MVC.Authorization;
using COI.UI.MVC.Helpers;
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
			var model = new IdeationUserVote
			{
				ideas = ideas,
				ideation = ideation
			};
			return View(model);
		}
		
		[Authorize(Roles="Admin,Superadmin")]
		[Route("/Create")]
		public IActionResult Create()
		{
			return View();
		}
	}
}