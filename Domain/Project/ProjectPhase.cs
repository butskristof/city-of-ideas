using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace COI.BL.Domain.Project
{
	public class ProjectPhase
	{
		public int ProjectPhaseId { get; set; }
		
		[Required]
		public String Title { get; set; }
		public String Description { get; set; }
		
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public DateTime Created { get; }
		
		/// <summary>
		/// Dynamically calculated based on the project end date
		/// </summary>
		public virtual ProjectState ProjectState => (EndDate < DateTime.Now) ? ProjectState.Closed : ProjectState.Open;

		public virtual Project Project { get; set; }
		public virtual ICollection<Ideation.Ideation> Ideations { get; set; }
		public virtual ICollection<Questionnaire.Questionnaire> Questionnaires { get; set; }

		public ProjectPhase()
		{
			this.Created = DateTime.Now;
			this.Ideations = new List<Ideation.Ideation>();
			this.Questionnaires = new List<Questionnaire.Questionnaire>();
		}
	}

}