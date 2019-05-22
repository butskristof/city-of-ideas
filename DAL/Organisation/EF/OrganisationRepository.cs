using System;
using System.Collections.Generic;
using System.Linq;
using COI.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace COI.DAL.Organisation.EF
{
	public class OrganisationRepository : EfRepository, IOrganisationRepository
	{
		public OrganisationRepository(CityOfIdeasDbContext ctx) : base(ctx)
		{
		}

		public IEnumerable<BL.Domain.Organisation.Organisation> ReadOrganisations()
		{
			return _ctx.Organisations.AsEnumerable();
		}

		public BL.Domain.Organisation.Organisation ReadOrganisation(int organisationId)
		{
			return _ctx.Organisations.Find(organisationId);
		}

		public BL.Domain.Organisation.Organisation ReadOrganisation(string identifier)
		{
			return _ctx.Organisations.FirstOrDefault(o => o.Identifier == identifier);
		}

		public BL.Domain.Organisation.Organisation CreateOrganisation(
			BL.Domain.Organisation.Organisation organisation
		)
		{
			if (ReadOrganisation(organisation.OrganisationId) != null)
			{
				throw new ArgumentException("Organisation already in database.");
			}

			try
			{
				_ctx.Organisations.Add(organisation);
				_ctx.SaveChanges();

				return organisation;
			}
			catch (DbUpdateException exception)
			{
				var msg = exception.InnerException == null ? "Invalid object." : exception.InnerException.Message;
				throw new ArgumentException(msg);
			}
		}

		public BL.Domain.Organisation.Organisation UpdateOrganisation(
			BL.Domain.Organisation.Organisation updatedOrganisation
		)
		{
			var entryToUpdate = ReadOrganisation(updatedOrganisation.OrganisationId);

			if (entryToUpdate == null)
			{
				throw new ArgumentException("Organisation to update not found.");
			}

			_ctx.Entry(entryToUpdate).CurrentValues.SetValues(updatedOrganisation);
			_ctx.SaveChanges();

			return ReadOrganisation(updatedOrganisation.OrganisationId);
		}

		public BL.Domain.Organisation.Organisation DeleteOrganisation(int organisationId)
		{
			var orgToDelete = ReadOrganisation(organisationId);
			if (orgToDelete == null)
			{
				throw new ArgumentException("Organisation to delete not found.");
			}

			_ctx.Organisations.Remove(orgToDelete);
			_ctx.SaveChanges();

			return orgToDelete;
		}
	}
}