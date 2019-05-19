using System.ComponentModel.DataAnnotations;

namespace COI.BL.Domain.Ideation
{
	public class Field
	{
		public int FieldId { get; set; }
		[MinLength(2)]
		public string Content { get; set; } 
		public FieldType FieldType { get; set; }

		public virtual Ideation Ideation { get; set; }
		public virtual Idea Idea { get; set; }
		public virtual Comment Comment { get; set; }
		public virtual Project.Project Project { get; set; }
	}
}

// Content will have different semantics based on the FieldType
// Text: Just the raw text content
// Picture: string with the image location, e.g. /uploads/users/userid/filename.jpg
// Video: string with the video location, e.g. /uploads/fields/videoname.mp4
// Location: latitude and longitude separated by ;