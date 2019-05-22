namespace COI.UI.MVC.Authorization
{
	public class AuthConstants
	{
		// ROLES
		public const string Superadmin = "Superadmin";
		public const string Admin = "Admin";
		public const string Moderator = "Moderator";
		public const string User = "User";

		public static readonly string[] Roles =
		{
			Superadmin, Admin, Moderator, User
		};
		
		// POLICIES
		public const string UserInOrgOrSuperadmin = "UserInOrgOrSuperadmin";
		public const string AdminPolicy = "Admin";
		public const string ModeratorPolicy = "Moderator";
	}
}