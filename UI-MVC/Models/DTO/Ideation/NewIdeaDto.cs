using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace COI.UI.MVC.Models.DTO.Ideation
{
	public class NewIdeaDto
	{
		public string Title { get; set; }
		public int IdeationId { get; set; }
		[Required]
		public string UserId { get; set; }
		public List<string> Texts { get; set; }
		public List<IFormFile> Images { get; set; }
		public List<IFormFile> Videos { get; set; }
		public List<string> Locations { get; set; }
		// TODO add user createdby

		public NewIdeaDto()
		{
			this.Texts = new List<string>();
			this.Images = new List<IFormFile>();
			this.Videos = new List<IFormFile>();
			this.Locations = new List<string>();
		}
	}
}