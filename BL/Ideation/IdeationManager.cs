using System;
using System.Collections.Generic;
using COI.BL.Domain.Ideation;
using COI.BL.Domain.User;
using COI.DAL.Ideation;

namespace COI.BL.Ideation
{
	public class IdeationManager : IIdeationManager
	{
		private readonly IIdeaRepository _ideaRepository;
		private readonly ICommentRepository _commentRepository;

		public IdeationManager(
			IIdeaRepository ideaRepository, 
			ICommentRepository commentRepository)
		{
			_ideaRepository = ideaRepository;
			_commentRepository = commentRepository;
		}

		public IEnumerable<Idea> GetIdeas()
		{
			return _ideaRepository.ReadIdeas();
		}

		public Idea GetIdea(int ideaId)
		{
			return _ideaRepository.ReadIdea(ideaId);
		}

		public Comment GetComment(int commentId)
		{
			return _commentRepository.ReadComment(commentId);
		}

		public void AddVoteToIdea(int ideaId, Vote vote)
		{
			Idea i = this.GetIdea(ideaId);
			if (i != null)
			{
				vote.Idea = i;
				i.Votes.Add(vote);
				_ideaRepository.UpdateIdea(i);
			}
			else
			{
				throw new ArgumentException("Idea not found.");
			}
		}

		public void AddVoteToComment(int commentId, Vote vote)
		{
			Comment c = this.GetComment(commentId);
			if (c != null)
			{
				vote.Comment = c;
				c.Votes.Add(vote);
				_commentRepository.UpdateComment(c);
			}
			else
			{
				throw new ArgumentException("Comment not found.");
			}
		}

		public Comment AddCommentToIdea(int ideaId, IEnumerable<Field> content)
		{
			Comment comment = new Comment()
			{
				Created = DateTime.Now,
				Fields = (ICollection<Field>) content,
			};
			
			AddCommentToIdea(ideaId, comment);

			return _commentRepository.CreateComment(comment);
		}

		public void AddCommentToIdea(int ideaId, Comment comment)
		{
			Idea i = this.GetIdea(ideaId);
			if (i != null)
			{
				comment.Idea = i;
				i.Comments.Add(comment);
				_ideaRepository.UpdateIdea(i);
			}
			else
			{
				throw new ArgumentException("Idea not found.");
			}
		}

		public IEnumerable<Comment> GetCommentsForIdea(int ideaId)
		{
			return _commentRepository.ReadCommentsForIdea(ideaId);
		}

		public int GetIdeaScore(int ideaId)
		{
			Idea idea = this.GetIdea(ideaId);
			if (idea != null)
			{
				return idea.GetScore();
			}
			else
			{
				throw new ArgumentException("Idea not found.");
			}
		}

		public int GetCommentScore(int commentId)
		{
			Comment comment = this.GetComment(commentId);
			if (comment != null)
			{
				return comment.GetScore();
			}
			else
			{
				throw new ArgumentException("Comment not found.");
			}
		}
	}
}