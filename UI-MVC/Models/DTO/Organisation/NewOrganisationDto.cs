using System.ComponentModel.DataAnnotations;

namespace COI.UI.MVC.Models.DTO.Organisation
{
	public class NewOrganisationDto
	{
		public string Name { get; set; }
		public string Identifier { get; set; }
		public string Description { get; set; }
		[RegularExpression("^#[0-9a-fA-F]{6}$")]
		public string Color { get; set; } // pass in as #FFFFFF
	}
}