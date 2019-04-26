using System;
using COI.DAL.EF;

namespace COI.DAL
{
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