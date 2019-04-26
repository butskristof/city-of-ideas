using System;

namespace COI.UI.MVC.Models.DTO.Project
{
	public class NewProjectPhaseDto
	{
		public string Title { get; set; }
		public string Description { get; set; }
		
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }

		public int ProjectId { get; set; }
	}
}