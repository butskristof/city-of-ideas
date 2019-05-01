using System;
using COI.BL.Domain.Project;

namespace COI.UI.MVC.Models.DTO.Project
{
	public class NewProjectPhaseDto
	{
		public string Title { get; set; }
		public string Description { get; set; }
		
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }

		public ProjectState State { get; set; }

		public int ProjectId { get; set; }
	}
}