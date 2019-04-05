using COI.DAL.EF;

namespace COI.DAL
{
	public interface IUnitOfWork
	{
		void StartUnitOfWork();
		void EndUnitOfWork();
	}

	public class UnitOfWork : IUnitOfWork
	{

		public UnitOfWork(CityOfIdeasDbContext context)
		{
			Context = context;
		}

		internal readonly CityOfIdeasDbContext Context; // readonly

		public void StartUnitOfWork()
		{
			Context.StartUnitOfWork();
		}

		public void EndUnitOfWork()
		{
			Context.EndUnitOfWork();
		}

	}
}