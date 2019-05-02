using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
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
		private readonly IUserService _userService;

		public UsersController(IUserService userService)
		{
			_userService = userService;
		}

		[AllowAnonymous]
		[HttpPost("Register")]
		public async Task<IActionResult> RegisterNewUser(RegisterDto input)
		{
			if (string.IsNullOrWhiteSpace(input.Email) || string.IsNullOrWhiteSpace(input.Password))
			{
				return BadRequest("Email or password is empty.");
			}

			if (input.Password != input.ConfirmPassword)
			{
				return BadRequest("Passwords do not match.");
			}

			try
			{
				string userId =
					await _userService.RegisterNewUser(input.Email, input.Password, input.FirstName, input.LastName);

				return Ok(new
				{
					userId = userId
				});
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

		[HttpPost("Logout")]
		public async Task<IActionResult> Logout()
		{
			await _userService.Logout();
			return Ok("You have been successfully logged out");
		}
	}
}