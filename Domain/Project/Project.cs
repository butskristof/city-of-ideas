using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using COI.BL.Domain.Ideation;

namespace COI.BL.Domain.Project
{
	public class Project
	{
		public int ProjectId { get; set; }
		
		[Required]
		public String Title { get; set; }
		public String Description { get; set; }
		
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		
		public virtual ProjectState State { get; set; }
		public virtual ICollection<Field> Fields { get; set; }

		public virtual Organisation.Organisation Organisation { get; set; }
		public virtual ICollection<ProjectPhase> ProjectPhases { get; set; }

		public Project()
		{
			this.ProjectPhases = new List<ProjectPhase>();
			this.State = ProjectState.Open;
		}
	}
}