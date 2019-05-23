using System.Collections.Generic;
using COI.BL.Domain.Ideation;

namespace COI.UI.MVC.Helpers
{
	public interface IFieldHelper
	{
		Field GetFirstOfType(IEnumerable<Field> fields, FieldType fieldType);
		string GetAllText(IEnumerable<Field> fields);
	}
	
	public class FieldHelper : IFieldHelper
	{
		public Field GetFirstOfType(IEnumerable<Field> fields, FieldType fieldType)
		{
			foreach (var field in fields)
			{
				if (field.FieldType.Equals(fieldType))
				{
					return field;
				}
			}

			return null;
		}

		public string GetAllText(IEnumerable<Field> fields)
		{
			string text = "";
			foreach (var field in fields)
			{
				if (field.FieldType.Equals(FieldType.Text))
				{
					text += field.Content;
				}
			}

			return text;
		}
	}
}