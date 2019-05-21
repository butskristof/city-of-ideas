using COI.UI.MVC.Models.DTO.Organisation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using SendGrid;

namespace COI.UI.MVC.Helpers
{
	public interface IOrganisationsHelper
	{
		NoRefOrganisationDto getOrganisationFromCookie();
	}
	
	public class OrganisationsHelper : IOrganisationsHelper
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		
		public OrganisationsHelper(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public NoRefOrganisationDto getOrganisationFromCookie()
        {
	        var json = _httpContextAccessor.HttpContext.Request.Cookies["organisation"];
            return JsonConvert.DeserializeObject<NoRefOrganisationDto>(json);
        }
	}
}