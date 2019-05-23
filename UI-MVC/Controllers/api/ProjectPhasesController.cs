using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using COI.BL;
using COI.BL.Domain.Project;
using COI.BL.Project;
using COI.UI.MVC.Authorization;
using COI.UI.MVC.Helpers;
using COI.UI.MVC.Models;
using COI.UI.MVC.Models.DTO.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers.api
{
    [Authorize(Policy = AuthConstants.AdminPolicy)]
    [Authorize(AuthenticationSchemes = AuthenticationConstants.AuthSchemes)]
	[ApiController]
	[Route("api/{orgId}/[controller]")]
	public class ProjectPhasesController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly IProjectManager _projectManager;
		private readonly IUnitOfWorkManager _unitOfWorkManager;

		private readonly IProjectPhasesHelper _projectPhasesHelper;

		public ProjectPhasesController(IMapper mapper, IProjectManager projectManager, IUnitOfWorkManager unitOfWorkManager, IProjectPhasesHelper projectPhasesHelper)
		{
			_mapper = mapper;
			_projectManager = projectManager;
			_unitOfWorkManager = unitOfWorkManager;
			_projectPhasesHelper = projectPhasesHelper;
		}

		[AllowAnonymous]
		[HttpGet("{id}")]
		public IActionResult GetProjectPhase(int id, [FromQuery] string userId)
		{
			try
			{
				return Ok(_projectPhasesHelper.GetProjectPhase(id, userId));
			}
			catch (ArgumentException e)
			{
				return NotFound(e.Message);
			}
			catch (Exception e)
			{
				return BadRequest($"Something went wrong in getting the project phase: {e.Message}.");
			}
		}

		[HttpPost]
		public IActionResult PostNewProjectPhase(NewProjectPhaseDto newPhase, [FromRoute] string orgId)
		{
			try
			{
				_unitOfWorkManager.StartUnitOfWork();
				ProjectPhase phase = _projectManager.AddProjectPhase(
					newPhase.Title, 
					newPhase.Description, 
					newPhase.StartDate, 
					newPhase.EndDate, 
					newPhase.ProjectId);
				_unitOfWorkManager.EndUnitOfWork();

				return CreatedAtAction(
					"GetProjectPhase", 
					new {orgId, id = phase.ProjectPhaseId},
					_mapper.Map<ProjectPhaseDto>(phase));
			}
			catch (ValidationException ve)
			{
				return UnprocessableEntity($"Invalid input data: {ve.ValidationResult.ErrorMessage}");
			}
			catch (Exception e)
			{
				return BadRequest($"Something went wrong in creating the project phase: {e.Message}.");
			}
		}

		[HttpPut("{id}")]
		public IActionResult UpdateProjectPhase(int id, NewProjectPhaseDto updatedValues)
		{
			try
			{
				_unitOfWorkManager.StartUnitOfWork();
				ProjectPhase updatedPhase = _projectManager.ChangeProjectPhase(
					id, 
					updatedValues.Title, 
					updatedValues.Description, 
					updatedValues.StartDate, 
					updatedValues.EndDate, 
					updatedValues.ProjectId);
				_unitOfWorkManager.EndUnitOfWork();

				if (updatedPhase == null)
				{
					return BadRequest("Something went wrong while updating the project phase.");
				}

				return Ok(_mapper.Map<ProjectPhaseDto>(updatedPhase));
			}
			catch (ValidationException ve)
			{
				return UnprocessableEntity($"Invalid input data: {ve.ValidationResult.ErrorMessage}");
			}
			catch (ArgumentException e)
			{
				switch (e.ParamName)
				{
					case "id":
						return NotFound(e.Message);
					case "projectId":
						return UnprocessableEntity(e.Message);
					default:
						return BadRequest(e.Message);
				}
			}
		}

	}
}