using System;
using System.Collections.Generic;

namespace COI.UI.MVC.Models.DTO.Ideation
{
	public class IdeationMinDto
	{
		public int IdeationId { get; set; }
		
		public string Title { get; set; }
		
		public DateTime Created { get; set; }
		public bool IsOpen { get; set; }
		public int VoteCount { get; set; }
		
		public List<FieldDto> Fields { get; set; }

		public int ProjectPhaseId { get; set; }
		
//		public int ShareCount { get; set; }
	}
}