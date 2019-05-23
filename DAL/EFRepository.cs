using COI.DAL.EF;

namespace COI.DAL
{
	/// <summary>
	/// Base class for repositories using Entity Framework, provides a base constructor which handles a
	/// DbContext as parameter
	/// </summary>
	public class EfRepository
	{
		protected readonly CityOfIdeasDbContext _ctx;

		// resolve through DI
		public EfRepository(CityOfIdeasDbContext ctx)
		{
			_ctx = ctx;
		}
	}
}