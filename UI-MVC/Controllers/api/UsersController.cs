using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using COI.BL.Domain.User;
using COI.UI.MVC.Models.DTO.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace COI.UI.MVC.Controllers.api
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class UsersController : ControllerBase
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly IConfiguration _config;

		public UsersController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration config)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_config = config;
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

		[AllowAnonymous]
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
                return Ok(new
                {
	                userId = user.Id
                });
			}
			
			// TODO 2FA & lockedout
            return BadRequest("Password incorrect.");
		}

		[AllowAnonymous]
		[HttpPost("RequestToken")]
		public async Task<IActionResult> RequestToken(LoginDto input)
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

			var pwSignInResult = await _signInManager.CheckPasswordSignInAsync(user, input.Password, false);
			if (pwSignInResult.Succeeded)
			{
				var token = GenerateJSONWebToken(user);
                return Ok(new
                {
	                token = new JwtSecurityTokenHandler().WriteToken(token),
	                userId = user.Id,
	                expiration = token.ValidTo
                });
			}
			
			// TODO 2FA & lockedout
            return BadRequest("Password incorrect.");
		}

		private JwtSecurityToken GenerateJSONWebToken(User user)
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName), 
			};

			var token = new JwtSecurityToken(_config["Jwt:Issuer"],
				_config["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddMinutes(30), 
				signingCredentials: credentials);
			
			return token;
		}

		[HttpPost("Logout")]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return Ok("You have been successfully logged out");
		}
	}
}