using Microsoft.AspNetCore.Http;

namespace COI.UI.MVC.Models.DTO.User
{
	public class NewProfilePictureDto
	{
		public string UserId { get; set; }
		public IFormFile Picture { get; set; }
	}
}