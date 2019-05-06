using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using COI.BL.Application;
using COI.BL.Domain.Ideation;
using COI.BL.Domain.Project;
using COI.BL.Domain.User;
using COI.BL.Project;
using COI.BL.User;
using COI.DAL.Ideation;
using COI.DAL.Ideation.EF;
using COI.DAL.Project;
using Microsoft.EntityFrameworkCore.Storage;

namespace COI.BL.Ideation
{
	public class IdeationManager : IIdeationManager
	{
		private readonly IIdeaRepository _ideaRepository;
		private readonly IIdeationRepository _ideationRepository;
		private readonly ICommentRepository _commentRepository;
		private readonly IProjectManager _projectManager;

		public IdeationManager(IIdeaRepository ideaRepository, IIdeationRepository ideationRepository, ICommentRepository commentRepository, IProjectManager projectManager)
		{
			_ideaRepository = ideaRepository;
			_ideationRepository = ideationRepository;
			_commentRepository = commentRepository;
			_projectManager = projectManager;
		}

		#region Ideations


		public IEnumerable<Domain.Ideation.Ideation> GetIdeations()
		{
			return _ideationRepository.ReadIdeations();
		}

		public Domain.Ideation.Ideation GetIdeation(int ideationId)
		{
			return _ideationRepository.ReadIdeation(ideationId);
		}

		public Domain.Ideation.Ideation AddIdeation(string title, ICollection<Field> fields, int projectPhaseId)
		{
			ProjectPhase phase = _projectManager.GetProjectPhase(projectPhaseId);
			if (phase == null)
			{
				throw new ArgumentException("Project phase not found.");
			}
			
			Domain.Ideation.Ideation ideation = new Domain.Ideation.Ideation()
			{
				Title = title,
				Fields = fields,
				ProjectPhase = phase
			};

			return AddIdeation(ideation);
		}

		private Domain.Ideation.Ideation AddIdeation(Domain.Ideation.Ideation ideation)
		{
			Validate(ideation);
			return _ideationRepository.CreateIdeation(ideation);
		}

		public Domain.Ideation.Ideation ChangeIdeation(int id, string title, int projectPhaseId)
		{
			Domain.Ideation.Ideation toChange = GetIdeation(id);
			if (toChange != null)
			{
				ProjectPhase phase = _projectManager.GetProjectPhase(projectPhaseId);
				if (phase == null)
				{
					throw new ArgumentException("Project phase not found", "projectPhaseId");
				}
				
				toChange.Title = title;
				toChange.ProjectPhase = phase;
				
				Validate(toChange);
				return _ideationRepository.UpdateIdeation(toChange);
			}
			
			throw new ArgumentException("Ideation not found.", "id");
		}

		public Domain.Ideation.Ideation RemoveIdeation(int id)
		{
			return _ideationRepository.DeleteIdeation(id);
		}

		private void Validate(Domain.Ideation.Ideation ideation)
		{
			Validator.ValidateObject(ideation, new ValidationContext(ideation), true);
		}
		
		#endregion

		#region Ideas

		public IEnumerable<Idea> GetIdeas()
		{
			return _ideaRepository.ReadIdeas();
		}

		public IEnumerable<Idea> GetIdeasForIdeation(int ideationId)
		{
			return _ideationRepository.ReadIdeasForIdeation(ideationId);
		}

		public Idea GetIdea(int ideaId)
		{
			return _ideaRepository.ReadIdea(ideaId);
		}

		public Idea AddIdea(string title, ICollection<Field> fields, int ideationId)
		{
			Domain.Ideation.Ideation ideation = GetIdeation(ideationId);
			if (ideation == null)
			{
				throw new ArgumentException("Ideation not found.");
			}
			
			Idea idea = new Idea()
			{
				Title = title,
				Fields = fields,
				Ideation = ideation
			};

			return AddIdea(idea);
		}

		private Idea AddIdea(Idea idea)
		{
			Validate(idea);
			return _ideaRepository.CreateIdea(idea);
		}

		public Idea ChangeIdea(int id, string title, int ideationId)
		{
			Idea toChange = GetIdea(id);
			if (toChange != null)
			{
				Domain.Ideation.Ideation ideation = GetIdeation(ideationId);
				if (ideation == null)
				{
					throw new ArgumentException("Ideation not found.", "ideationId");
				}

				toChange.Title = title;
				toChange.Ideation = ideation;
				
				Validate(toChange);
				return _ideaRepository.UpdateIdea(toChange);
			}
			
			throw new ArgumentException("Idea not found.", "id");
		}

		public Idea RemoveIdea(int id)
		{
			return _ideaRepository.DeleteIdea(id);
		}

		private void Validate(Idea idea)
		{
			Validator.ValidateObject(idea, new ValidationContext(idea), true);
		}

		#endregion

		#region Comments
		
		public IEnumerable<Comment> GetCommentsForIdea(int ideaId)
		{
			return _commentRepository.ReadCommentsForIdea(ideaId);
		}

		public Comment GetComment(int commentId)
		{
			return _commentRepository.ReadComment(commentId);
		}

		public Comment AddCommentToIdea(IEnumerable<Field> content, int ideaId)
		{
			Idea idea = GetIdea(ideaId);
			if (idea == null)
			{
				throw new ArgumentException("Idea not found.", "ideaId");
			}
			
			Comment comment = new Comment()
			{
				Fields = (ICollection<Field>) content,
				Idea = idea,
			};

			return AddComment(comment);
		}

		private Comment AddComment(Comment comment)
		{
			Validate(comment);
			return _commentRepository.CreateComment(comment);
		}

		public Comment ChangeIdeaComment(int id, ICollection<Field> content, int userId, int ideaId)
		{
			throw new NotImplementedException();
//			Comment toChange = GetComment(id);
//			if (toChange != null)
//			{
//				Idea idea = GetIdea(ideaId);
//				if (idea == null)
//				{
//					throw new ArgumentException("Idea not found.", "ideaId");
//				}
//
//				Domain.User.User user = _userManager.GetUser(userId);
//				if (user == null)
//				{
//					throw new ArgumentException("User not found.", "userId");
//				}
//			
//				toChange.Fields = content;
//				toChange.Idea = idea;
//				toChange.User = user;
//				
//				Validate(toChange);
//				return _commentRepository.UpdateComment(toChange);
//			}
//			
//			throw new ArgumentException("Idea not found.", "id");
		}

		public Comment RemoveComment(int id)
		{
			return _commentRepository.DeleteComment(id);
		}

		private void Validate(Comment comment)
		{
			Validator.ValidateObject(comment, new ValidationContext(comment), true);
		}

		#endregion

		public void AddVoteToIdea(Vote vote, int ideaId)
		{
			Idea idea = GetIdea(ideaId);
			if (idea == null)
			{
				throw new ArgumentException("Idea not found.");
			}

			vote.Idea = idea;
			idea.Votes.Add(vote);
			_ideaRepository.UpdateIdea(idea);
		}

		public void AddVoteToIdeation(Vote vote, int ideationId)
		{
			Domain.Ideation.Ideation ideation = GetIdeation(ideationId);
			if (ideation == null)
			{
				throw new ArgumentException("Ideation not found.");
			}

			vote.Ideation = ideation;
			ideation.Votes.Add(vote);
			_ideationRepository.UpdateIdeation(ideation);
		}

		public void AddVoteToComment(Vote vote, int commentId)
		{
			Comment c = GetComment(commentId);
			if (c == null)
			{
				throw new ArgumentException("Comment not found.");
			}
			
			vote.Comment = c;
			c.Votes.Add(vote);
			_commentRepository.UpdateComment(c);
		}
		public int GetIdeaScore(int ideaId)
		{
			Idea idea = GetIdea(ideaId);
			if (idea != null)
			{
				return idea.GetScore();
			}
			else
			{
				throw new ArgumentException("Idea not found.");
			}
		}

		public int GetIdeationScore(int ideationId)
		{
			Domain.Ideation.Ideation ideation = GetIdeation(ideationId);
			if (ideation != null)
			{
				return ideation.GetScore();
			}
			
			throw new ArgumentException("Ideation not found.");
		}

		public int GetCommentScore(int commentId)
		{
			Comment comment = GetComment(commentId);
			if (comment != null)
			{
				return comment.GetScore();
			}
			
            throw new ArgumentException("Comment not found.");
		}
		
	}
}