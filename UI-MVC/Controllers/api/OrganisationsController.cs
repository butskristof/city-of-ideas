using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using COI.BL;
using COI.BL.Domain.Organisation;
using COI.BL.Organisation;
using COI.UI.MVC.Models;
using COI.UI.MVC.Models.DTO.Organisation;
using COI.UI.MVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers.api
{
    [Authorize(AuthenticationSchemes = JwtConstants.AuthSchemes)]
	[ApiController]
	[Route("api/[controller]")]
	public class OrganisationsController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly IOrganisationManager _organisationManager;
		private readonly IUnitOfWorkManager _unitOfWorkManager;
		private readonly IFileService _fileService;

		public OrganisationsController(IMapper mapper, IOrganisationManager organisationManager, IUnitOfWorkManager unitOfWorkManager, IFileService fileService)
		{
			_mapper = mapper;
			_organisationManager = organisationManager;
			_unitOfWorkManager = unitOfWorkManager;
			_fileService = fileService;
		}

		[AllowAnonymous]
		[HttpGet]
		public IActionResult GetOrganisations()
		{
			var orgs = _organisationManager.GetOrganisations().ToList();
			var response = _mapper.Map<List<OrganisationDto>>(orgs);

			return Ok(response);
		}
		
		[AllowAnonymous]
		[HttpGet("{id}")]
		public IActionResult GetOrganisation(int id)
		{
			try
			{
				var org = _organisationManager.GetOrganisation(id);
				if (org == null)
				{
					return NotFound("Organisation not found.");
				}
				
				return Ok(_mapper.Map<OrganisationDto>(org));
			}
			catch (Exception e)
			{
				return BadRequest($"Something went wrong in getting the organisation: {e.Message}.");
			}
		}
		
		[HttpPost]
		public IActionResult PostNewOrganisation(NewOrganisationDto newOrg)
		{
			try
			{
				_unitOfWorkManager.StartUnitOfWork();
				Organisation org = _organisationManager.AddOrganisation(newOrg.Name, newOrg.Identifier);
				_unitOfWorkManager.EndUnitOfWork();

				return CreatedAtAction(
					"GetOrganisation", 
					new {id = org.OrganisationId},
					_mapper.Map<OrganisationDto>(org)
				);
			}
			catch (ValidationException ve)
			{
				return UnprocessableEntity($"Invalid input data: {ve.ValidationResult.ErrorMessage}");
			}
			catch (Exception e)
			{
				var msg = e.InnerException != null ? e.InnerException.Message : e.Message;
				return BadRequest($"Something went wrong in creating the organisation: {msg}.");
			}
		}
		
		[HttpPut("{id}")]
		public IActionResult UpdateOrganisation(int id, NewOrganisationDto updatedValues)
		{
			try
			{
				_unitOfWorkManager.StartUnitOfWork();
				Organisation updatedOrganisation = _organisationManager.ChangeOrganisation(
					id,
					updatedValues.Name,
					updatedValues.Identifier);
				_unitOfWorkManager.EndUnitOfWork();

				if (updatedOrganisation == null)
				{
					return BadRequest("Something went wrong while updating the organisation.");
				}

				return Ok(_mapper.Map<OrganisationDto>(updatedOrganisation));
			}
			catch (ValidationException ve)
			{
				return UnprocessableEntity($"Invalid input data: {ve.ValidationResult.ErrorMessage}");
			}
			catch (ArgumentException e)
			{
				return NotFound(e.Message);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
	
		[HttpDelete("{id}")]
		public IActionResult DeleteOrganisation(int id)
		{
			try
			{
				_unitOfWorkManager.StartUnitOfWork();
				Organisation deleted = _organisationManager.RemoveOrganisation(id);
				_unitOfWorkManager.EndUnitOfWork();
				
				if (deleted == null)
				{
					return BadRequest("Something went wrong while deleting the organisation.");
				}
				
				return Ok(_mapper.Map<OrganisationDto>(deleted));
			}
			catch (ArgumentException)
			{
				return NotFound("Organisation to delete not found.");
			}
		}

		[HttpPost("Logo")]
		public async Task<IActionResult> PostNewOrganisationLogo([FromForm] NewOrganisationLogoDto input)
		{
			try
			{
				string imgpath = await _fileService.SetOrganisationLogo(input.OrganisationId, input.Picture);
				return Ok(new
				{
					imgPath = imgpath
				});
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}