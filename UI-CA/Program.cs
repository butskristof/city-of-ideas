using System;
using System.Linq;
using COI.BL.Domain.Ideation;
using COI.DAL.EF;
using COI.DAL.Ideation.EF;

namespace COI.UI.CA
{
	class Program
	{
		static void Main(string[] args)
		{
			var ctx = new CityOfIdeasDbContext();
			ctx.Database.EnsureDeleted();
			ctx.Database.EnsureCreated();
			CityOfIdeasInitializer.Seed(ctx);
		}
	}
}