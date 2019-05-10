using System;
using System.ComponentModel.DataAnnotations;
using COI.BL.Domain.Common;

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
	}
}