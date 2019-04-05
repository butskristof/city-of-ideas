using COI.DAL;

namespace COI.BL
{
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
//			UnitOfWork.CommitChanges();
			UnitOfWork.EndUnitOfWork();
		}
	}
}