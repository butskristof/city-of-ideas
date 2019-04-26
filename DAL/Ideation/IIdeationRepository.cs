using System.Collections.Generic;
using COI.BL.Domain.Ideation;

namespace COI.DAL.Ideation
{
	public interface IIdeationRepository
	{
		IEnumerable<BL.Domain.Ideation.Ideation> ReadIdeations();
		BL.Domain.Ideation.Ideation ReadIdeation(int ideationId);

		IEnumerable<Idea> ReadIdeasForIdeation(int ideationId);

		BL.Domain.Ideation.Ideation CreateIdeation(BL.Domain.Ideation.Ideation ideation);

		BL.Domain.Ideation.Ideation UpdateIdeation(BL.Domain.Ideation.Ideation updatedIdeation);

		BL.Domain.Ideation.Ideation DeleteIdeation(int ideationId);
	}
}