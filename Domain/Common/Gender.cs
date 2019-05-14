using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace COI.BL.Domain.Common
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum Gender
	{
		Male,
		Female
	}
}