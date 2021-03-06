using System.Collections.Generic;
using COI.UI.MVC.Models.DTO.Project;

namespace COI.UI.MVC.Models.DTO.Organisation
{
	public class OrganisationDto
	{
		public int OrganisationId { get; set; }
		public string Name { get; set; }
		public string Identifier { get; set; }
		public string Description { get; set; }
		public string LogoLocation { get; set; }
		public string ImageLocation { get; set; }
		public string Color { get; set; }
		public List<ProjectMinDto> Projects { get; set; }
	}
}