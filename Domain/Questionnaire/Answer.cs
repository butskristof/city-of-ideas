using System;

namespace COI.BL.Domain.Questionnaire
{
	public class Answer
	{
		public int AnswerId { get; set; }
		public String Content { get; set; }

		public DateTime Created { get; }

		public virtual Option Option { get; set; }
		public virtual Question Question { get; set; }
		public virtual User.User User { get; set; }

		public Answer()
		{
			this.Created = DateTime.Now;
		}
	}
}