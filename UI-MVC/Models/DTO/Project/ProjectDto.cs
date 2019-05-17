using System;
using System.Collections.Generic;
using COI.BL.Domain.Project;
using COI.UI.MVC.Models.DTO.Ideation;

namespace COI.UI.MVC.Models.DTO.Project
{
	public class ProjectDto
	{
		public int ProjectId { get; set; }
		
		public string Title { get; set; }
		public string Description { get; set; }
		
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		
		public ProjectState ProjectState { get; set; }
		public List<FieldDto> Fields { get; set; }
		public int OrganisationId { get; set; }
		public List<ProjectPhaseDto> ProjectPhases { get; set; }
	}
}