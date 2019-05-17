using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace COI.UI.MVC.Models.DTO.Ideation
{
	public class NewIdeaCommentDto
	{
		public string UserId { get; set; }
		public int IdeaId { get; set; }
		public List<string> Texts { get; set; }
		public List<IFormFile> Images { get; set; }
		public List<IFormFile> Videos { get; set; }
	}
}