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

		public EfRepository(UnitOfWork uow)
		{
			if (uow == null)
			{
				throw new ArgumentNullException("uow");
			}
			
			_ctx = uow.Context;
		}
	}
}