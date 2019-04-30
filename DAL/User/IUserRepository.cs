using System.Collections.Generic;

namespace COI.DAL.User
{
	public interface IUserRepository
	{
		IEnumerable<BL.Domain.User.User> ReadUsers();
		BL.Domain.User.User ReadUser(string userId);
		BL.Domain.User.User CreateUser(BL.Domain.User.User user);
		BL.Domain.User.User UpdateUser(BL.Domain.User.User user);
		BL.Domain.User.User DeleteUser(string userId);
	}
}