using System;
using System.Text.RegularExpressions;
using System.Web;
using COI.BL.Domain.Organisation;
using COI.BL.Organisation;
using COI.UI.MVC.Models.DTO.Organisation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using SendGrid;

namespace COI.UI.MVC.Helpers
{
	public interface IOrganisationsHelper
	{
		Organisation GetOrganisation();
		string GetOrganisationIdentifier();
		bool HasOrganisation();
	}
	
	public class OrganisationsHelper : IOrganisationsHelper
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IOrganisationManager _organisationManager;
		
		public OrganisationsHelper(IHttpContextAccessor httpContextAccessor, IOrganisationManager organisationManager)
		{
			_httpContextAccessor = httpContextAccessor;
			_organisationManager = organisationManager;
		}

		public Organisation GetOrganisation()
		{ 
			if (_httpContextAccessor.HttpContext.Request.Path.Equals("/"))
			{
				return NoOrgPlaceholders();
			}

			var identifier = GetOrganisationIdentifier();
			var organisation = _organisationManager.GetOrganisation(identifier) ?? NoOrgPlaceholders();
			return organisation;
		}
		
		public string GetOrganisationIdentifier()
		{
			string path = _httpContextAccessor.HttpContext.Request.Path;
			var match = Regex.Match(path, "[a-zA-Z]+");
			return match.Groups[0].Value;
		}

		public bool HasOrganisation()
		{
			var orgId = GetOrganisationIdentifier();
			return orgId != "" && orgId != "NoOrg";
		}

		/**
		 * The following method returns an organisation with values to be shown when there is not organisation cookie
		 */
		public Organisation NoOrgPlaceholders()
		{
			return new Organisation
			{
				Name = "The Tree Company",
				Color = "#333333",
				Description = "The Tree Company",
				Identifier = "treecompany",
				ImageLocation = null,
				LogoLocation = null
			};
		}
	}
}