using System;
using System.ComponentModel.DataAnnotations;

namespace COI.BL.Domain.Helpers
{
	/// <summary>
	/// Attribute to denote a range of allowed ages
	/// </summary>
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