using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

		public Domain.Organisation.Organisation AddOrganisation(string name, string identifier, string color)
		{
			Domain.Organisation.Organisation org = new Domain.Organisation.Organisation()
			{
				Name = name,
				Identifier = identifier,
				Color = color
			};

			return AddOrganisation(org);
		}

		private Domain.Organisation.Organisation AddOrganisation(Domain.Organisation.Organisation organisation)
		{
			Validate(organisation);
			return _organisationRepository.CreateOrganisation(organisation);
		}

		public Domain.Organisation.Organisation ChangeOrganisation(int id, string name, string identifier, string color)
		{
			Domain.Organisation.Organisation org = GetOrganisation(id);
			if (org != null)
			{
				org.Name = name;
				org.Identifier = identifier;
				org.Color = color;
				Validate(org);
				return _organisationRepository.UpdateOrganisation(org);
			}
			
			throw new ArgumentException("Organisation not found.");
		}

		public Domain.Organisation.Organisation RemoveOrganisation(int id)
		{
			return _organisationRepository.DeleteOrganisation(id);
		}

		/**
		 * Helper method to validate the object we want to persist against the validation annotations.
		 * Will throw a ValidationException upon failing.
		 */
		private void Validate(Domain.Organisation.Organisation org)
		{
			Validator.ValidateObject(org, new ValidationContext(org), true);
		}

		public void AddLogoToOrganisation(int organisationId, string imgPath)
		{
			Domain.Organisation.Organisation organisation = GetOrganisation(organisationId);
			if (organisation == null)
			{
				throw new ArgumentException("Organisation not found.");
			}

			organisation.LogoLocation = imgPath;
			_organisationRepository.UpdateOrganisation(organisation);
		}

		public void AddImageToOrganisation(int organisationId, string imgPath)
		{
			Domain.Organisation.Organisation organisation = GetOrganisation(organisationId);
			if (organisation == null)
			{
				throw new ArgumentException("Organisation not found.");
			}

			organisation.ImageLocation = imgPath;
			_organisationRepository.UpdateOrganisation(organisation);
		}
	}
}