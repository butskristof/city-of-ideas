using System;
using System.ComponentModel.DataAnnotations;
using COI.BL.Domain.Common;
using COI.BL.Domain.Helpers;

namespace COI.UI.MVC.Models.DTO.User
{
	public class RegisterDto
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
		
		[Required]
		public string FirstName { get; set; }
		
		[Required]
		public string LastName { get; set; }
		
		public Gender? Gender { get; set; }
		
		[DateOfBirth(MinAge = 13, MaxAge = 150)]
		[Required]
		public DateTime? DateOfBirth { get; set; }
		
		[Required]
		[Range(1000,9999)]
		public int? PostalCode { get; set; }
		
		[DataType(DataType.Password)]
		public string Password { get; set; }
		
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Passwords do not match")]
		public string ConfirmPassword { get; set; }
	}
}