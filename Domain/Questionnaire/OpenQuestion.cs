using System;
using System.Collections.Generic;

namespace COI.BL.Domain.Questionnaire
{
	public class OpenQuestion : Question
	{
		public virtual ICollection<Answer> Answers { get; set; }

		public OpenQuestion()
		{
			this.Answers = new List<Answer>();
		}
	}
}