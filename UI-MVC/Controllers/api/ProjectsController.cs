using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Castle.Core.Internal;
using COI.BL;
using COI.BL.Domain.Ideation;
using COI.BL.Domain.Project;
using COI.BL.Ideation;
using COI.BL.Project;
using COI.UI.MVC.Helpers;
using COI.UI.MVC.Models;
using COI.UI.MVC.Models.DTO.Project;
using COI.UI.MVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers.api
{
    [Authorize(AuthenticationSchemes = JwtConstants.AuthSchemes)]
	[ApiController]
	[Route("api/[controller]")]
	public class ProjectsController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly IProjectManager _projectManager;
		private readonly IIdeationManager _ideationManager;
		private readonly IUnitOfWorkManager _unitOfWorkManager;
		private readonly IFileService _fileService;

		private readonly IProjectPhasesHelper _projectPhasesHelper;

		public ProjectsController(IMapper mapper, IProjectManager projectManager, IIdeationManager ideationManager, IUnitOfWorkManager unitOfWorkManager, IFileService fileService, IProjectPhasesHelper projectPhasesHelper)
		{
			_mapper = mapper;
			_projectManager = projectManager;
			_ideationManager = ideationManager;
			_unitOfWorkManager = unitOfWorkManager;
			_fileService = fileService;
			_projectPhasesHelper = projectPhasesHelper;
		}

		[AllowAnonymous]
		[HttpGet]
		public IActionResult GetProjects([FromQuery(Name = "limit")] int numberOfProjectsToGet, [FromQuery(Name = "state")] string stateString)
		{
			IEnumerable<Project> projs = null;
            bool stateValid = Enum.TryParse(stateString, true, out ProjectState state);
            
			if (numberOfProjectsToGet != 0)
			{
				if (stateValid)
				{
                    projs = _projectManager.GetLastNProjects(
                        numberOfProjectsToGet, state
                        ).ToList();
				}
				else
				{
                    projs = _projectManager.GetLastNProjects(
                        numberOfProjectsToGet
                        ).ToList();
				}
			}
			else
			{
				projs = _projectManager.GetProjects().ToList();
			}
			
			var response = _mapper.Map<List<ProjectMinDto>>(projs);

			return Ok(response);
		}

		[AllowAnonymous]
		[HttpGet("last")]
		public IActionResult GetLastProject([FromQuery(Name = "state")] string stateString)
		{
			Project ret = null;
			
			if (!stateString.IsNullOrEmpty())
			{
                bool stateValid = Enum.TryParse(stateString, true, out ProjectState state);
                if (!stateValid)
                {
                    return BadRequest("Project state invalid.");
                }

                ret = _projectManager.GetLastProjectWithState(state);
			}
			else
			{
				ret = _projectManager.GetLastNProjects(1).FirstOrDefault();
			}

			if (ret == null)
			{
				return BadRequest("No projects found for the current criteria.");
			}

			return Ok(_mapper.Map<ProjectMinDto>(ret));
		}
		
		[AllowAnonymous]
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
		
		[AllowAnonymous]
		[HttpGet("{id}/projectPhases")]
		public IActionResult GetPhasesForProject(int id, [FromQuery] string userId)
		{
			return Ok(_projectPhasesHelper.GetProjectPhases(id, userId));
		}
		
		[HttpPost]
		public async Task<IActionResult> PostNewProject([FromForm]NewProjectDto newProj)
		{
			try
			{
				_unitOfWorkManager.StartUnitOfWork();
				
				Project p = _projectManager.AddProject(
					newProj.Title, 
					newProj.StartDate, 
					newProj.EndDate, 
					newProj.OrganisationId);
				
				foreach (var video in newProj.Videos)
				{
					string imgLocation = await _fileService.ConvertFileToLocation(video);
					_ideationManager.AddFieldToProject(FieldType.Video, imgLocation, p.ProjectId);
				}

				foreach (var image in newProj.Images)
				{
					string imgLocation = await _fileService.ConvertFileToLocation(image);
					_ideationManager.AddFieldToProject(FieldType.Picture, imgLocation, p.ProjectId);
				}

				foreach (var textfield in newProj.Texts)
				{
					_ideationManager.AddFieldToProject(FieldType.Text, textfield, p.ProjectId);
				}
				
				foreach (var location in newProj.Locations)
				{
					_ideationManager.AddFieldToProject(FieldType.Location, location, p.ProjectId);
				}
				
				_unitOfWorkManager.EndUnitOfWork();

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
				_unitOfWorkManager.StartUnitOfWork();
				Project updatedProject = _projectManager.ChangeProject(
					id, 
					updatedValues.Title,
					updatedValues.StartDate, 
					updatedValues.EndDate, 
					updatedValues.OrganisationId);
				_unitOfWorkManager.EndUnitOfWork();

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
					case "organisationId":
						return UnprocessableEntity(e.Message);
					default:
						return BadRequest(e.Message);
				}
			}
		}
	
		[HttpDelete("{id}")]
		public IActionResult DeleteProject(int id)
		{
			try
			{
				_unitOfWorkManager.StartUnitOfWork();
				Project deleted = _projectManager.RemoveProject(id);
				_unitOfWorkManager.EndUnitOfWork();
				
				if (deleted == null)
				{
					return BadRequest("Something went wrong while deleting the project.");
				}
				
				return Ok(_mapper.Map<ProjectDto>(deleted));
			}
			catch (ArgumentException e)
			{
				return NotFound(e.Message);
			}
		}
	}
}