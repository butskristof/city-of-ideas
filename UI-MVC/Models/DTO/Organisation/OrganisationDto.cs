using System.Collections.Generic;
using COI.UI.MVC.Models.DTO.Project;

namespace COI.UI.MVC.Models.DTO.Organisation
{
	public class OrganisationDto
	{
		public int OrganisationId { get; set; }
		public string Name { get; set; }
		public string Identifier { get; set; }
		public List<ProjectDto> Projects { get; set; }
	}
}