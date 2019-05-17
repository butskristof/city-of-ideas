using System;
using System.Collections.Generic;
using COI.BL.Domain.Project;
using Microsoft.AspNetCore.Http;

namespace COI.UI.MVC.Models.DTO.Project
{
	public class NewProjectDto
	{
		public string Title { get; set; }
		public string Description { get; set; }
		
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }

		public ProjectState State { get; set; }
		
		public int OrganisationId { get; set; }
		
		public List<string> Texts { get; set; }
		public List<IFormFile> Images { get; set; }
	}
}