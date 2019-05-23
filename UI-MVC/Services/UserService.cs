using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using COI.BL.Domain.Common;
using COI.BL.Domain.Relations;
using COI.BL.Domain.User;
using COI.BL.Organisation;
using COI.UI.MVC.Authorization;
using COI.UI.MVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace COI.UI.MVC.Services
{
	/// <summary>
	/// Provides a layer on top of Identity for use with both MVC and API.
	/// Declared as a service in the presentation because Identity breaks n-tier possibilities.
	/// </summary>
	public interface IUserService
	{
		Task<User> GetUser(string userId);
		Task<JwtSecurityToken> GenerateJwt(string email, string password);
		Task<User> RegisterNewUser(
			string email, 
			string password, 
			string firstName, 
			string lastName,
			Gender gender,
			DateTime dateOfBirth,
			int postalCode,
			string organisation);

		Task<User> AddUserToOrganisation(string userId, string organisation);
		
		Task<string> Login(string email, string password);
		Task<bool> Logout();

		int NumberOfUsers();
	}
	
	public class UserService : IUserService
	{
		private readonly IOrganisationManager _organisationManager;
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly IConfiguration _config;

		public UserService(IOrganisationManager organisationManager, UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration config)
		{
			_organisationManager = organisationManager;
			_userManager = userManager;
			_signInManager = signInManager;
			_config = config;
		}

		public async Task<User> GetUser(string userId)
		{
			return await _userManager.FindByIdAsync(userId);
		}

		/// <summary>
		/// Verifies user credentials and returns a JWT that expires after 30 days.
		/// </summary>
		/// <param name="email"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
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
		
		/// <summary>
		/// Generate JWT, secrets are read from configuration file.
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		private JwtSecurityToken GenerateJsonWebToken(User user)
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Id),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName), 
			};

			var token = new JwtSecurityToken(_config["Jwt:Issuer"],
				_config["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(31), 
				signingCredentials: credentials);
			
			return token;
		}

		/// <summary>
		/// Create a new user and grant the role 'User'.
		/// </summary>
		/// <param name="email"></param>
		/// <param name="password"></param>
		/// <param name="firstName"></param>
		/// <param name="lastName"></param>
		/// <param name="gender"></param>
		/// <param name="dateOfBirth"></param>
		/// <param name="postalCode"></param>
		/// <param name="organisation"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="Exception"></exception>
		public async Task<User> RegisterNewUser(string email, 
			string password, 
			string firstName, 
			string lastName, 
			Gender gender,
			DateTime dateOfBirth, 
			int postalCode,
			string organisation)
		{
			var org = _organisationManager.GetOrganisation(organisation);
			if (org == null)
			{
				throw new ArgumentException("Organisation not found.", "organisation");
			}
			
			var newUser = new User()
			{
				UserName = email,
				Email = email,
				FirstName = firstName,
				LastName = lastName,
				Gender = gender,
				DateOfBirth = dateOfBirth,
				PostalCode = postalCode
			};
			newUser.Organisations.Add(new OrganisationUser()
			{
				User = newUser, Organisation = org
			});
			try
			{
				ValidateUser(newUser);
			}
			catch (ValidationException ve)
			{
				throw new ArgumentException($"Invalid input data: {ve.Message}");
			}

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
				await _userManager.AddToRoleAsync(newUser, AuthConstants.User);
				return newUser;
			}

			string msg = string.Join(";", userCreationResult.Errors.Select(x => x.Description));
			throw new Exception(msg);
		}

		/// <summary>
		/// Add an additional organisation to a user. 
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="organisation">Should be provided as string, eg 'districtantwerpen'</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="Exception"></exception>
		public async Task<User> AddUserToOrganisation(string userId, string organisation)
		{
			var org = _organisationManager.GetOrganisation(organisation);
			if (org == null)
			{
				throw new ArgumentException("Organisation not found.", "organisation");
			}

			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				throw new ArgumentException("User not found.", "userId");
			}
			
			if (org.Users.Any(ou => ou.User.Id == userId)) // user already in org
			{
				return user; 
			}

			
			user.Organisations.Add(new OrganisationUser()
			{
				User = user, Organisation = org
			});

			var updateResult = await _userManager.UpdateAsync(user);
			if (updateResult.Succeeded)
			{
                return user;
			}

			throw new Exception("Something went wrong when updating the user.");
		}

		/// <summary>
		/// Will login a user with cookies.
		/// </summary>
		/// <param name="email"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
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

		/// <summary>
		/// Will log a user out with cookies.
		/// </summary>
		/// <returns></returns>
		public async Task<bool> Logout()
		{
			await _signInManager.SignOutAsync();
			return true;
		}

		/// <summary>
		/// Returns the number of users currently in the application
		/// </summary>
		/// <returns></returns>
		public int NumberOfUsers()
		{
			return _userManager.Users.Count();
		}

		private void ValidateUser(User user)
		{
			Validator.ValidateObject(user, new ValidationContext(user), true);
		}
	}
}