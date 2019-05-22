using System.Linq;
using System.Threading.Tasks;
using COI.BL.Domain.User;
using COI.BL.Organisation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;

namespace COI.UI.MVC.Authorization
{
	public class UserInOrgHandler : AuthorizationHandler<UserInOrgOrSuperadminRequirement>
	{
		private readonly UserManager<User> _userManager;
		private readonly IOrganisationManager _organisationManager;

		public UserInOrgHandler(UserManager<User> userManager, IOrganisationManager organisationManager)
		{
			_userManager = userManager;
			_organisationManager = organisationManager;
		}

		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserInOrgOrSuperadminRequirement orSuperadminRequirement)
		{
			if (!(context.Resource is AuthorizationFilterContext authContext)) return Task.CompletedTask;
			
			var orgId = authContext.RouteData.Values["orgId"] as string;
			var org = _organisationManager.GetOrganisation(orgId);
			if (org == null) // invalid org identifier
			{
				return Task.CompletedTask;
			}

			var userId = _userManager.GetUserId(context.User); // both cookie and jwt contain UserName here
			
            // should find a result if the user belongs to the organisation
			var match = org.Users.FirstOrDefault(ou => ou.User.Id == userId); 
			if (match != null)
			{
				context.Succeed(orSuperadminRequirement);
			}
			return Task.CompletedTask;
		}
	}
}