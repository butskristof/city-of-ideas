using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
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
//		public Styling Styling { get; set; }

		public string LogoLocation { get; set; }

		public virtual Platform.Platform Platform { get; set; }
		public virtual ICollection<Project.Project> Projects { get; set; }
		public virtual ICollection<OrganisationUser> Users { get; set; }

		public Organisation()
		{
			this.Projects = new List<Project.Project>();
			this.Users = new List<OrganisationUser>();
		}
	}
}