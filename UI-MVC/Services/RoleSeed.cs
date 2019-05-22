using System;
using System.Threading.Tasks;
using COI.BL.Domain.User;
using COI.UI.MVC.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace COI.UI.MVC.Services
{
	public static class RoleSeed
	{
		public static async Task CreateRoles(IServiceProvider serviceProvider, IConfiguration Configuration)
		{
			var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

			// loop over precompiled roles and create if necessary
			foreach (string roleName in AuthConstants.Roles)
			{
				var roleExists = await roleManager.RoleExistsAsync(roleName);
				if (!roleExists)
				{
					var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
				}
			}

			// get data for superadmin user from configuration file
			var adminData = Configuration.GetSection("AdminUser");

			var powerUser = new User()
			{
				UserName = adminData["UserEmail"],
				Email = adminData["UserEmail"],
				FirstName = adminData["FirstName"],
				LastName = adminData["LastName"]
			};

			var userPw = adminData["UserPassword"];
			var user = await userManager.FindByEmailAsync(powerUser.Email);

			// create superadmin if necessary
			if (user == null)
			{
				var createPowerUser = await userManager.CreateAsync(powerUser, userPw);
				if (createPowerUser.Succeeded)
				{
					await userManager.AddToRoleAsync(powerUser, AuthConstants.Superadmin);
				}
			}
		}
	}
}