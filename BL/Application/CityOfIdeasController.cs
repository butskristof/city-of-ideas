using System;
using System.Collections.Generic;
using COI.BL.Domain.Ideation;
using COI.BL.Domain.Questionnaire;
using COI.BL.Domain.User;
using COI.BL.Ideation;
using COI.BL.Questionnaire;
using COI.BL.User;
using COI.DAL.Ideation;
using COI.DAL.Ideation.EF;
using COI.DAL.Questionnaire;
using COI.DAL.Questionnaire.EF;
using COI.DAL.User;
using COI.DAL.User.EF;

namespace COI.BL.Application
{
	/**
	 * The application controller will orchestrate actions from the UI where multiple managers have to do work.
	 * It will act as an intermediary when a Unit of Work pattern is applied.
	 * Otherwise, manager methods will be called directly.
	 */
	public interface ICityOfIdeasController
	{
//		Answer AnswerOpenQuestion(string userId, int questionId, string content);
//		Answer AnswerChoiceQuestion(string userId, int optionId);
		
		// Comments
		Comment AddCommentToIdea(IEnumerable<Field> content, string userId, int ideaId);
		
		// Votes
		Vote AddVoteToIdea(int value, string userId, int ideaId);
		Vote AddVoteToIdeation(int value, string userId, int ideationId);
		Vote AddVoteToComment(int value, string userId, int commentId);
		
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
		
		public Comment AddCommentToIdea(IEnumerable<Field> content, string userId, int ideaId)
		{
			_unitOfWorkManager.StartUnitOfWork();
			
			Comment comment = _ideationManager.AddCommentToIdea(content, ideaId);
			_userManager.AddCommentToUser(comment, userId);
			
			_unitOfWorkManager.EndUnitOfWork();

			return comment;
		}
		
		#endregion

		#region Votes

		public Vote AddVoteToIdea(int value, string userId, int ideaId)
		{
			_unitOfWorkManager.StartUnitOfWork();

			Vote vote = _userManager.AddVoteToUser(value, userId);
			
			_ideationManager.AddVoteToIdea(vote, ideaId);
			
			_unitOfWorkManager.EndUnitOfWork();

			return vote;
		}

		public Vote AddVoteToComment(int value, string userId, int commentId)
		{
			_unitOfWorkManager.StartUnitOfWork();

			Vote vote = _userManager.AddVoteToUser(value, userId);
			
			_ideationManager.AddVoteToComment(vote, commentId);
			
			// If the ideaId should be invalid, the method throws before the unit of work is saved, so the context will be discarded before changes are saved. The Vote object won't be saved to the user's votes.
			_unitOfWorkManager.EndUnitOfWork();

			return vote;
		}

		public Vote AddVoteToIdeation(int value, string userId, int ideationId)
		{
			_unitOfWorkManager.StartUnitOfWork();

			Vote vote = _userManager.AddVoteToUser(value, userId);
			
			_ideationManager.AddVoteToIdeation(vote, ideationId);
			
			_unitOfWorkManager.EndUnitOfWork();

			return vote;
		}

		#endregion

		#region Answers

		public Answer AddAnswerToQuestion(string content, string userId, int questionId)
		{
			_unitOfWorkManager.StartUnitOfWork();

			Answer answer = _questionnaireManager.AnswerQuestion(content, questionId);
			_userManager.AddAnswerToUser(answer, userId);
			
			_unitOfWorkManager.EndUnitOfWork();

			return answer;
		}

		public Answer AddAnswerToOption(string userId, int optionId)
		{
			_unitOfWorkManager.StartUnitOfWork();

			Answer answer = _questionnaireManager.AnswerOption(optionId);
			_userManager.AddAnswerToUser(answer, userId);
			
			_unitOfWorkManager.EndUnitOfWork();

			return answer;
		}

		#endregion
	}
}