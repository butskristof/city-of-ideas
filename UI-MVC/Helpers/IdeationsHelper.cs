using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using COI.BL.Domain.Ideation;
using COI.BL.Ideation;
using COI.BL.Project;
using COI.BL.User;
using COI.UI.MVC.Models.DTO.Ideation;

namespace COI.UI.MVC.Helpers
{
	public interface IIdeationsHelper
	{
		IdeationDto GetIdeation(int ideationId, string userId);
		IEnumerable<IdeationDto> GetIdeations(string userId, int phaseId = 0);
		IEnumerable<IdeationMinDto> GetMinIdeations(string userId, int phaseId = 0);
	}

	public class IdeationsHelper : IIdeationsHelper
	{
		private readonly IMapper _mapper;
		private readonly IIdeationManager _ideationManager;
		private readonly IUserManager _userManager;
		private readonly IProjectManager _projectManager;

		public IdeationsHelper(IMapper mapper, IIdeationManager ideationManager, IUserManager userManager, IProjectManager projectManager)
		{
			_mapper = mapper;
			_ideationManager = ideationManager;
			_userManager = userManager;
			_projectManager = projectManager;
		}

		public IdeationDto GetIdeation(int ideationId, string userId)
		{
			try
			{
				var ideation = _ideationManager.GetIdeation(ideationId);
				if (ideation == null)
				{
					throw new ArgumentException("Ideation not found", "ideationId");
				}

				var dto = _mapper.Map<IdeationDto>(ideation);

				var vote = _userManager.GetVoteForIdeation(ideationId, userId);
				if (vote != null)
				{
					dto.UserVoteValue = vote.Value;
				}
				
				return dto;
			}
			catch (Exception e)
			{
				throw new Exception($"Something went wrong in getting the ideation: {e.Message}.");
			}
		}
		
		public IEnumerable<IdeationDto> GetIdeations(string userId, int phaseId = 0)
		{
			IEnumerable<Ideation> ideations;
			if (phaseId != 0)
			{
				ideations = _projectManager.GetProjectPhase(phaseId).Ideations;
			}
			else
			{
                ideations = _ideationManager.GetIdeations().ToList();
			}
			var dtos = _mapper.Map<IEnumerable<IdeationDto>>(ideations);
			
			foreach (var ideation in dtos)
			{
				var vote = _userManager.GetVoteForIdeation(ideation.IdeationId, userId);
				if (vote != null)
				{
					ideation.UserVoteValue = vote.Value;
				}
			}

			return dtos;
		}

		public IEnumerable<IdeationMinDto> GetMinIdeations(string userId, int phaseId = 0)
		{
			var fullDtos = this.GetIdeations(userId, phaseId);
			return _mapper.Map<IEnumerable<IdeationMinDto>>(fullDtos);
		}
	}
}