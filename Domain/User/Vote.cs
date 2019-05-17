using System.ComponentModel.DataAnnotations;
using COI.BL.Domain.Ideation;

namespace COI.BL.Domain.User
{
	public class Vote
	{
		// email && user == null -> unverified
		// email && user != null -> verified
		// email && user.isVerified 
		
		public int VoteId { get; set; }
		
		[Required]
		public int Value { get; set; }

		public virtual User User { get; set; }
		public string Email { get; set; }

		public virtual Ideation.Ideation Ideation { get; set; }
		public virtual Idea Idea { get; set; }
		public virtual Comment Comment { get; set; }
	}
}