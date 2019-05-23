namespace COI.UI.MVC.Authorization
{
	/// <summary>
	/// constants for various authentication and authorization uses
	/// defined here as compile-time constant to avoid typos
	/// </summary>
	public class AuthConstants
	{
		// ROLES
		public const string Superadmin = "Superadmin";
		public const string Admin = "Admin";
		public const string Moderator = "Moderator";
		public const string User = "User";

		// put all roles together in an array for easy iteration
		public static readonly string[] Roles =
		{
			Superadmin, Admin, Moderator, User
		};
		
		// POLICIES
		public const string UserInOrgOrSuperadminPolicy = "UserInOrgOrSuperadmin";
		public const string SuperadminPolicy = "Superadmin";
		public const string AdminPolicy = "Admin";
		public const string ModeratorPolicy = "Moderator";
	}
}