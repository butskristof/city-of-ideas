using COI.DAL.EF;

namespace COI.DAL.User.EF
{
	public class UserRepository : EfRepository, IUserRepository
	{
		public UserRepository(CityOfIdeasDbContext ctx) : base(ctx)
		{
		}

		public UserRepository(UnitOfWork uow) : base(uow)
		{
		}

		public BL.Domain.User.User ReadUser(int userId)
		{
			return _ctx.Users.Find(userId);
		}

		public void UpdateUser(BL.Domain.User.User user)
		{
			var userToUpdate = _ctx.Ideas.Find(user.UserId);
			_ctx.Entry(userToUpdate).CurrentValues.SetValues(user);
			_ctx.SaveChanges();
		}
	}
}