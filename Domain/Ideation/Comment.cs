using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using COI.BL.Domain.User;

namespace COI.BL.Domain.Ideation
{
	public class Comment : IVotable
	{
		public int CommentId { get; set; }
		
		public virtual ICollection<Flag> Flags { get; set; }
		public virtual ICollection<Vote> Votes { get; set; }
		public virtual ICollection<Field> Fields { get; set; }
		
		public DateTime Created { get; set; }

		public virtual User.User User { get; set; }
		public virtual Idea Idea { get; set; }

		public Comment()
		{
			Created = DateTime.Now;
			this.Flags = new List<Flag>();
			this.Votes = new List<Vote>();
			this.Fields = new List<Field>();
		}

		public int GetScore()
		{
			return this.Votes.Sum(v => v.Value);
		}
	}
}