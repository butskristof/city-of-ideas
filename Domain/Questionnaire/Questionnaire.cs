using System;
using System.Collections.Generic;
using COI.BL.Domain.Project;
using COI.BL.Domain.User;

namespace COI.BL.Domain.Questionnaire
{
	public class Questionnaire
	{
		public int QuestionnaireId { get; set; }
		public String Title { get; set; }
		public String Description { get; set; }

		public virtual ProjectPhase ProjectPhase { get; set; }
		
		// TODO add remaining fields
		public virtual ICollection<Question> Questions { get; set; }
		public virtual ICollection<Share> Shares { get; set; }

		public Questionnaire()
		{
			this.Questions = new List<Question>();
			this.Shares = new List<Share>();
		}
	}
}