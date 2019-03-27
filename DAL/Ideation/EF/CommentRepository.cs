using System.Collections.Generic;
using System.Linq;
using COI.BL.Domain.Ideation;
using COI.DAL.EF;

namespace COI.DAL.Ideation.EF
{
	public class CommentRepository : EfRepository, ICommentRepository
	{
		public CommentRepository(CityOfIdeasDbContext ctx) : base(ctx)
		{
		}

		public CommentRepository(UnitOfWork uow) : base(uow)
		{
		}

		public Comment ReadComment(int commentId)
		{
			return this._ctx.Comments.Find(commentId);
		}

		public IEnumerable<Comment> ReadCommentsForIdea(int ideaId)
		{
			return this._ctx.Comments.Where(c => c.Idea.IdeaId == ideaId).AsEnumerable();
		}

		public Comment CreateComment(Comment newComment)
		{
			_ctx.Comments.Add(newComment);
			_ctx.SaveChanges();
			return newComment;
		}

		public void UpdateComment(Comment comment)
		{
			var commentToUpdate = _ctx.Comments.Find(comment.CommentId);
			_ctx.Entry(commentToUpdate).CurrentValues.SetValues(comment);
			_ctx.SaveChanges();
		}
	}
}