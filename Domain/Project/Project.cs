using System;
using System.Collections.Generic;
using COI.BL.Domain.Common;

namespace COI.BL.Domain.Project
{
	public class Project
	{
		public int ProjectId { get; set; }
		
		public String Title { get; set; }
		public String Description { get; set; }
		
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		
		public virtual ProjectState State { get; set; }
//		public Styling Styling { get; set; }

		public virtual Organisation.Organisation Organisation { get; set; }
		public virtual ICollection<ProjectPhase> ProjectPhases { get; set; }

		public Project()
		{
			this.ProjectPhases = new List<ProjectPhase>();
		}
	}
}