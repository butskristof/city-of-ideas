using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace COI.BL.Domain.Questionnaire
{
	public class Question
	{
		public int QuestionId { get; set; }
		[Required]
		public String Inquiry { get; set; }

		[Required]
		public QuestionType QuestionType { get; set; }
		
		public virtual ICollection<Answer> Answers { get; set; }
		public virtual ICollection<Option> Options { get; set; }
		
		public virtual Questionnaire Questionnaire { get; set; }

		public Question()
		{
			this.Answers = new List<Answer>();
			this.Options = new List<Option>();
		}
	}
}