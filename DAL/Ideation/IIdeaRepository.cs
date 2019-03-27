using System.Collections.Generic;
using COI.BL.Domain.Ideation;

namespace COI.DAL.Ideation
{
	public interface IIdeaRepository
	{
		IEnumerable<Idea> ReadIdeas();
		Idea ReadIdea(int ideaId);
		void UpdateIdea(Idea idea);
	}
}