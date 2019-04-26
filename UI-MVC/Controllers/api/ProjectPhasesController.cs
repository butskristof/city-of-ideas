using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using COI.BL.Domain.Project;
using COI.BL.Project;
using COI.UI.MVC.Models.DTO.Project;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers.api
{
	[ApiController]
	[Route("api/[controller]")]
	public class ProjectPhasesController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly IProjectManager _projectManager;

		public ProjectPhasesController(IMapper mapper, IProjectManager projectManager)
		{
			_mapper = mapper;
			_projectManager = projectManager;
		}
		
		[HttpGet("{id}")]
		public IActionResult GetProjectPhase(int id)
		{
			try
			{
				var phase = _projectManager.GetProjectPhase(id);
				if (phase == null)
				{
					return NotFound("Project phase not found.");
				}

				return Ok(_mapper.Map<ProjectPhaseDto>(phase));
			}
			catch (Exception e)
			{
				return BadRequest($"Something went wrong in getting the project phase: {e.Message}.");
			}
		}

		[HttpPost]
		public IActionResult PostNewProjectPhase(NewProjectPhaseDto newPhase)
		{
			try
			{
				ProjectPhase phase = _projectManager.AddProjectPhase(
					newPhase.Title, 
					newPhase.Description, 
					newPhase.StartDate, 
					newPhase.EndDate, 
					newPhase.ProjectId);

				return CreatedAtAction(
					"GetProjectPhase", 
					new {id = phase.ProjectPhaseId},
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
				ProjectPhase updatedPhase = _projectManager.ChangeProjectPhase(
					id, 
					updatedValues.Title, 
					updatedValues.Description, 
					updatedValues.StartDate, 
					updatedValues.EndDate, 
					updatedValues.ProjectId);

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
						break;
					case "projectId":
						return UnprocessableEntity(e.Message);
						break;
					default:
						return BadRequest(e.Message);
						break;
				}
			}
		}

		[HttpDelete("{id}")]
		public IActionResult DeleteOrganisation(int id)
		{
			try
			{
				ProjectPhase deleted = _projectManager.RemoveProjectPhase(id);
				if (deleted == null)
				{
					return BadRequest("Something went wrong while deleting the project.");
				}
				
				return Ok(_mapper.Map<ProjectPhaseDto>(deleted));
			}
			catch (ArgumentException)
			{
				return NotFound("Project phase to delete not found.");
			}
		}
	}
}