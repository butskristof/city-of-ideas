using System.Web;
using COI.UI.MVC.Models.DTO.Organisation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using SendGrid;

namespace COI.UI.MVC.Helpers
{
	public interface IOrganisationsHelper
	{
		NoRefOrganisationDto GetOrganisationFromCookie();
		string GetOrganisationJson();
	}
	
	public class OrganisationsHelper : IOrganisationsHelper
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		
		public OrganisationsHelper(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public NoRefOrganisationDto GetOrganisationFromCookie()
        {
	        var json = _httpContextAccessor.HttpContext.Request.Cookies["organisation"];
	        if (json == null)
	        {
		        return new NoRefOrganisationDto
		        {
			        Name = "No organisation",
			        Color = "#333333",
			        ImageLocation = null,
			        LogoLocation = null,
			        Description = "",
			        Identifier = null
		        };
	        }
	        return JsonConvert.DeserializeObject<NoRefOrganisationDto>(json);
        }

		public string GetOrganisationJson()
		{
	        var json = _httpContextAccessor.HttpContext.Request.Cookies["organisation"];
	        return json;
		}
	}
}