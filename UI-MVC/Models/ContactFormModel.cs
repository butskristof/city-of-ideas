using System.ComponentModel.DataAnnotations;

namespace COI.UI.MVC.Models
{
	public class ContactFormModel
	{
		[Required] public string Name { get; set; }
		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		[Required] public string Message { get; set; }
	}
}