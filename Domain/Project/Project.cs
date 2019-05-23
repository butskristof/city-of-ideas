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
		
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public DateTime Created { get; }

		/// <summary>
		/// Dynamically calculated based on the project end date
		/// </summary>
		public virtual ProjectState ProjectState => (EndDate < DateTime.Now) ? ProjectState.Closed : ProjectState.Open;

		public virtual ICollection<Field> Fields { get; set; }

		public virtual Organisation.Organisation Organisation { get; set; }
		public virtual ICollection<ProjectPhase> ProjectPhases { get; set; }

		public Project()
		{
			this.Created = DateTime.Now;
			this.ProjectPhases = new List<ProjectPhase>();
		}
	}
}