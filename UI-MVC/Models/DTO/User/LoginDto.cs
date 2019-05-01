using System.ComponentModel.DataAnnotations;

namespace COI.UI.MVC.Models.DTO.User
{
	public class LoginDto
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}