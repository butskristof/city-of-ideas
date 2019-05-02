using System;
using AutoMapper;
using COI.BL.Application;
using COI.BL.Domain.User;
using COI.BL.User;
using COI.UI.MVC.Models.DTO.Ideation;
using COI.UI.MVC.Models.DTO.User;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers.api
{
	[ApiController]
	[Route("api/[controller]")]
	public class VotesController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly IUserManager _userManager;
		private readonly ICityOfIdeasController _coiCtrl;

		public VotesController(IMapper mapper, IUserManager userManager, ICityOfIdeasController cityOfIdeasController)
		{
			_mapper = mapper;
			_userManager = userManager;
			_coiCtrl = cityOfIdeasController;
		}
		
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
         
//		[HttpPost]
//		public IActionResult PostNewIdeaVote(NewIdeaVoteDto vote)
//		{
//			try
//			{
//				Vote createdVote = _coiCtrl.AddVoteToIdea(vote.Value, vote.UserId, vote.IdeaId);
//
//				// TODO update response
////				return CreatedAtAction();
//				return Ok();
//			}
//			catch (ArgumentException e)
//			{
//				switch (e.ParamName)
//				{
//					case "ideaId":
//						return UnprocessableEntity(e.Message);
//						break;
//					case "userId":
//						return UnprocessableEntity(e.Message);
//						break;
//					default:
//						return BadRequest(e.Message);
//						break;
//				}
//			}
//		}
		
		[HttpPost]
		public IActionResult PostNewCommentVote(NewCommentVoteDto vote)
		{
			try
			{
				Vote createdVote = _coiCtrl.AddVoteToComment(
					vote.Value, 
					vote.UserId, 
					vote.CommentId);

				// TODO update response
//				return CreatedAtAction();
				return Ok();
			}
			catch (ArgumentException e)
			{
				switch (e.ParamName)
				{
					case "commentId":
						return UnprocessableEntity(e.Message);
					case "userId":
						return UnprocessableEntity(e.Message);
					default:
						return BadRequest(e.Message);
				}
			}
		}
		

		[HttpDelete("{id}")]
		public IActionResult DeleteVote(int id)
		{
			try
			{
				Vote deleted = _userManager.RemoveVote(id);
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