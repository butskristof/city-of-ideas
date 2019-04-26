using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using COI.BL.Domain.Organisation;
using COI.BL.Domain.Project;
using COI.BL.Project;
using COI.UI.MVC.Models.DTO.Organisation;
using COI.UI.MVC.Models.DTO.Project;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers.api
{
	[ApiController]
	[Route("api/[controller]")]
	public class ProjectsController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly IProjectManager _projectManager;

		public ProjectsController(IMapper mapper, IProjectManager projectManager)
		{
			_mapper = mapper;
			_projectManager = projectManager;
		}
		
		[HttpGet]
		public IActionResult GetProjects()
		{
			var projs = _projectManager.GetProjects().ToList();
			var response = _mapper.Map<List<ProjectDto>>(projs);

			return Ok(response);
		}
		
		[HttpGet("{id}")]
		public IActionResult GetProject(int id)
		{
			try
			{
				var proj = _projectManager.GetProject(id);
				if (proj == null)
				{
					return NotFound("Project not found.");
				}

				return Ok(_mapper.Map<ProjectDto>(proj));
			}
			catch (Exception e)
			{
				return BadRequest($"Something went wrong in getting the project: {e.Message}.");
			}
		}
		
		[HttpGet("{id}/projectPhases")]
		public IActionResult GetPhasesForProject(int id)
		{
			var phases = _projectManager.GetPhasesForProject(id).ToList();
			var response = _mapper.Map<List<ProjectPhaseDto>>(phases);

			return Ok(response);
		}
		
		[HttpPost]
		public IActionResult PostNewProject(NewProjectDto newProj)
		{
			try
			{
				Project p = _projectManager.AddProject(
					newProj.Title, 
					newProj.Description, 
					newProj.StartDate, 
					newProj.EndDate, 
					newProj.OrganisationId);

				return CreatedAtAction(
					"GetProject", 
					new {id = p.ProjectId},
					_mapper.Map<ProjectDto>(p));
			}
			catch (ValidationException ve)
			{
				return UnprocessableEntity($"Invalid input data: {ve.ValidationResult.ErrorMessage}");
			}
			catch (Exception e)
			{
				return BadRequest($"Something went wrong in creating the project: {e.Message}.");
			}
		}
		
		[HttpPut("{id}")]
		public IActionResult UpdateProject(int id, NewProjectDto updatedValues)
		{
			try
			{
				Project updatedProject = _projectManager.ChangeProject(
					id, 
					updatedValues.Title,
					updatedValues.Description, 
					updatedValues.StartDate, 
					updatedValues.EndDate, 
					updatedValues.OrganisationId);

				if (updatedProject == null)
				{
					return BadRequest("Something went wrong while updating the project.");
				}

				return Ok(_mapper.Map<ProjectDto>(updatedProject));
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
					case "organisationId":
						return UnprocessableEntity(e.Message);
						break;
					default:
						return BadRequest(e.Message);
						break;
				}
			}
		}
	
		[HttpDelete("{id}")]
		public IActionResult DeleteProject(int id)
		{
			try
			{
				Project deleted = _projectManager.RemoveProject(id);
				if (deleted == null)
				{
					return BadRequest("Something went wrong while deleting the project.");
				}
				
				return Ok(_mapper.Map<ProjectDto>(deleted));
			}
			catch (ArgumentException)
			{
				return NotFound("Project to delete not found.");
			}
		}
	}
}