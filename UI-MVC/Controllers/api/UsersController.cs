using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using COI.UI.MVC.Models.DTO.User;
using COI.UI.MVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JwtConstants = COI.UI.MVC.Models.JwtConstants;

namespace COI.UI.MVC.Controllers.api
{
    [Authorize(AuthenticationSchemes = JwtConstants.AuthSchemes)]
	[ApiController]
	[Route("api/[controller]")]
	public class UsersController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly IUserService _userService;
		private readonly IFileService _fileService;

		public UsersController(IMapper mapper, IUserService userService, IFileService fileService)
		{
			_mapper = mapper;
			_userService = userService;
			_fileService = fileService;
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetUserInfo(string id)
		{
			try
			{
				var user = await _userService.GetUser(id);
				if (user == null)
				{
					return NotFound("User not found.");
				}

				return Ok(_mapper.Map<UserDto>(user));
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[AllowAnonymous]
		[HttpPost("Register")]
		public async Task<IActionResult> RegisterNewUser(RegisterDto input)
		{
			if (string.IsNullOrWhiteSpace(input.Email) 
			    || string.IsNullOrWhiteSpace(input.Password)
			    || !input.Gender.HasValue
			    || !input.DateOfBirth.HasValue
			    || !input.PostalCode.HasValue)
			{
				return BadRequest("Not all required fields are provided.");
			}

			if (input.Password != input.ConfirmPassword)
			{
				return BadRequest("Passwords do not match.");
			}

			try
			{
				var newUser =
					await _userService.RegisterNewUser(
						input.Email, 
						input.Password, 
						input.FirstName, 
						input.LastName,
						input.Gender.Value,
						input.DateOfBirth.Value,
						input.PostalCode.Value,
						input.Organisation);

				return Ok(_mapper.Map<UserDto>(newUser));
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[AllowAnonymous]
		[HttpPost("Login")]
		public async Task<IActionResult> Login(LoginDto input)
		{
			if (string.IsNullOrWhiteSpace(input.Email) || string.IsNullOrWhiteSpace(input.Password))
			{
				return BadRequest("Email or password is empty.");
			}

			try
			{
				var userId = await _userService.Login(input.Email, input.Password);
				return Ok(new
				{
					userId = userId
				});
			}
			catch (ArgumentException e)
			{
				return BadRequest(e.Message);
			}
		}

		[HttpPost("AddOrganisation")]
		public async Task<IActionResult> AddOrganisation(OrganisationUserDto orgUserDto)
		{
			try
			{
				var ret = await _userService.AddUserToOrganisation(orgUserDto.UserId, orgUserDto.Organisation);
				return Ok(_mapper.Map<UserDto>(ret));
			}
			catch (ArgumentException e)
			{
				return BadRequest(e.Message);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[AllowAnonymous]
		[HttpPost("RequestToken")]
		public async Task<IActionResult> RequestToken(LoginDto input)
		{
			if (string.IsNullOrWhiteSpace(input.Email) || string.IsNullOrWhiteSpace(input.Password))
			{
				return BadRequest("Email or password is empty.");
			}

			try
			{
				var token = await _userService.GenerateJwt(input.Email, input.Password);
				return Ok(new
				{
					token = new JwtSecurityTokenHandler().WriteToken(token),
					userId = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName)?.Value,
					expiration = token.ValidTo
				});
			}
			catch (ArgumentException e)
			{
				switch (e.ParamName)
				{
					case "email":
						return BadRequest(e.Message);
					case "password":
						return BadRequest(e.Message);
					default:
						return BadRequest("Something went wrong while logging in.");
				}
			}
		}
		
		[HttpPost("ProfilePicture")]
		public async Task<IActionResult> PostNewProfilePicture([FromForm] NewProfilePictureDto input)
		{
			try
			{
				string imgpath = await _fileService.SetUserProfilePicture(input.UserId, input.Picture);
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

		[HttpPost("Logout")]
		public async Task<IActionResult> Logout()
		{
			await _userService.Logout();
			return Ok("You have been successfully logged out");
		}
	}
}