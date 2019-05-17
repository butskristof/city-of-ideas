using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using COI.BL.Domain.Project;
using COI.BL.Domain.User;

namespace COI.BL.Domain.Ideation
{
	public class Ideation : IVotable
	{
		public int IdeationId { get; set; }
		[Required]
		public String Title { get; set; }

		public DateTime Created { get; }

		public bool IsOpen { get; set; }
		
		// TODO add remaining fields
		public virtual ICollection<Vote> Votes { get; set; }
		public virtual ICollection<Idea> Ideas { get; set; }
		public virtual ICollection<Field> Fields { get; set; }

		public virtual ProjectPhase ProjectPhase { get; set; }

		public Ideation()
		{
			IsOpen = true;
			Created = DateTime.Now;
			this.Votes = new List<Vote>();
			this.Ideas = new List<Idea>();
			this.Fields = new List<Field>();
		}

		public int GetScore()
		{
			return this.Votes.Sum(v => v.Value);
		}
	}
}