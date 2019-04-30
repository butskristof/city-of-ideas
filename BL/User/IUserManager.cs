using COI.BL.Domain.Ideation;
using COI.BL.Domain.Questionnaire;
using COI.BL.Domain.User;

namespace COI.BL.User
{
	public interface IUserManager
	{
		Domain.User.User GetUser(string userId);
		
		void AddCommentToUser(Comment comment, string userId);
		void AddAnswerToUser(string userId, Answer answer);

		Vote GetVote(int voteId);
		Vote AddVoteToUser(int value, string userId);
//		Vote AddVoteToIdea(int value, int userId, int ideaId);
//		Vote ChangeIdeaVote(int id, int value, int userId, int ideaId);
		Vote RemoveVote(int voteId);
	}
}