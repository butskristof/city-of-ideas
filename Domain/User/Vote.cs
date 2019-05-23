using System;
using System.ComponentModel.DataAnnotations;
using COI.BL.Domain.Ideation;

namespace COI.BL.Domain.User
{
	public class Vote
	{
		public int VoteId { get; set; }
		
		[Required]
		public int Value { get; set; }

		public virtual User User { get; set; }
		public string Email { get; set; }
		
		public DateTime Created { get; }

		public virtual Ideation.Ideation Ideation { get; set; }
		public virtual Idea Idea { get; set; }
		public virtual Comment Comment { get; set; }

		public Vote()
		{
			this.Created = DateTime.Now;
		}
	}
}