using System;
using System.Threading.Tasks;
using COI.BL.Domain.User;
using COI.UI.MVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace COI.UI.MVC.Services
{
	public static class RoleSeed
	{
		public static async Task CreateRoles(IServiceProvider serviceProvider, IConfiguration Configuration)
		{
			// adding custom roles
			var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

			string[] roles =
			{
				Roles.Superadmin, 
				Roles.Admin, 
				Roles.Moderator, 
				Roles.User
			};
			IdentityResult roleResult;

			foreach (string roleName in roles)
			{
				var roleExists = await roleManager.RoleExistsAsync(roleName);
				if (!roleExists)
				{
					roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
				}
			}

			var adminData = Configuration.GetSection("AdminUser");

			var powerUser = new User()
			{
				UserName = adminData["UserEmail"],
				Email = adminData["UserEmail"],
				FirstName = adminData["FirstName"],
				LastName = adminData["LastName"]
			};

			string userPw = adminData["UserPassword"];
			var user = await userManager.FindByEmailAsync(powerUser.Email);

			if (user == null)
			{
				var createPowerUser = await userManager.CreateAsync(powerUser, userPw);
				if (createPowerUser.Succeeded)
				{
					await userManager.AddToRoleAsync(powerUser, Roles.Superadmin);
				}
			}
		}
	}
}