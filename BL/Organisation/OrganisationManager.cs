using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using COI.BL.Domain.Ideation;
using COI.DAL.Ideation.EF;
using COI.DAL.Organisation;

namespace COI.BL.Organisation
{
	public class OrganisationManager : IOrganisationManager
	{
		private readonly IOrganisationRepository _organisationRepository;

		public OrganisationManager(IOrganisationRepository organisationRepository)
		{
			_organisationRepository = organisationRepository;
		}

		public IEnumerable<Domain.Organisation.Organisation> GetOrganisations()
		{
			return _organisationRepository.ReadOrganisations();
		}

		public Domain.Organisation.Organisation GetOrganisation(int orgId)
		{
			return _organisationRepository.ReadOrganisation(orgId);
		}

		public Domain.Organisation.Organisation AddOrganisation(string name, string identifier)
		{
			Domain.Organisation.Organisation org = new Domain.Organisation.Organisation()
			{
				Name = name,
				Identifier = identifier
			};

			return AddOrganisation(org);
		}

		private Domain.Organisation.Organisation AddOrganisation(Domain.Organisation.Organisation organisation)
		{
			Validate(organisation);
			return _organisationRepository.CreateOrganisation(organisation);
		}

		public Domain.Organisation.Organisation ChangeOrganisation(int id, string name, string identifier)
		{
			Domain.Organisation.Organisation org = GetOrganisation(id);
			if (org != null)
			{
				org.Name = name;
				org.Identifier = identifier;
				Validate(org);
				return _organisationRepository.UpdateOrganisation(org);
			}
			
			throw new ArgumentException("Organisation not found.");
		}

		public Domain.Organisation.Organisation RemoveOrganisation(int id)
		{
			return _organisationRepository.DeleteOrganisation(id);
		}

		private void Validate(Domain.Organisation.Organisation org)
		{
			Validator.ValidateObject(org, new ValidationContext(org), true);
		}
	}
}