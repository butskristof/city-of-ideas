using System;

namespace COI.BL.Domain.Common
{
	public class Address
	{
		public int AddressId { get; set; }
		
		public String Street { get; set; }
		public int Number { get; set; }
		public String NumberAddition { get; set; }
		public String PostalCode { get; set; }
		public String City { get; set; }
		public String Country { get; set; }
	}
}