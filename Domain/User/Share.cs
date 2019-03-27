using System.Collections.Generic;

namespace COI.BL.Domain.User
{
	public class Share
	{
		public int ShareId { get; set; }
		public virtual User User { get; set; }
		public virtual SharePlace SharePlace { get; set; }

		public virtual Ideation.Ideation Ideation { get; set; }
		public virtual Ideation.Idea Idea { get; set; }
		public virtual Questionnaire.Questionnaire Questionnaire { get; set; }
	}
}