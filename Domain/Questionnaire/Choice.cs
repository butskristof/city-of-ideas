using System;
using System.Collections.Generic;
using System.Text;

namespace COI.BL.Domain.Questionnaire
{
	public class Choice : Question
	{
		public Boolean Dropdown { get; set; }
		public Boolean IsMultipleChoice { get; set; }
		

		public Choice()
		{
		}
	}
}