using System;
using System.Collections.Generic;

namespace COI.UI.MVC.Models.DTO.Ideation
{
	public class IdeaMinDto
	{
		public int IdeaId { get; set; }
		
		public string Title { get; set; }

		public List<FieldDto> Fields { get; set; }
		
		public DateTime Created { get; set; }

		public int VoteCount { get; set; }
		public int UserVoteValue { get; set; }

		public int IdeationId { get; set; }
		public string UserId { get; set; }
	}
}