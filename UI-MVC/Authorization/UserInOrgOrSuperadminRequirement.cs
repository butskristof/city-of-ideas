using Microsoft.AspNetCore.Authorization;

namespace COI.UI.MVC.Authorization
{
	/// <summary>
	/// Requirement declaration for checking that the user is in the current organisation
	/// or is a superuser.
	/// No additional properties are required, data is acquired in the handler
	/// </summary>
	public class UserInOrgOrSuperadminRequirement : IAuthorizationRequirement
	{
	}
}