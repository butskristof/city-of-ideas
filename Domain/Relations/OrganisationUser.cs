using System.ComponentModel.DataAnnotations;

namespace COI.BL.Domain.Relations
{
	public class OrganisationUser
	{
//		[Required]
		public virtual Organisation.Organisation Organisation { get; set; }
//		[Required]
		public virtual User.User User { get; set; }
	}
}