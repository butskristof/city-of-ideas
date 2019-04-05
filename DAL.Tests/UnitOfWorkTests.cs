using System;
using COI.BL.Domain.Common;
using COI.BL.Domain.Ideation;
using COI.BL.Domain.User;
using COI.DAL.Ideation.EF;
using COI.DAL.User.EF;
using Xunit;

namespace COI.DAL.Tests
{
	public class UnitOfWorkTests
	{
		// test whether context doesn't save changes until unit of work issues commit
//		[Fact]
//		public void UnitOfWork_DoesntSaveOnUpdate()
//		{
//			// arrange
//			using (var factory = new CityOfIdeasDbContextFactory())
//			{
//				const int userId = 1;
//				const int ideaId = 2;
//				const int value = 1;
//
//				// act
//				using (var ctx = factory.CreateContext(true, true))
//				{
//					var uow = new UnitOfWork(ctx);
//
//					var userRepo = new UserRepository(uow);
//					var ideaRepo = new IdeaRepository(uow);
//
//					Vote vote = new Vote() {Value = value};
//					BL.Domain.User.User user = userRepo.ReadUser(userId);
//					user.Votes.Add(vote);
//					userRepo.UpdateUser(user);
//					
//					// intermediate test to check user isn't saved
//					using (var innerCtx = factory.CreateContext(false, true))
//					{
//						var innerUserRepo = new UserRepository(innerCtx);
//
//						BL.Domain.User.User u = innerUserRepo.ReadUser(userId);
//						Assert.Empty(u.Votes);
//					}
//
//					Idea idea = ideaRepo.ReadIdea(ideaId);
//					idea.Votes.Add(vote);
//					ideaRepo.UpdateIdea(idea);
//					
//					// intermediate test to check user isn't saved
//					using (var innerCtx = factory.CreateContext(false, true))
//					{
//						var innerIdeaRepo = new IdeaRepository(innerCtx);
//
//						Idea i = innerIdeaRepo.ReadIdea(ideaId);
//						Assert.Empty(i.Votes);
//					}
//
//
//					uow.CommitChanges();
//				}
//
//				// assert
//				using (var ctx = factory.CreateContext(false))
//				{
//					var userRepo = new UserRepository(ctx);
//					var ideaRepo = new IdeaRepository(ctx);
//
//					BL.Domain.User.User user = userRepo.ReadUser(userId);
//					Idea idea = ideaRepo.ReadIdea(ideaId);
//
//					Assert.Single(user.Votes);
//					Assert.Single(idea.Votes);
//				}
//			}
//		}
	}
}