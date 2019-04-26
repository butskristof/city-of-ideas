using System.Collections.Generic;

namespace COI.DAL.Organisation
{
	public interface IOrganisationRepository
	{
		IEnumerable<BL.Domain.Organisation.Organisation> ReadOrganisations();
		BL.Domain.Organisation.Organisation ReadOrganisation(int organisationId);
		BL.Domain.Organisation.Organisation CreateOrganisation(BL.Domain.Organisation.Organisation organisation);
		BL.Domain.Organisation.Organisation UpdateOrganisation(BL.Domain.Organisation.Organisation updatedOrganisation);
		BL.Domain.Organisation.Organisation DeleteOrganisation(int organisationId);
	}
}