using System.Collections.Generic;

namespace COI.BL.Organisation
{
	public interface IOrganisationManager
	{
		IEnumerable<Domain.Organisation.Organisation> GetOrganisations();
		Domain.Organisation.Organisation GetOrganisation(int orgId);
		Domain.Organisation.Organisation GetOrganisation(string identifier);
		Domain.Organisation.Organisation AddOrganisation(string name, 
			string identifier, 
			string description, 
			string color);
		Domain.Organisation.Organisation ChangeOrganisation(int id, 
			string name, 
			string identifier, 
			string description, 
			string color);
		Domain.Organisation.Organisation RemoveOrganisation(int id);

		void AddLogoToOrganisation(int organisationId, string imgPath);
		void AddImageToOrganisation(int organisationId, string imgPath);
	}
}