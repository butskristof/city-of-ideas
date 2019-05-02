using System.Collections.Generic;
using COI.BL.Domain.Ideation;
using COI.BL.Domain.User;

namespace COI.BL.Ideation
{
	public interface IIdeationManager
	{
		IEnumerable<Domain.Ideation.Ideation> GetIdeations();
		Domain.Ideation.Ideation GetIdeation(int ideationId);
		Domain.Ideation.Ideation AddIdeation(string title, ICollection<Field> fields, int projectPhaseId);
		Domain.Ideation.Ideation ChangeIdeation(int id, string title, int projectPhaseId);
		Domain.Ideation.Ideation RemoveIdeation(int id);
		
		IEnumerable<Idea> GetIdeas();
		IEnumerable<Idea> GetIdeasForIdeation(int ideationId);
		Idea GetIdea(int ideaId);
		Idea AddIdea(string title, ICollection<Field> fields, int ideationId);
		Idea ChangeIdea(int id, string title, int ideationId);
		Idea RemoveIdea(int id);

		IEnumerable<Comment> GetCommentsForIdea(int ideaId);
		Comment GetComment(int commentId);
//		Comment AddComment(ICollection<Field> content, int ideaId);
		Comment AddCommentToIdea(IEnumerable<Field> content, int ideaId);
//		void AddCommentToIdea(int ideaId, Comment comment);
		Comment ChangeIdeaComment(int id, ICollection<Field> content, int userId, int ideaId);
		Comment RemoveComment(int id);
		
		void AddVoteToIdea(Vote vote, int ideaId);
		void AddVoteToIdeation(Vote vote, int ideationId);
		void AddVoteToComment(Vote vote, int commentId);

		int GetIdeaScore(int ideaId);
		int GetIdeationScore(int ideationId);
		int GetCommentScore(int commentId);

	}
}