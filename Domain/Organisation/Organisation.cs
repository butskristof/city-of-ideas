using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using COI.BL.Domain.Relations;

namespace COI.BL.Domain.Organisation
{
	public class Organisation
	{
		public int OrganisationId { get; set; }
		
		[Required]
		[MinLength(1)]
		public String Name { get; set; }
		
		[Required]
		[MinLength(1)]
		[MaxLength(20)]
		public String Identifier { get; set; } // for web URL

		public string Description { get; set; }

		public string LogoLocation { get; set; }
		public string ImageLocation { get; set; }
		[RegularExpression("^#[0-9a-fA-F]{6}$")]
		public string Color { get; set; }

		public DateTime Created { get; }

		public virtual ICollection<Project.Project> Projects { get; set; }
		public virtual ICollection<OrganisationUser> Users { get; set; }

		public Organisation()
		{
			this.Created = DateTime.Now;
			this.Projects = new List<Project.Project>();
			this.Users = new List<OrganisationUser>();
		}
	}
}