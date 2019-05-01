//using System;
//using COI.BL.Domain.Common;
//using COI.BL.Domain.User;
//using COI.DAL.User.EF;
//using Xunit;
//
//namespace COI.DAL.Tests.User
//{
//	public class UserEfRepositoryTests
//	{
//		[Fact]
//		public void ReadUser_WithInvalidId_ReturnsNull()
//		{
//			// arrange
//			using (var factory = new CityOfIdeasDbContextFactory())
//			{
//				const int id = 0;
//				BL.Domain.User.User result = null;
//				using (var ctx = factory.CreateContext(false))
//				{
//					// act
//					var repo = new UserRepository(ctx);
//					result = repo.ReadUser(id);
//				}
//				
//				// assert
//				Assert.Null(result);
//			}
//		}
//
//		[Fact]
//		public void ReadUser_WithValidId_ReturnsUser()
//		{
//			// arrange
//			using (var factory = new CityOfIdeasDbContextFactory())
//			{
//				const int id = 1;
//				const String expectedFirstName = "Kristof";
//				const String expectedLastName = "Buts";
//				BL.Domain.User.User result = null;
//				using (var ctx = factory.CreateContext(true))
//				{
//					// act
//					var repo = new UserRepository(ctx);
//					result = repo.ReadUser(id);
//					
//					// assert
//					Assert.NotNull(result);
//					Assert.Equal(id, result.UserId);
//					Assert.Equal(expectedFirstName, result.FirstName);
//					Assert.Equal(expectedLastName, result.LastName);
//					Assert.Equal(Gender.Male, result.Gender);
//					Assert.Equal(Role.UserVerified, result.Role);
//				}
//			}
//		}
//
//		[Fact]
//		public void UpdateUser_WithModifiedValues_UpdatesUser()
//		{
//			// arrange
//			using (var factory = new CityOfIdeasDbContextFactory())
//			{
//				const int id = 1;
//				const String newFirstName = "New First Name";
//				const String expectedLastName = "Buts";
//				
//				// act
//				using (var ctx = factory.CreateContext(true))
//				{
//					var repo = new UserRepository(ctx);
//					BL.Domain.User.User user = repo.ReadUser(id);
//					user.FirstName = newFirstName;
//					
//					repo.UpdateUser(user);
//				}
//				
//				// assert
//				BL.Domain.User.User result = null;
//				using (var ctx = factory.CreateContext(false))
//				{
//					var repo = new UserRepository(ctx);
//					result = repo.ReadUser(id);
//					
//					Assert.NotNull(result);
//					Assert.NotNull(result);
//					Assert.Equal(id, result.UserId);
//					Assert.Equal(newFirstName, result.FirstName);
//					Assert.Equal(expectedLastName, result.LastName);
//					Assert.Equal(Gender.Male, result.Gender);
//					Assert.Equal(Role.UserVerified, result.Role);
//				}
//			}
//		}
//	}
//}