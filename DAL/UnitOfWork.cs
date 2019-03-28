using COI.DAL.EF;

namespace COI.DAL
{
	public class UnitOfWork
	{
		public UnitOfWork()
		{
			Context = new CityOfIdeasDbContext(true);
		}

		public UnitOfWork(CityOfIdeasDbContext context)
		{
			Context = context;
		}

		internal readonly CityOfIdeasDbContext Context; // readonly

		public void CommitChanges()
		{
			Context.CommitChanges();
		}
	}
}