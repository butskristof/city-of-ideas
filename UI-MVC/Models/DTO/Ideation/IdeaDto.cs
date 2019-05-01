using System;
using System.Collections.Generic;

namespace COI.UI.MVC.Models.DTO.Ideation
{
	public class IdeaDto
	{
		public int IdeaId { get; set; }
		
		public string Title { get; set; }

		public List<FieldDto> Fields { get; set; }
		public List<CommentDto> Comments { get; set; }

		public int VoteCount { get; set; }

		public int IdeationId { get; set; }
		public string UserId { get; set; }
	}
}