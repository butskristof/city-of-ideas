using System;
using System.Collections.Generic;
using COI.BL.Domain.Project;

namespace COI.UI.MVC.Models.DTO.Project
{
	public class ProjectMinDto
	{
		public int ProjectId { get; set; }
		
		public string Title { get; set; }
		public string Description { get; set; }
		
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		
		public ProjectState ProjectState { get; set; }
		public List<ProjectPhaseMinDto> ProjectPhases { get; set; }
		public int OrganisationId { get; set; }
	}
}