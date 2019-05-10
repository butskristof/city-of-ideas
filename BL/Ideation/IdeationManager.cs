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
		private readonly IFieldRepository _fieldRepository;
		private readonly IProjectManager _projectManager;

		public IdeationManager(IIdeaRepository ideaRepository, IIdeationRepository ideationRepository, ICommentRepository commentRepository, IFieldRepository fieldRepository, IProjectManager projectManager)
		{
			_ideaRepository = ideaRepository;
			_ideationRepository = ideationRepository;
			_commentRepository = commentRepository;
			_fieldRepository = fieldRepository;
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

		public Domain.Ideation.Ideation AddIdeation(string title, int projectPhaseId)
		{
			ProjectPhase phase = _projectManager.GetProjectPhase(projectPhaseId);
			if (phase == null)
			{
				throw new ArgumentException("Project phase not found.");
			}
			
			Domain.Ideation.Ideation ideation = new Domain.Ideation.Ideation()
			{
				Title = title,
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

		public Domain.Ideation.Ideation ChangeIdeationState(int id, bool newState)
		{
			Domain.Ideation.Ideation toChange = GetIdeation(id);
			if (toChange != null)
			{
				toChange.IsOpen = newState;
				
				Validate(toChange);
				return _ideationRepository.UpdateIdeation(toChange);
			}
			
			throw new ArgumentException("Ideation not found.", "id");
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

		public Idea AddIdea(string title, int ideationId)
		{
			Domain.Ideation.Ideation ideation = GetIdeation(ideationId);
			if (ideation == null)
			{
				throw new ArgumentException("Ideation not found.");
			}
			
			Idea idea = new Idea()
			{
				Title = title,
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

		public Comment AddCommentToIdea(int ideaId)
		{
			Idea idea = GetIdea(ideaId);
			if (idea == null)
			{
				throw new ArgumentException("Idea not found.", "ideaId");
			}
			
			Comment comment = new Comment()
			{
				Idea = idea,
			};

			return AddComment(comment);
		}

		private Comment AddComment(Comment comment)
		{
			Validate(comment);
			return _commentRepository.CreateComment(comment);
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

		#region Fields

		public Field GetField(int fieldId)
		{
			return _fieldRepository.ReadField(fieldId);
		}

		public Field AddFieldToIdea(FieldType type, string content, int ideaId)
		{
			Idea idea = GetIdea(ideaId);
			if (idea == null)
			{
				throw new ArgumentException("Idea not found.", "ideaId");
			}
			
			Field field = new Field()
			{
				Content = content,
				FieldType = type,
				Idea = idea
			};

			return AddField(field);
		}

		public Field AddFieldToIdeation(FieldType type, string content, int ideationId)
		{
			Domain.Ideation.Ideation ideation = GetIdeation(ideationId);
			if (ideation == null)
			{
				throw new ArgumentException("Ideation not found.", "ideationId");
			}
			
			Field field = new Field()
			{
				Content = content,
				FieldType = type,
				Ideation = ideation
			};

			return AddField(field);
		}

		public Field AddFieldToComment(FieldType type, string content, int commentId)
		{
			Comment comment = GetComment(commentId);
			if (comment == null)
			{
				throw new ArgumentException("Comment not found.", "commentId");
			}
			
			Field field = new Field()
			{
				Content = content,
				FieldType = type,
				Comment = comment
			};

			return AddField(field);
		}

		private Field AddField(Field field)
		{
			Validate(field);
			return _fieldRepository.CreateField(field);
		}

		public Field ChangeIdeaField(int id, FieldType type, string content, int ideaId)
		{
			Field toChange = GetField(id);
			if (toChange != null)
			{
				Idea idea = GetIdea(ideaId);
				if (idea == null)
				{
                    throw new ArgumentException("Idea not found.", "ideaId");
				}

				toChange.FieldType = type;
				toChange.Content = content;
				toChange.Idea = idea;

				Validate(toChange);
				return _fieldRepository.UpdateField(toChange);
			}
			
			throw new ArgumentException("Field not found.", "id");
		}

		public Field ChangeIdeationField(int id, FieldType type, string content, int ideationId)
		{
			Field toChange = GetField(id);
			if (toChange != null)
			{
				Domain.Ideation.Ideation ideation = GetIdeation(ideationId);
				if (ideation == null)
				{
                    throw new ArgumentException("Ideation not found.", "ideationId");
				}

				toChange.FieldType = type;
				toChange.Content = content;
				toChange.Ideation = ideation;

				Validate(toChange);
				return _fieldRepository.UpdateField(toChange);
			}
			
			throw new ArgumentException("Field not found.", "id");
		}

		public Field ChangeCommentField(int id, FieldType type, string content, int commentId)
		{
			Field toChange = GetField(id);
			if (toChange != null)
			{
				Comment comment = GetComment(commentId);
				if (comment == null)
				{
                    throw new ArgumentException("Comment not found.", "commentId");
				}

				toChange.FieldType = type;
				toChange.Content = content;
				toChange.Comment = comment;

				Validate(toChange);
				return _fieldRepository.UpdateField(toChange);
			}
			
			throw new ArgumentException("Field not found.", "id");
		}

		private void Validate(Field field)
		{
			Validator.ValidateObject(field, new ValidationContext(field), true);
		}

		public Field RemoveField(int fieldId)
		{
			return _fieldRepository.DeleteField(fieldId);
		}

		#endregion
		
		#region Votes

		
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
			Comment comment = GetComment(commentId);
			if (comment == null)
			{
				throw new ArgumentException("Comment not found.");
			}
			
			vote.Comment = comment;
			comment.Votes.Add(vote);
			_commentRepository.UpdateComment(comment);
		}
		
		#endregion

		#region Scores

		public int GetIdeaScore(int ideaId)
		{
			Idea idea = GetIdea(ideaId);
			if (idea != null)
			{
				return idea.GetScore();
			}
			
            throw new ArgumentException("Idea not found.");
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

		#endregion
	}
}