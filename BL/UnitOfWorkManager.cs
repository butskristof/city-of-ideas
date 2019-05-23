using COI.DAL;

namespace COI.BL
{
	/// <summary>
	/// Manages the Unit of Work implementation
	/// This is also used in the presentation layer because of the dependency on the Identity framework which breaks
	/// n-tier possibilities
	/// </summary>
	public interface IUnitOfWorkManager
	{
		void StartUnitOfWork();
		void EndUnitOfWork();
		void Save();
	}

	public class UnitOfWorkManager : IUnitOfWorkManager
	{
		public UnitOfWorkManager(IUnitOfWork unitOfWork)
		{
			UnitOfWork = unitOfWork;
		}

		private IUnitOfWork UnitOfWork { get; set; }
		
		public void StartUnitOfWork()
		{
			UnitOfWork.StartUnitOfWork();
		}

		public void EndUnitOfWork()
		{
			UnitOfWork.EndUnitOfWork();
		}

		public void Save()
		{
			UnitOfWork.EndUnitOfWork();
		}
	}
}