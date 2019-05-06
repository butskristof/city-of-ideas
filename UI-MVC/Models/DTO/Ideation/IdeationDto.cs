using System;
using System.Collections.Generic;

namespace COI.UI.MVC.Models.DTO.Ideation
{
	public class IdeationDto
	{
		public int IdeationId { get; set; }
		
		public string Title { get; set; }
		
		public DateTime Created { get; set; }
		
		public int VoteCount { get; set; }

		public List<IdeaDto> Ideas { get; set; }
		public List<FieldDto> Fields { get; set; }

		public int ProjectPhaseId { get; set; }
		
//		public int ShareCount { get; set; }
	}
}