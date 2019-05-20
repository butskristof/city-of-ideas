using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using COI.BL.Ideation;
using COI.BL.Project;
using COI.BL.User;
using COI.UI.MVC.Models.DTO.Ideation;
using COI.UI.MVC.Models.DTO.Project;

namespace COI.UI.MVC.Helpers
{
	public interface IProjectPhasesHelper
	{
		ProjectPhaseDto GetProjectPhase(int phaseId, string userId);
		IEnumerable<ProjectPhaseDto> GetProjectPhases(int projectId, string userId);
	}

	public class ProjectPhasesHelper : IProjectPhasesHelper
	{
		private readonly IMapper _mapper;
		private readonly IProjectManager _projectManager;
		private readonly IIdeationManager _ideationManager;
		private readonly IUserManager _userManager;

		private readonly IIdeationsHelper _ideationsHelper;

		public ProjectPhasesHelper(IMapper mapper, IProjectManager projectManager, IIdeationManager ideationManager, IUserManager userManager, IIdeationsHelper ideationsHelper)
		{
			_mapper = mapper;
			_projectManager = projectManager;
			_ideationManager = ideationManager;
			_userManager = userManager;
			_ideationsHelper = ideationsHelper;
		}

		public ProjectPhaseDto GetProjectPhase(int phaseId, string userId)
		{
			try
			{
				var phase = _projectManager.GetProjectPhase(phaseId);
				if (phase == null)
				{
					throw new ArgumentException("Phase not found", "phaseId");
				}

				var dto = _mapper.Map<ProjectPhaseDto>(phase);

				dto.Ideations = _ideationsHelper.GetMinIdeations(userId, phaseId) as List<IdeationMinDto>;
				
				return dto;
			}
			catch (Exception e)
			{
				throw new Exception($"Something went wrong in getting the comment: {e.Message}.");
			}
		}

		public IEnumerable<ProjectPhaseDto> GetProjectPhases(int projectId, string userId)
		{
			var phases = _projectManager.GetPhasesForProject(projectId).ToList();
			var dtos = _mapper.Map<List<ProjectPhaseDto>>(phases);

			foreach (var phase in dtos)
			{
				phase.Ideations =
					_ideationsHelper.GetMinIdeations(userId, phase.ProjectPhaseId) as List<IdeationMinDto>;
			}

			return dtos;
		}
	}
}