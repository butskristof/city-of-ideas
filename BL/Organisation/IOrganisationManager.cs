using System.Collections.Generic;

namespace COI.BL.Organisation
{
	public interface IOrganisationManager
	{
		IEnumerable<Domain.Organisation.Organisation> GetOrganisations();
		Domain.Organisation.Organisation GetOrganisation(int orgId);
		Domain.Organisation.Organisation AddOrganisation(string name, string identifier);
		Domain.Organisation.Organisation ChangeOrganisation(int id, string name, string identifier);
		Domain.Organisation.Organisation RemoveOrganisation(int id);
	}
}