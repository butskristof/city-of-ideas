using COI.BL.Domain.Ideation;

namespace COI.DAL.Ideation
{
	public interface IFieldRepository
	{
		Field ReadField(int fieldId);
		Field CreateField(Field field);
		Field UpdateField(Field updatedField);
		Field DeleteField(int fieldId);
	}
}