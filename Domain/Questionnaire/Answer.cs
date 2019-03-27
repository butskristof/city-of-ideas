using System;

namespace COI.BL.Domain.Questionnaire
{
	public class Answer
	{
		public int AnswerId { get; set; }
		public String Content { get; set; }

		public virtual Option Option { get; set; }
		public virtual OpenQuestion OpenQuestion { get; set; }
		public virtual User.User User { get; set; }
	}
}