namespace COI.BL.Domain.Relations
{
	public class OrganisationUser
	{
		public virtual Organisation.Organisation Organisation { get; set; }
		public virtual User.User User { get; set; }
	}
}