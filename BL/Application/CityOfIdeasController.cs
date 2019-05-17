using System;
using Castle.Core.Internal;
using COI.BL.Domain.Ideation;
using COI.BL.Domain.Questionnaire;
using COI.BL.Domain.User;
using COI.BL.Ideation;
using COI.BL.Questionnaire;
using COI.BL.User;

namespace COI.BL.Application
{
	public interface ICityOfIdeasController
	{
		// Comments
		Comment AddCommentToIdea(string userId, int ideaId);
		
		// Votes
		Vote AddVoteToIdea(int value, string userId, string email, int ideaId);
		Vote AddVoteToIdeation(int value, string userId, string email, int ideationId);
		Vote AddVoteToComment(int value, string userId, string email, int commentId);
		
		// Answers
		Answer AddAnswerToQuestion(string content, string userId, int questionId);
		Answer AddAnswerToOption(string userId, int optionId);
	}

	public class CityOfIdeasController : ICityOfIdeasController
	{
		
		private readonly IUnitOfWorkManager _unitOfWorkManager;
		private readonly IUserManager _userManager;
		private readonly IIdeationManager _ideationManager;
		private readonly IQuestionnaireManager _questionnaireManager;

		public CityOfIdeasController(IUnitOfWorkManager unitOfWorkManager, IUserManager userManager, IIdeationManager ideationManager, IQuestionnaireManager questionnaireManager)
		{
			this._unitOfWorkManager = unitOfWorkManager;
			this._userManager = userManager;
			this._ideationManager = ideationManager;
			this._questionnaireManager = questionnaireManager;
		}

		#region Comments
		
		public Comment AddCommentToIdea(string userId, int ideaId)
		{
			Comment comment = _ideationManager.AddCommentToIdea(ideaId);
			_userManager.AddCommentToUser(comment, userId);

			return comment;
		}
		
		#endregion

		#region Votes

		public Vote AddVoteToIdea(int value, string userId, string email, int ideaId)
		{
			_unitOfWorkManager.StartUnitOfWork();

			bool isUserVote = !userId.IsNullOrEmpty();
			bool isEmailVote = !email.IsNullOrEmpty();

			Vote vote = null;
			if (isUserVote)
			{
                vote = _userManager.GetVoteForIdea(ideaId, userId);
			}
			else if (isEmailVote)
			{
				vote = _userManager.GetEmailVoteForIdea(ideaId, email);
			}

			if (vote == null)
			{
				if (isUserVote)
				{
                    vote = _userManager.AddVoteToUser(value, userId);
				}
				else if (isEmailVote)
				{
                    vote = _userManager.AddVoteWithEmail(value, email);
				}
				else
				{
					vote = _userManager.AddAnonymousVote(value);
				}
			}
			else
			{
                vote = _userManager.ChangeVoteValue(vote.VoteId, value);
			}
			
            _ideationManager.AddVoteToIdea(vote, ideaId);
			
			_unitOfWorkManager.EndUnitOfWork();

			if (vote == null)
			{
				throw new Exception("Something went wrong while creating the vote.");
			}

			return vote;
		}

		public Vote AddVoteToComment(int value, string userId, string email, int commentId)
		{
			_unitOfWorkManager.StartUnitOfWork();

			bool isUserVote = !userId.IsNullOrEmpty();
			bool isEmailVote = !email.IsNullOrEmpty();

			Vote vote = null;
			if (isUserVote)
			{
                vote = _userManager.GetVoteForComment(commentId, userId);
			}
			else if (isEmailVote)
			{
				vote = _userManager.GetEmailVoteForComment(commentId, email);
			}

			if (vote == null)
			{
				if (isUserVote)
				{
                    vote = _userManager.AddVoteToUser(value, userId);
				}
				else if (isEmailVote)
				{
                    vote = _userManager.AddVoteWithEmail(value, email);
				}
				else
				{
					vote = _userManager.AddAnonymousVote(value);
				}
			}
			else
			{
                vote = _userManager.ChangeVoteValue(vote.VoteId, value);
			}
			
            _ideationManager.AddVoteToComment(vote, commentId);
			
			_unitOfWorkManager.EndUnitOfWork();

			if (vote == null)
			{
				throw new Exception("Something went wrong while creating the vote.");
			}

			return vote;
		}

		public Vote AddVoteToIdeation(int value, string userId, string email, int ideationId)
		{
			_unitOfWorkManager.StartUnitOfWork();

			bool isUserVote = !userId.IsNullOrEmpty();
			bool isEmailVote = !email.IsNullOrEmpty();

			Vote vote = null;
			if (isUserVote)
			{
                vote = _userManager.GetVoteForIdeation(ideationId, userId);
			}
			else if (isEmailVote)
			{
				vote = _userManager.GetEmailVoteForIdeation(ideationId, email);
			}

			if (vote == null)
			{
				if (isUserVote)
				{
                    vote = _userManager.AddVoteToUser(value, userId);
				}
				else if (isEmailVote)
				{
                    vote = _userManager.AddVoteWithEmail(value, email);
				}
				else
				{
					vote = _userManager.AddAnonymousVote(value);
				}
			}
			else
			{
                vote = _userManager.ChangeVoteValue(vote.VoteId, value);
			}
			
            _ideationManager.AddVoteToIdeation(vote, ideationId);
			
			_unitOfWorkManager.EndUnitOfWork();

			if (vote == null)
			{
				throw new Exception("Something went wrong while creating the vote.");
			}

			return vote;
		}

		#endregion

		#region Answers

		public Answer AddAnswerToQuestion(string content, string userId, int questionId)
		{
			Answer answer = _questionnaireManager.AnswerQuestion(content, questionId);
			_userManager.AddAnswerToUser(answer, userId);

			return answer;
		}

		public Answer AddAnswerToOption(string userId, int optionId)
		{
			Answer answer = _questionnaireManager.AnswerOption(optionId);
			_userManager.AddAnswerToUser(answer, userId);

			return answer;
		}

		#endregion
	}
}