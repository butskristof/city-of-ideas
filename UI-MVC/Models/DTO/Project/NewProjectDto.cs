using System;
using System.Collections.Generic;
using COI.BL.Domain.Project;
using Microsoft.AspNetCore.Http;

namespace COI.UI.MVC.Models.DTO.Project
{
	public class NewProjectDto
	{
		public string Title { get; set; }
		
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }

		public ProjectState State { get; set; }
		
		public int OrganisationId { get; set; }
		
		public List<string> Texts { get; set; }
		public List<IFormFile> Images { get; set; }
		public List<IFormFile> Videos { get; set; }
		public List<string> Locations { get; set; }

		public NewProjectDto()
		{
			this.Texts = new List<string>();
			this.Images = new List<IFormFile>();
			this.Videos = new List<IFormFile>();
			this.Locations = new List<string>();
		}
	}
}