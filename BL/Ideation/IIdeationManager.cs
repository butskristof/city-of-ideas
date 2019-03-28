using System.Collections.Generic;
using COI.BL.Domain.Ideation;
using COI.BL.Domain.User;

namespace COI.BL.Ideation
{
	public interface IIdeationManager
	{
		IEnumerable<Idea> GetIdeas();
		Idea GetIdea(int ideaId);
		Comment GetComment(int commentId);
		
		void AddVoteToIdea(int ideaId, Vote vote);
		void AddVoteToComment(int commentId, Vote vote);

		int GetIdeaScore(int ideaId);
		int GetCommentScore(int commentId);

		Comment AddCommentToIdea(int ideaId, IEnumerable<Field> content);
		void AddCommentToIdea(int ideaId, Comment comment);
		IEnumerable<Comment> GetCommentsForIdea(int ideaId);
	}
}