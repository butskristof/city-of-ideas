using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace COI.BL.Domain.Project
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum ProjectState
	{
		Open,
		Closed
	}
}