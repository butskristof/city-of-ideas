using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace COI.UI.MVC.Authorization
{
	/// <summary>
	/// handler to check whether a user is a superadmin
	/// </summary>
	public class SuperadminHandler : AuthorizationHandler<UserInOrgOrSuperadminRequirement>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserInOrgOrSuperadminRequirement requirement)
		{
			if (context.User.IsInRole(AuthConstants.Superadmin))
			{
				context.Succeed(requirement);
			}

			return Task.CompletedTask;
		}
	}
}