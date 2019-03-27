using System;

namespace COI.BL.Domain.Questionnaire
{
	public abstract class Question
	{
		public int QuestionId { get; set; }
		public String Inquiry { get; set; }
		public virtual Questionnaire Questionnaire { get; set; }
	}
}