using COI.DAL;

namespace COI.BL
{
	public class UnitOfWorkManager
	{
		public UnitOfWorkManager()
		{
			UnitOfWork = new UnitOfWork();
		}

		internal UnitOfWork UnitOfWork { get; private set; }

		public void Save()
		{
			UnitOfWork.CommitChanges();
		}
	}
}