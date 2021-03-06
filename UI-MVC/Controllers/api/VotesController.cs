using System;
using AutoMapper;
using COI.BL;
using COI.BL.Application;
using COI.BL.Domain.User;
using COI.BL.User;
using COI.UI.MVC.Authorization;
using COI.UI.MVC.Models;
using COI.UI.MVC.Models.DTO.Ideation;
using COI.UI.MVC.Models.DTO.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers.api
{
    [Authorize(Policy = AuthConstants.UserInOrgOrSuperadminPolicy)]
    [Authorize(AuthenticationSchemes = AuthenticationConstants.AuthSchemes)]
	[ApiController]
	[Route("api/{orgId}/[controller]")]
	public class VotesController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly IUserManager _userManager;
		private readonly ICityOfIdeasController _coiCtrl;
		private readonly IUnitOfWorkManager _unitOfWorkManager;

		public VotesController(IMapper mapper, IUserManager userManager, ICityOfIdeasController coiCtrl, IUnitOfWorkManager unitOfWorkManager)
		{
			_mapper = mapper;
			_userManager = userManager;
			_coiCtrl = coiCtrl;
			_unitOfWorkManager = unitOfWorkManager;
		}

		[AllowAnonymous]
		[HttpGet("{id}")]
		public IActionResult GetVote(int id)
		{
			try
			{
				var vote = _userManager.GetVote(id);
				if (vote == null)
				{
					return NotFound("Comment not found.");
				}
				return Ok(_mapper.Map<VoteDto>(vote));
			}
			catch (Exception e)
			{
				return BadRequest($"Something went wrong in getting the comment: {e.Message}.");
			}
		}

		/// <summary>
		/// Posts a new vote
		/// This action can also be used to change a user's vote: existence of a preceding vote will be checked
		/// and updated accordingly with the new value
		/// </summary>
		/// <param name="vote"></param>
		/// <param name="orgId"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpPost]
		public IActionResult PostNewVote(NewVoteDto vote, [FromRoute] string orgId)
		{
			try
			{
				Vote newVote = null;

				if (vote.IdeaId != null)
				{
					newVote = _coiCtrl.AddVoteToIdea(vote.Value, vote.UserId, vote.Email, vote.IdeaId.Value);
				}
				else if (vote.IdeationId != null)
				{
					newVote = _coiCtrl.AddVoteToIdeation(vote.Value, vote.UserId, vote.Email, vote.IdeationId.Value);
				}
				else if (vote.CommentId != null)
				{
					newVote = _coiCtrl.AddVoteToComment(vote.Value, vote.UserId, vote.Email, vote.CommentId.Value);
				}

				if (newVote != null)
				{
					return CreatedAtAction("GetVote", new {orgId, id = newVote.VoteId}, _mapper.Map<VoteDto>(newVote));
				}

				return BadRequest("No idea, ideation or comment specified.");

			}
			catch (ArgumentException e)
			{
				return UnprocessableEntity(e.Message);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}