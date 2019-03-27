using System;
using System.Collections.Generic;

namespace COI.BL.Domain.Platform
{
	public class Platform
	{
		public int PlatformId { get; set; }
		public String Name { get; set; }
		public int MaxCharsQuestions { get; set; }
		public int MaxCharAnswers { get; set; }
		
		public virtual ICollection<Organisation.Organisation> Organisations { get; set; }

		public Platform()
		{
			this.Organisations = new List<Organisation.Organisation>();
		}
	}
}