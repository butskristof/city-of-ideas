using System;
using System.Data.Common;
using COI.DAL.EF;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace COI.DAL.Tests
{
	public class CityOfIdeasDbContextFactory : IDisposable
	{
		private DbConnection _connection;

		private DbContextOptions<CityOfIdeasDbContext> CreateOptions()
		{
			return new DbContextOptionsBuilder<CityOfIdeasDbContext>()
				.UseSqlite(_connection)
				.UseLazyLoadingProxies()
				.Options;
		}

		public CityOfIdeasDbContext CreateContext()
		{
			if (_connection == null)
			{
				_connection = new SqliteConnection("DataSource=:memory:");
				_connection.Open();
			}
			
			var ret = new CityOfIdeasDbContext(CreateOptions());
			ret.Database.EnsureCreated();
			return ret;
		}

		public void Dispose()
		{
			if (_connection != null)
			{
				_connection.Close();
				_connection = null;
			}
		}
	}

	public class CityOfIdeasDbContextSeeder
	{
		public static void Seed(CityOfIdeasDbContext ctx)
		{
			// TODO for later use, when db gets hosting elsewehere
			// Initializer.Seed method should be moved here
			
			throw new NotImplementedException();
		}
	}
}