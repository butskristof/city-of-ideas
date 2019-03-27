using System;
using System.Collections.Generic;
using COI.BL.Domain.Common;
using COI.BL.Domain.Ideation;
using COI.BL.Domain.Questionnaire;
using COI.BL.Domain.Relations;

namespace COI.BL.Domain.User
{
	public class User
	{
		public int UserId { get; set; }
		
		public String FirstName { get; set; }
		public String LastName { get; set; }
		public virtual Email Email { get; set; }
		public Gender Gender { get; set; }
		public DateTime DateOfBirth { get; set; }
		public virtual Address Address { get; set; }

		public Role Role { get; set; }
		public virtual ICollection<Share> Shares { get; set; }
		public virtual ICollection<Flag> Flags { get; set; }
		public virtual ICollection<Vote> Votes { get; set; }
		public virtual ICollection<Answer> Answers { get; set; }
		public virtual ICollection<Comment> Comments { get; set; }

		public virtual ICollection<OrganisationUser> Organisations { get; set; }
		public virtual ICollection<Idea> Ideas { get; set; }

		public String GetName()
		{
			return String.Format("{0}{1}{2}", this.FirstName, (FirstName == null || LastName == null) ? null : " " , this.LastName);
		}

		public User()
		{
			this.Shares = new List<Share>();
			this.Flags = new List<Flag>();
			this.Votes = new List<Vote>();
			this.Answers = new List<Answer>();
			this.Comments = new List<Comment>();
			this.Ideas = new List<Idea>();
			this.Organisations = new List<OrganisationUser>();
		}
	}
}