using COI.DAL.EF;

namespace COI.DAL
{
	public class UnitOfWork
	{
		public UnitOfWork()
		{
			Context = new CityOfIdeasDbContext(true);
		}

		internal readonly CityOfIdeasDbContext Context; // readonly

		public void CommitChanges()
		{
			Context.CommitChanges();
		}
	}
}