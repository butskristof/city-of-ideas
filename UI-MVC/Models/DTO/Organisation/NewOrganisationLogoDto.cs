using Microsoft.AspNetCore.Http;

namespace COI.UI.MVC.Models.DTO.Organisation
{
	public class NewOrganisationLogoDto
	{
		public int OrganisationId { get; set; }
		public IFormFile Picture { get; set; }
	}
}