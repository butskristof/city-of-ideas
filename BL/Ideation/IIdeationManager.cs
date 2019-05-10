using System.Collections.Generic;
using COI.BL.Domain.Ideation;
using COI.BL.Domain.User;
using Microsoft.AspNetCore.Http;

namespace COI.BL.Ideation
{
	public interface IIdeationManager
	{
		IEnumerable<Domain.Ideation.Ideation> GetIdeations();
		Domain.Ideation.Ideation GetIdeation(int ideationId);
		Domain.Ideation.Ideation AddIdeation(string title, int projectPhaseId);
		Domain.Ideation.Ideation ChangeIdeation(int id, string title, int projectPhaseId);
		Domain.Ideation.Ideation RemoveIdeation(int id);
		
		IEnumerable<Idea> GetIdeas();
		IEnumerable<Idea> GetIdeasForIdeation(int ideationId);
		Idea GetIdea(int ideaId);
		Idea AddIdea(string title, int ideationId);
		Idea ChangeIdea(int id, string title, int ideationId);
		Idea RemoveIdea(int id);

		IEnumerable<Comment> GetCommentsForIdea(int ideaId);
		Comment GetComment(int commentId);
		Comment AddCommentToIdea(int ideaId);
//		Comment ChangeIdeaComment(int id, ICollection<Field> content, int userId, int ideaId);
		Comment RemoveComment(int id);

		Field GetField(int fieldId);
		Field AddFieldToIdea(FieldType type, string content, int ideaId);
		Field AddFieldToIdeation(FieldType type, string content, int ideationId);
		Field AddFieldToComment(FieldType type, string content, int commentId);
		Field ChangeIdeaField(int id, FieldType type, string content, int ideaId);
		Field ChangeIdeationField(int id, FieldType type, string content, int ideationId);
		Field ChangeCommentField(int id, FieldType type, string content, int commentId);
		Field RemoveField(int fieldId);
		
		void AddVoteToIdea(Vote vote, int ideaId);
		void AddVoteToIdeation(Vote vote, int ideationId);
		void AddVoteToComment(Vote vote, int commentId);

		int GetIdeaScore(int ideaId);
		int GetIdeationScore(int ideationId);
		int GetCommentScore(int commentId);

	}
}