using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using COI.BL.Domain.User;

namespace COI.BL.Domain.Ideation
{
	public class Idea : IVotable
	{
		public int IdeaId { get; set; }
		
		[Required]
		public String Title { get; set; }

		public DateTime Created { get; }
		
		// TODO AllowedFieldTypes
		public virtual ICollection<Field> Fields { get; set; }

		public virtual ICollection<Flag> Flags { get; set; }
		public virtual ICollection<Vote> Votes { get; set; }
		public virtual ICollection<Comment> Comments { get; set; }
		
		public virtual Ideation Ideation { get; set; }
		public virtual User.User CreatedBy { get; set; }

		public Idea()
		{
			Created = DateTime.Now;
			this.Fields = new List<Field>();
			this.Flags = new List<Flag>();
			this.Comments = new List<Comment>();
			this.Votes = new List<Vote>();
		}

		public int GetScore()
		{
			return this.Votes.Sum(v => v.Value);
		}
	}
}