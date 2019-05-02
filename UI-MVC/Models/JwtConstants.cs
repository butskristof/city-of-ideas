using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace COI.UI.MVC.Models
{
	public class JwtConstants
	{
		public const string AuthSchemes = "Identity.Application," + JwtBearerDefaults.AuthenticationScheme;
	}
}