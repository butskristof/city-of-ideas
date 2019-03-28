using COI.DAL;

namespace COI.BL
{
	public class UnitOfWorkManager : IUnitOfWorkManager
	{
		public UnitOfWorkManager()
		{
			UnitOfWork = new UnitOfWork();
		}

		public UnitOfWorkManager(UnitOfWork unitOfWork)
		{
			UnitOfWork = unitOfWork;
		}

		internal UnitOfWork UnitOfWork { get; private set; }

		public void Save()
		{
			UnitOfWork.CommitChanges();
		}
	}
}