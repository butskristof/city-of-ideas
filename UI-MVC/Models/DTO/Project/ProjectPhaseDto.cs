using System;
using System.Collections.Generic;
using COI.BL.Domain.Project;

namespace COI.UI.MVC.Models.DTO.Project
{
	public class ProjectPhaseDto
	{
		public int ProjectPhaseId { get; set; }
		
		public string Title { get; set; }
		public string Description { get; set; }
		
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		
		public string State { get; set; }
	}
}