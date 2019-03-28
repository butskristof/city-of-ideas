using COI.BL.Domain.Ideation;
using COI.BL.Domain.Questionnaire;
using COI.BL.Domain.User;

namespace COI.BL.User
{
	public interface IUserManager
	{
		Domain.User.User GetUser(int userId);
		
		Vote AddVoteToUser(int userId, int value);
		void AddVoteToUser(int userId, Vote vote);
		void AddCommentToUser(int userId, Comment comment);
		void AddAnswerToUser(int userId, Answer answer);
	}
}