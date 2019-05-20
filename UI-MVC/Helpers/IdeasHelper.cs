using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using COI.BL.Domain.Ideation;
using COI.BL.Ideation;
using COI.BL.User;
using COI.UI.MVC.Models.DTO.Ideation;

namespace COI.UI.MVC.Helpers
{
	public interface IIdeasHelper
	{
		IdeaDto GetIdea(int ideaId, string userId);
		IEnumerable<IdeaDto> GetIdeas(string userId, int ideationId = 0);
		IEnumerable<IdeaMinDto> GetMinIdeas(string userId, int ideationId = 0);
	}

	public class IdeasHelper : IIdeasHelper
	{
		private readonly IMapper _mapper;
		private readonly IIdeationManager _ideationManager;
		private readonly IUserManager _userManager;
		private readonly ICommentsHelper _commentsHelper;

		public IdeasHelper(IMapper mapper, IIdeationManager ideationManager, IUserManager userManager, ICommentsHelper commentsHelper)
		{
			_mapper = mapper;
			_ideationManager = ideationManager;
			_userManager = userManager;
			_commentsHelper = commentsHelper;
		}

		public IdeaDto GetIdea(int ideaId, string userId)
		{
			try
			{
				var idea = _ideationManager.GetIdea(ideaId);
				if (idea == null)
				{
					throw new ArgumentException("Idea not found", "ideaId");
				}

				var dto = _mapper.Map<IdeaDto>(idea);

				var vote = _userManager.GetVoteForIdea(ideaId, userId);
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

		public IEnumerable<IdeaDto> GetIdeas(string userId, int ideationId = 0)
		{
			List<Idea> ideas;
			if (ideationId != 0)
			{
				ideas = _ideationManager.GetIdeasForIdeation(ideationId).ToList();
			}
			else
			{
				ideas = _ideationManager.GetIdeas().ToList();
			}
			var dtos = _mapper.Map<List<IdeaDto>>(ideas);
			
			foreach (var idea in dtos)
			{
				var vote = _userManager.GetVoteForIdea(idea.IdeaId, userId);
				if (vote != null)
				{
					idea.UserVoteValue = vote.Value;
				}

				idea.Comments = _commentsHelper.GetCommentsForIdea(idea.IdeaId, userId);
			}

			return dtos;
		}

		public IEnumerable<IdeaMinDto> GetMinIdeas(string userId, int ideationId = 0)
		{
			var fullDtos = this.GetIdeas(userId, ideationId);
			return _mapper.Map<List<IdeaMinDto>>(fullDtos);
		}
	}
}