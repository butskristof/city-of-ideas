using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace COI.UI.MVC.Models.DTO.Ideation
{
	public class NewIdeaDto
	{
		public string Title { get; set; }
		public List<string> Texts { get; set; }
		public List<IFormFile> Images { get; set; }
		public List<IFormFile> Videos { get; set; }
		public int IdeationId { get; set; }
		// TODO add user createdby
	}
}