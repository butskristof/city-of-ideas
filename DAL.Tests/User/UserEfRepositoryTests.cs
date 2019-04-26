using System;
using System.Collections.Generic;
using System.Linq;
using COI.DAL.User.EF;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace COI.DAL.Tests.User
{
	public class UserEfRepositoryTests
	{
		[Fact]
		public void ReadUsers_WithEmptyDb_ReturnsEmptyList()
		{
			// arrange
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				IEnumerable<BL.Domain.User.User> users = null;
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new UserRepository(ctx);
					users = repo.ReadUsers().ToList();
				}

				// assert
				Assert.NotNull(users);
				Assert.Empty(users);
			}
		}

		[Fact]
		public void ReadUsers_WithData_ReturnsList()
		{
			// arrange
			var testUser = new BL.Domain.User.User()
			{
				FirstName = "Test",
				LastName = "Test",
				DateOfBirth = DateTime.Now
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				IEnumerable<BL.Domain.User.User> users = null;
				using (var ctx = factory.CreateContext())
				{
					ctx.Users.Add(testUser);
					ctx.SaveChanges();
					
					// act
					var repo = new UserRepository(ctx);
					users = repo.ReadUsers().ToList();
				}
				
				// assert
				Assert.NotNull(users);
				Assert.Single(users);
				Assert.Equal(testUser, users.FirstOrDefault());
			}
		}

		[Fact]
		public void ReadUser_WithValidId_ReturnsObject()
		{
			// arrange
			var testUser = new BL.Domain.User.User()
			{
				FirstName = "Test",
				LastName = "Test",
				DateOfBirth = DateTime.Now
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				BL.Domain.User.User retrievedUser = null;
				using (var ctx = factory.CreateContext())
				{
					ctx.Users.Add(testUser);
					ctx.SaveChanges();
					
					// act
					var id = ctx.Users.FirstOrDefault().UserId;
					var repo = new UserRepository(ctx);
					retrievedUser = repo.ReadUser(id);
				}
				
				// assert
				Assert.NotNull(retrievedUser);
				Assert.Equal(testUser, retrievedUser);
			}
		}
		
		[Fact]
		public void ReadUser_WithInvalidId_ReturnsNull()
		{
			var invalidId = 0;
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				BL.Domain.User.User retrievedUser = null;
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new UserRepository(ctx);
					retrievedUser = repo.ReadUser(invalidId);
				}
				
				// assert
				Assert.Null(retrievedUser);
			}
		}

		[Fact]
		public void CreateUser_WithValidObject_ReturnsObjectWithId()
		{
			// arrange
			var testUser = new BL.Domain.User.User()
			{
				FirstName = "Test",
				LastName = "Test",
				DateOfBirth = DateTime.Now
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				BL.Domain.User.User retrievedUser = null;
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new UserRepository(ctx);
					retrievedUser = repo.CreateUser(testUser);
				}
				
				// assert
				Assert.NotNull(retrievedUser);
				Assert.Equal(testUser, retrievedUser);
				Assert.Equal(1, retrievedUser.UserId);
			}
		}
		
		[Fact]
		public void CreateUser_WithDuplicateObject_Throws()
		{
			// arrange
			var testUser = new BL.Domain.User.User()
			{
				FirstName = "Test",
				LastName = "Test",
				DateOfBirth = DateTime.Now
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				Action duplicateAdd = null;
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new UserRepository(ctx);
					repo.CreateUser(testUser);
					// this will throw an exception
					duplicateAdd = () => repo.CreateUser(testUser);
					
					// assert
					var exception = Assert.Throws<ArgumentException>(duplicateAdd);
					Assert.Equal("User already in database.", exception.Message);
				}
			}
		}

        public static IEnumerable<object[]> GetUsers()
		{
			yield return new object[]
			{
				new BL.Domain.User.User(),
				"SQLite Error 19: 'NOT NULL constraint failed: Users.FirstName'."
			};
			yield return new object[]
			{
				new BL.Domain.User.User() { FirstName = ""},
				"SQLite Error 19: 'NOT NULL constraint failed: Users.LastName'."
			};
			yield return new object[]
			{
				new BL.Domain.User.User() { LastName = ""},
				"SQLite Error 19: 'NOT NULL constraint failed: Users.FirstName'."
			};
		}

		[Theory]
		[MemberData(nameof(GetUsers))]
		public void CreateUser_WithInvalidObject_Throws(
			BL.Domain.User.User toCreate,
			string msg
		)
		{
			// arrange
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				Action result = null;
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new UserRepository(ctx);
					result = () => repo.CreateUser(toCreate);
					
					// assert
					var exception = Assert.Throws<ArgumentException>(result);
					Assert.Equal(msg, exception.Message);
				}
			}
		}
		
		[Fact]
		public void UpdateUser_WithValidObject_ReturnsObjectWithId()
		{
			// arrange
			var testUser = new BL.Domain.User.User()
			{
				FirstName = "Test",
				LastName = "Test",
				DateOfBirth = DateTime.Now
			};
			var newFirst = "New Test";
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				BL.Domain.User.User retrievedUser = null;
				using (var ctx = factory.CreateContext())
				{
					ctx.Users.Add(testUser);
					ctx.SaveChanges();

					testUser.FirstName = newFirst;
					
					// act
					var repo = new UserRepository(ctx);
					retrievedUser = repo.UpdateUser(testUser);
				}
				
				// assert
				Assert.NotNull(retrievedUser);
				Assert.Equal(testUser, retrievedUser);
				Assert.Equal(newFirst, retrievedUser.FirstName);
			}
		}

		[Fact]
		public void UpdateUser_WithInvalidId_Throws()
		{
			// arrange
			var testUser = new BL.Domain.User.User()
			{
				FirstName = "Test",
				LastName = "Test",
				DateOfBirth = DateTime.Now
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				using (var ctx = factory.CreateContext())
				{
					ctx.Users.Add(testUser);
					ctx.SaveChanges();

					testUser.UserId = 0;

					// act
					var repo = new UserRepository(ctx);
					Action result = () => repo.UpdateUser(testUser);

					// assert
					var exception = Assert.Throws<ArgumentException>(result);
					Assert.Equal("User to update not found.", exception.Message);
				}
			}
		}

		[Theory]
		[MemberData(nameof(GetUsers))]
		public void UpdateUser_WithInvalidObject_Throws(
			BL.Domain.User.User updatedValues, string irrelevant
		)
		{
			// arrange
			var testUser = new BL.Domain.User.User()
			{
				FirstName = "Test",
				LastName = "Test",
				DateOfBirth = DateTime.Now
			};

			using (var factory = new CityOfIdeasDbContextFactory())
			{
				using (var ctx = factory.CreateContext())
				{
					ctx.Users.Add(testUser);
					ctx.SaveChanges();

					testUser.FirstName = updatedValues.FirstName;
					testUser.LastName = updatedValues.LastName;
					testUser.DateOfBirth = updatedValues.DateOfBirth;

					// act
					var repo = new UserRepository(ctx);
					Action result = () => repo.UpdateUser(testUser);

					// assert
					var exception = Assert.Throws<DbUpdateException>(result);
				}
			}
		}

		[Fact]
		public void DeleteUser_WithValidId_DeletesUser()
		{
			// arrange
			var testUser = new BL.Domain.User.User()
			{
				FirstName = "Test",
				LastName = "Test",
				DateOfBirth = DateTime.Now
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				BL.Domain.User.User retrievedUser0 = null;
				BL.Domain.User.User retrievedUser1 = null;
				using (var ctx = factory.CreateContext())
				{
					ctx.Users.Add(testUser);
					ctx.SaveChanges();
					
					// act
					var id = ctx.Users.FirstOrDefault().UserId;
					var repo = new UserRepository(ctx);
					retrievedUser0 = repo.DeleteUser(id);
					retrievedUser1 = repo.ReadUser(id);
				}
				
				// assert
				Assert.NotNull(retrievedUser0);
				Assert.Equal(testUser, retrievedUser0);
				Assert.Null(retrievedUser1);
			}
		}

		[Fact]
		public void DeleteUser_WithInvalidId_Throws()
		{
			// arrange
			var invalidId = 0;
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new UserRepository(ctx);
					Action result = () => repo.DeleteUser(invalidId);
					
					// assert
					var exception = Assert.Throws<ArgumentException>(result);
					Assert.Equal("User to delete not found.", exception.Message);
				}
			}
		}
	}
}