using System;
using System.Collections.Generic;

namespace COI.BL.Domain.Questionnaire
{
	public class Option
	{
		public int OptionId { get; set; }
		
		public String Content { get; set; }
		public virtual ICollection<Answer> Answers { get; set; }

		public virtual Choice Choice { get; set; }

		public Option()
		{
			this.Answers = new List<Answer>();
		}
	}
}