using System;
using System.Collections.Generic;
using COI.BL.Domain.Project;
using COI.UI.MVC.Models.DTO.Ideation;
using COI.UI.MVC.Models.DTO.Questionnaire;

namespace COI.UI.MVC.Models.DTO.Project
{
	public class ProjectPhaseDto
	{
		public int ProjectPhaseId { get; set; }
		
		public string Title { get; set; }
		public string Description { get; set; }
		
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		
		public ProjectState ProjectState { get; set; }

		public int ProjectId { get; set; }

		public List<IdeationMinDto> Ideations { get; set; }
		public List<QuestionnaireMinDto> Questionnaires { get; set; }
	}
}