using System;
using System.Collections.Generic;
using COI.UI.MVC.Models.DTO.User;

namespace COI.UI.MVC.Models.DTO.Ideation
{
	public class CommentDto
	{
		public int CommentId { get; set; }
		
		public List<FieldDto> Fields { get; set; }
		
		public DateTime Created { get; set; }
		
		public UserDto User { get; set; }
		
		public int VoteCount { get; set; }
		public int UserVoteValue { get; set; }
		public int IdeaId { get; set; }
	}
}