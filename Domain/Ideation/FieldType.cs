using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace COI.BL.Domain.Ideation
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum FieldType
	{
		Text,
		Picture,
		Video,
		Location
	}
}