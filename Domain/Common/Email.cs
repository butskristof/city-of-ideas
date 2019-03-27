using System;

namespace COI.BL.Domain.Common
{
	public class Email
	{
		public int EmailId { get; set; }
		
		public String Front { get; set; }
		public String Domain { get; set; }
		public String Tld { get; set; } // top-level domain
	}
}