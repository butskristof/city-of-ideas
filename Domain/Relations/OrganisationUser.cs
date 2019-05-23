namespace COI.BL.Domain.Relations
{
	/// <summary>
	/// Intermediate table for the many-to-many relation between user and organisation,
	/// configured with Fluent API in DAL
	/// </summary>
	public class OrganisationUser
	{
		public virtual Organisation.Organisation Organisation { get; set; }
		public virtual User.User User { get; set; }
	}
}