using System;
using System.Linq;
using System.Threading.Tasks;
using COI.BL.Domain.User;
using COI.UI.MVC.Models.DTO.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace COI.UI.MVC.Controllers.api
{
	[ApiController]
	[Route("api/[controller]")]
	public class UsersController : ControllerBase
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;

		public UsersController(UserManager<User> userManager, SignInManager<User> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

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
			
			User newUser = new BL.Domain.User.User()
			{
				UserName = input.Email,
				Email = input.Email,
				FirstName = input.FirstName,
				LastName = input.LastName
			};

			IdentityResult userCreationResult = null;
			try
			{
				userCreationResult = await _userManager.CreateAsync(newUser, input.Password);
			}
			catch (Exception e)
			{
				return BadRequest("Something went wrong while creating new user: " + e.Message);
			}

			if (!userCreationResult.Succeeded)
			{
				string msg = string.Join(";", userCreationResult.Errors.Select(x => x.Description));
				return BadRequest("Something went wrong while creating new user: " + msg);
			}

			await _signInManager.SignInAsync(newUser, false);
			return Ok("Registration successful.");
		}

		[HttpPost("Login")]
		public async Task<IActionResult> Login(LoginDto input)
		{
			if (string.IsNullOrWhiteSpace(input.Email) || string.IsNullOrWhiteSpace(input.Password))
			{
				return BadRequest("Email or password is empty.");
			}

			var user = await _userManager.FindByEmailAsync(input.Email);
			if (user == null)
			{
				return BadRequest("User not found.");
			}

			var pwSignInResult = await _signInManager.PasswordSignInAsync(user, input.Password, true, false);
			if (pwSignInResult.Succeeded)
			{
                return Ok("Login successful.");
			}
			
			// TODO 2FA & lockedout
            return BadRequest("Password incorrect.");
		}

		[HttpPost("Logout")]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return Ok("You have been successfully logged out");
		}
	}
}