using System.Collections.Generic;
using COI.BL.Domain.Ideation;

namespace COI.DAL.Ideation
{
	public interface ICommentRepository
	{
		Comment ReadComment(int commentId);
		IEnumerable<Comment> ReadCommentsForIdea(int ideaId);

		Comment CreateComment(Comment newComment);

		void UpdateComment(Comment comment);
	}
}