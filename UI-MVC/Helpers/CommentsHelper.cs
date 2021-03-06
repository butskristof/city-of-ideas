using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using COI.BL.Ideation;
using COI.BL.User;
using COI.UI.MVC.Models.DTO.Ideation;

namespace COI.UI.MVC.Helpers
{
	/// <summary>
	/// Helper for accessing comments and adding the user's current vote to the return object
	/// </summary>
	public interface ICommentsHelper
	{
		CommentDto GetComment(int id, string userId);
		List<CommentDto> GetCommentsForIdea(int ideaId, string userId);
		int GetUserVoteValueForComment(int commentId, string userId);
	}

	public class CommentsHelper : ICommentsHelper
	{
		private readonly IMapper _mapper;
		private readonly IIdeationManager _ideationManager;
		private readonly IUserManager _userManager;

		public CommentsHelper(IMapper mapper, IIdeationManager ideationManager, IUserManager userManager)
		{
			_mapper = mapper;
			_ideationManager = ideationManager;
			_userManager = userManager;
		}

		public CommentDto GetComment(int id, string userId)
		{
			try
			{
				var comment = _ideationManager.GetComment(id);
				if (comment == null)
				{
					throw new ArgumentException("Comment not found", "id");
				}

				var dto = _mapper.Map<CommentDto>(comment);

				var vote = _userManager.GetVoteForComment(id, userId);
				if (vote != null)
				{
					dto.UserVoteValue = vote.Value;
				}
				
				return dto;
			}
			catch (Exception e)
			{
				throw new Exception($"Something went wrong in getting the comment: {e.Message}.");
			}
		}

		public List<CommentDto> GetCommentsForIdea(int ideaId, string userId)
		{
			var comments = _ideationManager.GetCommentsForIdea(ideaId).ToList();
			var dtos = _mapper.Map<List<CommentDto>>(comments);
			
			foreach (var comment in dtos)
			{
				var vote = _userManager.GetVoteForComment(comment.CommentId, userId);
				if (vote != null)
				{
					comment.UserVoteValue = vote.Value;
				}
			}

			return dtos;
		}

		public int GetUserVoteValueForComment(int commentId, string userId)
		{
			var vote = _userManager.GetVoteForComment(commentId, userId);
			if (vote != null)
			{
				return vote.Value;
			}

			return 0;
		}
	}
}