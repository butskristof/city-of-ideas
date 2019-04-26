using System;
using System.Collections.Generic;
using System.Linq;
using COI.BL.Domain.Ideation;
using COI.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace COI.DAL.Ideation.EF
{
	public class CommentRepository : EfRepository, ICommentRepository
	{
		public CommentRepository(CityOfIdeasDbContext ctx) : base(ctx)
		{
		}
		
		public IEnumerable<Comment> ReadCommentsForIdea(int ideaId)
		{
			return _ctx
				.Comments
				.Where(c => c.Idea.IdeaId == ideaId)
				.AsEnumerable();
		}

		public Comment ReadComment(int commentId)
		{
			return _ctx.Comments.Find(commentId);
		}


		public Comment CreateComment(Comment comment)
		{
			if (ReadComment(comment.CommentId) != null)
			{
				throw new ArgumentException("Comment already in database.");
			}

			try
			{
				_ctx.Comments.Add(comment);
				_ctx.SaveChanges();
				
				return comment;
			}
			catch (DbUpdateException exception)
			{
				var msg = exception.InnerException == null ? "Invalid object." : exception.InnerException.Message;
				throw new ArgumentException(msg);
			}
		}

		public Comment UpdateComment(Comment updatedComment)
		{
			var entryToUpdate = ReadComment(updatedComment.CommentId);

			if (entryToUpdate == null)
			{
				throw new ArgumentException("Comment to update not found.");
			}
			
			_ctx.Entry(entryToUpdate).CurrentValues.SetValues(updatedComment);
			_ctx.SaveChanges();

			return ReadComment(updatedComment.CommentId);
		}

		public Comment DeleteComment(int commentId)
		{
			var toDelete = ReadComment(commentId);
			if (toDelete == null)
			{
				throw new ArgumentException("Comment to delete not found.");
			}

			_ctx.Comments.Remove(toDelete);
			_ctx.SaveChanges();

			return toDelete;
		}
	}
}