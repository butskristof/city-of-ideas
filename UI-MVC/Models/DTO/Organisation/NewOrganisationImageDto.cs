using Microsoft.AspNetCore.Http;

namespace COI.UI.MVC.Models.DTO.Organisation
{
	public class NewOrganisationImageDto
	{
		public int OrganisationId { get; set; }
		public IFormFile Picture { get; set; }
	}
}