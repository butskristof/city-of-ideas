namespace COI.DAL.User
{
	public interface IUserRepository
	{
		BL.Domain.User.User ReadUser(int userId);
		void UpdateUser(BL.Domain.User.User user);
	}
}