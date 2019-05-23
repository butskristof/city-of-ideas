using COI.BL.Domain.Ideation;
using COI.BL.Domain.Questionnaire;
using COI.BL.Domain.User;

namespace COI.BL.User
{
	/// <summary>
	/// This interface will only concern user-related domain objects,
	/// actual user functions will be handles by Identity-compatible UserManager 
	/// </summary>
	public interface IUserManager
	{
		Domain.User.User GetUser(string userId);
		
		void AddCommentToUser(Comment comment, string userId);
		void AddAnswerToUser(Answer answer, string userId);
		void AddIdeaToUser(Idea idea, string userId);

		Vote GetVote(int voteId);
		Vote AddVoteToUser(int value, string userId);
		Vote AddVoteWithEmail(int value, string email);
		Vote AddAnonymousVote(int value);
		Vote ChangeVoteValue(int id, int value);
		Vote RemoveVote(int voteId);

		Vote GetEmailVoteForIdea(int ideaId, string email);
		Vote GetEmailVoteForComment(int commentId, string email);
		Vote GetEmailVoteForIdeation(int ideationId, string email);
		Vote GetVoteForIdea(int ideaId, string userId);
		Vote GetVoteForIdeation(int ideationId, string userId);
		Vote GetVoteForComment(int commentId, string userId);

		void AddPictureLocationToUser(string userId, string imgpath);
	}
}