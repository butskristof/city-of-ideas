using System;
using COI.BL.Domain.Ideation;
using COI.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace COI.DAL.Ideation.EF
{
	public class FieldRepository : EfRepository, IFieldRepository
	{
		public FieldRepository(CityOfIdeasDbContext ctx) : base(ctx)
		{
		}

		public Field ReadField(int fieldId)
		{
			return _ctx.Fields.Find(fieldId);
		}

		public Field CreateField(Field field)
		{
			if (ReadField(field.FieldId) != null)
			{
				throw new ArgumentException("Field already in database.");
			}

			try
			{
				_ctx.Fields.Add(field);
				_ctx.SaveChanges();

				return field;
			}
			catch (DbUpdateException e)
			{
				var msg = e.InnerException == null ? "Invalid object." : e.InnerException.Message;
				throw new ArgumentException(msg);
			}
		}

		public Field UpdateField(Field updatedField)
		{
			var entryToUpdate = ReadField(updatedField.FieldId);

			if (entryToUpdate == null)
			{
				throw new ArgumentException("Field to update not found.");
			}

			_ctx.Entry(entryToUpdate).CurrentValues.SetValues(updatedField);
			_ctx.SaveChanges();

			return ReadField(updatedField.FieldId);
		}

		public Field DeleteField(int fieldId)
		{
			var toDelete = ReadField(fieldId);
			if (toDelete == null)
			{
				throw new ArgumentException("Field to delete not found.");
			}

			_ctx.Fields.Remove(toDelete);
			_ctx.SaveChanges();

			return toDelete;
		}
	}
}