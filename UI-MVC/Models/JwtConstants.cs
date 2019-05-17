using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace COI.UI.MVC.Models
{
	/**
	 * Constants defined for allowing both Identity cookies and JWT Bearers
	 */
	public class JwtConstants
	{
		public const string AuthSchemes = "Identity.Application," + JwtBearerDefaults.AuthenticationScheme;
	}
}