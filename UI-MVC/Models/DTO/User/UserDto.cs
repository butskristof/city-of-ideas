using System;
using COI.BL.Domain.Common;

namespace COI.UI.MVC.Models.DTO.User
{
	public class UserDto
	{
		public string UserId { get; set; }
		public string Email { get; set; }
		public String FirstName { get; set; }
		public String LastName { get; set; }
		public Gender Gender { get; set; }
		public DateTime DateOfBirth { get; set; }
		public int PostalCode { get; set; }
	}
}