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

		public CityOfIdeasDbContext CreateContext(bool addTestData = false)
		{
			if (_connection == null)
			{
				_connection = new SqliteConnection("DataSource=:memory:");
				_connection.Open();

				var options = CreateOptions();
				using (var ctx = new CityOfIdeasDbContext(options))
				{
					ctx.Database.EnsureCreated();
					if (addTestData)
					{
						CityOfIdeasInitializer.Seed(ctx);
					}
				}
			}
			
			return new CityOfIdeasDbContext(CreateOptions());
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
}