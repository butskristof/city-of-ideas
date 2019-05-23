using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using COI.BL.Domain.Project;

namespace COI.BL.Domain.Questionnaire
{
	public class Questionnaire
	{
		public int QuestionnaireId { get; set; }
		[Required]
		public String Title { get; set; }
		public String Description { get; set; }

		public virtual ProjectPhase ProjectPhase { get; set; }
		
		public virtual ICollection<Question> Questions { get; set; }
		
		public DateTime Created { get; }

		public Questionnaire()
		{
			this.Created = DateTime.Now;
			this.Questions = new List<Question>();
		}
	}
}