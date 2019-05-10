using System.Threading.Tasks;
using COI.BL.Domain.Ideation;
using COI.BL.Domain.Questionnaire;
using COI.BL.Domain.User;

namespace COI.BL.User
{
	public interface IUserManager
	{
		Domain.User.User GetUser(string userId);
		
		void AddCommentToUser(Comment comment, string userId);
		void AddAnswerToUser(Answer answer, string userId);

		Vote GetVote(int voteId);
		Vote AddVoteToUser(int value, string userId);
		Vote AddVoteWithEmail(int value, string email);
		Vote AddAnonymousVote(int value);
//		Vote AddVoteToIdea(int value, int userId, int ideaId);
//		Vote ChangeIdeaVote(int id, int value, string userId, int ideaId);
		Vote ChangeVoteValue(int id, int value);
		Vote RemoveVote(int voteId);

		Vote GetVoteForIdea(int ideaId, string userId);
		Vote GetEmailVoteForIdea(int ideaId, string email);
		Vote GetEmailVoteForComment(int commentId, string email);
		Vote GetEmailVoteForIdeation(int ideationId, string email);
		Vote GetVoteForIdeation(int ideationId, string userId);
		Vote GetVoteForComment(int commentId, string userId);

		void AddPictureLocationToUser(string userId, string imgpath);
	}
}