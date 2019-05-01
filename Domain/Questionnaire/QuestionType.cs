using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace COI.BL.Domain.Questionnaire
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum QuestionType
	{
		OpenQuestion,
		SingleChoice,
		MultipleChoice
	}
}