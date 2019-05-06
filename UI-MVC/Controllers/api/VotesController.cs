using System;
using AutoMapper;
using COI.BL;
using COI.BL.Application;
using COI.BL.Domain.User;
using COI.BL.User;
using COI.UI.MVC.Models;
using COI.UI.MVC.Models.DTO.Ideation;
using COI.UI.MVC.Models.DTO.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers.api
{
    [Authorize(AuthenticationSchemes = JwtConstants.AuthSchemes)]
	[ApiController]
	[Route("api/[controller]")]
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

		[HttpPost]
		public IActionResult PostNewVote(NewVoteDto vote)
		{
			try
			{
				Vote newVote = null;

				if (vote.IdeaId != null)
				{
					newVote = _coiCtrl.AddVoteToIdea(vote.Value, vote.UserId, vote.IdeaId.Value);
				}
				else if (vote.IdeationId != null)
				{
					newVote = _coiCtrl.AddVoteToIdeation(vote.Value, vote.UserId, vote.IdeationId.Value);
				}
				else if (vote.CommentId != null)
				{
					newVote = _coiCtrl.AddVoteToComment(vote.Value, vote.UserId, vote.CommentId.Value);
				}

				if (newVote != null)
				{
					return CreatedAtAction("GetVote", new {id = newVote.VoteId}, _mapper.Map<VoteDto>(newVote));
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
         
////		[HttpPost]
////		public IActionResult PostNewIdeaVote(NewIdeaVoteDto vote)
////		{
////			try
////			{
////				Vote createdVote = _coiCtrl.AddVoteToIdea(vote.Value, vote.UserId, vote.IdeaId);
////
////				// TODO update response
//////				return CreatedAtAction();
////				return Ok();
////			}
////			catch (ArgumentException e)
////			{
////				switch (e.ParamName)
////				{
////					case "ideaId":
////						return UnprocessableEntity(e.Message);
////						break;
////					case "userId":
////						return UnprocessableEntity(e.Message);
////						break;
////					default:
////						return BadRequest(e.Message);
////						break;
////				}
////			}
////		}
//		
//		[HttpPost("comment")]
//		public IActionResult PostNewCommentVote(NewCommentVoteDto vote)
//		{
//			try
//			{
//				_unitOfWorkManager.StartUnitOfWork();
//				Vote createdVote = _coiCtrl.AddVoteToComment(
//					vote.Value, 
//					vote.UserId, 
//					vote.CommentId);
//				_unitOfWorkManager.EndUnitOfWork();
//
//				// TODO update response
////				return CreatedAtAction();
//				return Ok();
//			}
//			catch (ArgumentException e)
//			{
//				switch (e.ParamName)
//				{
//					case "commentId":
//						return UnprocessableEntity(e.Message);
//					case "userId":
//						return UnprocessableEntity(e.Message);
//					default:
//						return BadRequest(e.Message);
//				}
//			}
//		}
		

		[HttpDelete("{id}")]
		public IActionResult DeleteVote(int id)
		{
			try
			{
				_unitOfWorkManager.StartUnitOfWork();
				Vote deleted = _userManager.RemoveVote(id);
				_unitOfWorkManager.EndUnitOfWork();
				
				if (deleted == null)
				{
					return NotFound("Comment to delete not found.");
				}

				return Ok(_mapper.Map<CommentDto>(deleted));
			}
			catch (ArgumentException)
			{
				return NotFound("Vote to delete not found.");
			}
		}
	}
}