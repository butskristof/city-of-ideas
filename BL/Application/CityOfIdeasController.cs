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
using COI.DAL.User;
using COI.DAL.User.EF;

namespace COI.BL.Application
{
	/**
	 * The application controller will orchestrate actions from the UI where multiple managers have to do work.
	 * It will act as an intermediary when a Unit of Work pattern is applied.
	 * Otherwise, manager methods will be called directly.
	 */
	public class CityOfIdeasController
	{
		public IUserRepository _userRepository;
		public IVoteRepository _voteRepository;
		public IIdeaRepository _ideaRepository;
		public ICommentRepository _commentRepository;
		public IQuestionnaireRepository _questionnaireRepository;
		public IQuestionRepository _questionRepository;
		public IAnswerRepository _answerRepository;

//		public CityOfIdeasController(
//			IUserRepository userRepository, IVoteRepository voteRepository, IIdeaRepository ideaRepository, ICommentRepository commentRepository, IQuestionnaireRepository questionnaireRepository, IQuestionRepository questionRepository)
//		{
//			_userRepository = userRepository;
//			_voteRepository = voteRepository;
//			_ideaRepository = ideaRepository;
//			_commentRepository = commentRepository;
//			_questionnaireRepository = questionnaireRepository;
//			_questionRepository = questionRepository;
//		}

		public Vote AddVote(int userId, int ideaId, int value)
		{
			UnitOfWorkManager unitOfWorkManager = new UnitOfWorkManager();
			IUserManager userManager = new UserManager(_userRepository, _voteRepository);

			Vote vote = userManager.AddVoteToUser(userId, value);
			
			IIdeationManager ideationManager = new IdeationManager(_ideaRepository, _commentRepository);
			ideationManager.AddVoteToIdea(ideaId, vote);
			
			// If the ideaId should be invalid, the method throws before the unit of work is saved, so the context will be discarded before changes are saved. The Vote object won't be saved to the user's votes.
			unitOfWorkManager.Save();

			return vote;
		}

		public Vote AddVoteToComment(int userId, int commentId, int value)
		{
			UnitOfWorkManager unitOfWorkManager = new UnitOfWorkManager();
			IUserManager userManager = new UserManager(_userRepository, _voteRepository);

			Vote vote = userManager.AddVoteToUser(userId, value);
			
			IIdeationManager ideationManager = new IdeationManager(_ideaRepository, _commentRepository);
			ideationManager.AddVoteToComment(commentId, vote);
			
			// If the ideaId should be invalid, the method throws before the unit of work is saved, so the context will be discarded before changes are saved. The Vote object won't be saved to the user's votes.
			unitOfWorkManager.Save();

			return vote;
		}
		
		public Comment AddComment(int userId, int ideaId, IEnumerable<Field> content)
		{
			UnitOfWorkManager unitOfWorkManager = new UnitOfWorkManager();
			IIdeationManager ideationManager = new IdeationManager(_ideaRepository, _commentRepository);
			Comment comment = ideationManager.AddCommentToIdea(ideaId, content);

			IUserManager userManager = new UserManager(_userRepository, _voteRepository);
			userManager.AddCommentToUser(userId, comment);

			unitOfWorkManager.Save();
			
			return comment;
		}

		public Answer AnswerOpenQuestion(int userId, int questionId, string content)
		{
			UnitOfWorkManager unitOfWorkManager = new UnitOfWorkManager();
			
			IQuestionnaireManager questionnaireManager = new QuestionnaireManager(_questionRepository, _answerRepository, _questionnaireRepository);
			Answer answer = questionnaireManager.AnswerOpenQuestion(questionId, content);

			IUserManager userManager = new UserManager(_userRepository, _voteRepository);
			userManager.AddAnswerToUser(userId, answer);

			unitOfWorkManager.Save();
			
			return answer;
		}

		public Answer AnswerChoiceQuestion(int userId, int optionId)
		{
			UnitOfWorkManager unitOfWorkManager = new UnitOfWorkManager();
			
			IQuestionnaireManager questionnaireManager = new QuestionnaireManager(_questionRepository, _answerRepository, _questionnaireRepository);
			Answer answer = questionnaireManager.AnswerChoiceQuestion(optionId);

			IUserManager userManager = new UserManager(_userRepository, _voteRepository);
			userManager.AddAnswerToUser(userId, answer);

			unitOfWorkManager.Save();
			
			return answer;
		}
	}
}