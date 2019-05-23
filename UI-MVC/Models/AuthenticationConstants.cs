using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace COI.UI.MVC.Models
{
	/// <summary>
	/// Constants defined for allowing both Identity cookies and JWT bearer authentication
	/// </summary>
	public class AuthenticationConstants
	{
		public const string AuthSchemes = "Identity.Application," + JwtBearerDefaults.AuthenticationScheme;
	}
}