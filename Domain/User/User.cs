using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using COI.BL.Domain.Common;
using COI.BL.Domain.Helpers;
using COI.BL.Domain.Ideation;
using COI.BL.Domain.Questionnaire;
using COI.BL.Domain.Relations;
using Microsoft.AspNetCore.Identity;

namespace COI.BL.Domain.User
{
	public class User : IdentityUser
	{
		[Required]
		public String FirstName { get; set; }
		[Required]
		public String LastName { get; set; }

		public Gender Gender { get; set; }
		
		[DateOfBirth(MinAge = 13, MaxAge = 150)]
		public DateTime DateOfBirth { get; set; }
		
		[Range(1000,9999)]
		public int PostalCode { get; set; }
		
		public string ProfilePictureLocation { get; set; }

		public virtual ICollection<Flag> Flags { get; set; }
		public virtual ICollection<Vote> Votes { get; set; }
		public virtual ICollection<Answer> Answers { get; set; }
		public virtual ICollection<Comment> Comments { get; set; }

		public virtual ICollection<OrganisationUser> Organisations { get; set; }
		public virtual ICollection<Idea> Ideas { get; set; }

		/// <summary>
		/// Helper for getting the user's full name
		/// </summary>
		/// <returns></returns>
		public String GetName()
		{
			return String.Format("{0}{1}{2}", this.FirstName, 
				(String.IsNullOrEmpty(FirstName) || String.IsNullOrEmpty(LastName)) ? null : " ", // join with space
				this.LastName);
		}

		public User()
		{
			this.Flags = new List<Flag>();
			this.Votes = new List<Vote>();
			this.Answers = new List<Answer>();
			this.Comments = new List<Comment>();
			this.Ideas = new List<Idea>();
			this.Organisations = new List<OrganisationUser>();
			this.ProfilePictureLocation = "/img/user.png";
		}
	}
}