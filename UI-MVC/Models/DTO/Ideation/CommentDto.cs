using System;
using System.Collections.Generic;
using COI.UI.MVC.Models.DTO.User;

namespace COI.UI.MVC.Models.DTO.Ideation
{
	public class CommentDto
	{
		public int CommentId { get; set; }
		
		public ICollection<FieldDto> Fields { get; set; }
		
		public DateTime Created { get; set; }
		
		public UserDto User { get; set; }
		
		public int Score { get; set; }
		public int IdeaId { get; set; }
	}
}