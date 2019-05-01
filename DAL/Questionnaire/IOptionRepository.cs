using System.Collections.Generic;
using COI.BL.Domain.Questionnaire;

namespace COI.DAL.Questionnaire
{
	public interface IOptionRepository
	{
		Option ReadOption(int optionId);
		Option CreateOption(Option option);
		Option UpdateOption(Option option);
		Option DeleteOption(int optionId);
	}
}