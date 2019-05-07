using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using COI.BL.Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace COI.UI.MVC.Services
{
	public interface IUserService
	{
		Task<JwtSecurityToken> GenerateJwt(string email, string password);
		Task<User> RegisterNewUser(string email, string password, string firstname, string lastname);
		Task<string> Login(string email, string password);
		Task<bool> Logout();

		int NumberOfUsers();
	}
	
	public class UserService : IUserService
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly IConfiguration _config;

		public UserService(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration config)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_config = config;
		}

		public async Task<JwtSecurityToken> GenerateJwt(string email, string password)
		{
			var user = await _userManager.FindByEmailAsync(email);
			if (user == null)
			{
				throw new ArgumentException("User not found.", "email");
			}
			
			var pwSignInResult = await _signInManager.CheckPasswordSignInAsync(user, password, false);
			if (pwSignInResult.Succeeded)
			{
				var token = GenerateJsonWebToken(user);
				return token;
			}
			
			throw new ArgumentException("Password incorrect", "password");
		}
		
		private JwtSecurityToken GenerateJsonWebToken(User user)
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.UniqueName, user.Id), 
			};

			var token = new JwtSecurityToken(_config["Jwt:Issuer"],
				_config["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddMinutes(30), 
				signingCredentials: credentials);
			
			return token;
		}

		public async Task<User> RegisterNewUser(string email, string password, string firstname, string lastname)
		{
			var newUser = new User()
			{
				UserName = email,
				Email = email,
				FirstName = firstname,
				LastName = lastname
			};

			IdentityResult userCreationResult = null;
			try
			{
				userCreationResult = await _userManager.CreateAsync(newUser, password);
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}

			if (userCreationResult.Succeeded)
			{
				return newUser;
			}

			string msg = string.Join(";", userCreationResult.Errors.Select(x => x.Description));
			throw new Exception(msg);
		}

		public async Task<string> Login(string email, string password)
		{
			var user = await _userManager.FindByEmailAsync(email);
			if (user == null)
			{
				throw new ArgumentException("User not found.", "email");
			}
			
			var pwSignInResult = await _signInManager.PasswordSignInAsync(user, password, true, true);
			if (pwSignInResult.Succeeded)
			{
				return user.Id;
			}
			
			// TODO 2FA & lockedout
			throw new ArgumentException("Password incorrect", "password");
		}

		public async Task<bool> Logout()
		{
			await _signInManager.SignOutAsync();
			return true;
		}

		public int NumberOfUsers()
		{
			return _userManager.Users.Count();
		}
	}
}