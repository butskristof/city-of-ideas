using System;
using System.ComponentModel.DataAnnotations;

namespace COI.BL.Domain.Helpers
{
	/**
	 * Can be used to specify a range for the object's age
	 */
	public class DateOfBirthAttribute : ValidationAttribute
	{
		public int MinAge { get; set; }
		public int MaxAge { get; set; }

		public override bool IsValid(object value)
		{
			if (value == null)
				return true;

			var val = (DateTime)value;

			if (val.AddYears(MinAge) > DateTime.Now)
				return false;

			return (val.AddYears(MaxAge) > DateTime.Now);
		}
	}

}