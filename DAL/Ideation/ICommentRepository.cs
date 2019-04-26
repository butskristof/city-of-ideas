using System.Collections.Generic;
using COI.BL.Domain.Ideation;

namespace COI.DAL.Ideation
{
	public interface ICommentRepository
	{
		IEnumerable<Comment> ReadCommentsForIdea(int ideaId);
		Comment ReadComment(int commentId);
		Comment CreateComment(Comment comment);
		Comment UpdateComment(Comment updatedComment);
		Comment DeleteComment(int commentId);
	}
}