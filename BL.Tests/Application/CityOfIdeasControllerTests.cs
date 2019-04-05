using System;
using System.Collections;
using System.Collections.Generic;
using COI.BL.Application;
using COI.BL.Domain.Ideation;
using COI.BL.Domain.Questionnaire;
using COI.BL.Domain.User;
using COI.BL.Questionnaire;
using COI.DAL;
using COI.DAL.Ideation;
using COI.DAL.Questionnaire;
using COI.DAL.User;
using Moq;
using Xunit;

namespace COI.BL.Tests.Application
{
//	public class CityOfIdeasControllerTests
//	{
//		[Fact]
//		public void AddVoteToIdea_WithValidValues_ReturnsVote()
//		{
//			// arrange
//			const int userId = 1;
//			const int ideaId = 1;
//			const int value = 1;
//			Domain.User.User user = new Domain.User.User(){UserId = userId};
//			Idea idea = new Idea(){IdeaId = ideaId};
//			
//			var userRepo = new Mock<IUserRepository>();
//			userRepo
//				.Setup(x => x.ReadUser(userId))
//				.Returns(user);
//			var voteRepo = new Mock<IVoteRepository>();
//			voteRepo
//				.Setup(v => v.CreateVote(It.IsAny<Vote>()))
//				.Returns<Vote>(v => v);
//			var ideaRepo = new Mock<IIdeaRepository>();
//			ideaRepo
//				.Setup(x => x.ReadIdea(ideaId))
//				.Returns(idea);
//			var ctrl = new CityOfIdeasController()
//			{
//				_userRepository = userRepo.Object,
//				_voteRepository = voteRepo.Object,
//				_ideaRepository = ideaRepo.Object,
//			};
//
//			// act
//			Vote result = ctrl.AddVote(userId, ideaId, value);
//			
//			// assert
//			Assert.NotNull(result);
//			Assert.Equal(ideaId, result.Idea.IdeaId);
//			Assert.Equal(userId, result.User.UserId);
//			Assert.Equal(value, result.Value);
//			Assert.Equal(1, user.Votes.Count);
//			Assert.Equal(1, idea.Votes.Count);
//		}
//
//		[Fact]
//		public void AddVoteToIdea_WithInvalidUserId_Throws()
//		{
//			// arrange
//			const int userId = 1;
//			const int ideaId = 1;
//			const int value = 1;
//			Idea idea = new Idea(){IdeaId = ideaId};
//
//			var userRepo = new Mock<IUserRepository>();
//			userRepo
//				.Setup(x => x.ReadUser(It.IsAny<int>()))
//				.Returns((Domain.User.User) null);
//			var ideaRepo = new Mock<IIdeaRepository>();
//			ideaRepo
//				.Setup(x => x.ReadIdea(ideaId))
//				.Returns(idea);
//			var ctrl = new CityOfIdeasController()
//			{
//				_userRepository = userRepo.Object,
//				_ideaRepository = ideaRepo.Object
//			};
//
//			// act
//			Action result = () => ctrl.AddVote(userId, ideaId, value);
//			
//			// assert
//			Assert.Throws<ArgumentException>(result);
//			Assert.Empty(idea.Votes);
//		}
//		
//		[Fact]
//		public void AddVoteToIdea_WithInvalidIdeaId_Throws()
//		{
//			// arrange
//			const int userId = 1;
//			const int ideaId = 1;
//			const int value = 1;
//			Domain.User.User user = new Domain.User.User() {UserId = userId};
//
//			var userRepo = new Mock<IUserRepository>();
//			userRepo
//				.Setup(x => x.ReadUser(userId))
//				.Returns(user);
//			var ideaRepo = new Mock<IIdeaRepository>();
//			ideaRepo
//				.Setup(x => x.ReadIdea(It.IsAny<int>()))
//				.Returns((Idea) null);
//			var voteRepo = new Mock<IVoteRepository>();
//			voteRepo
//				.Setup(v => v.CreateVote(It.IsAny<Vote>()))
//				.Returns<Vote>(v => v);
//			var ctrl = new CityOfIdeasController()
//			{
//				_userRepository = userRepo.Object,
//				_ideaRepository = ideaRepo.Object,
//				_voteRepository = voteRepo.Object
//			};
//
//			// act
//			Action result = () => ctrl.AddVote(userId, ideaId, value);
//			
//			// assert
//			Assert.Throws<ArgumentException>(result);
//		}
//		
//		[Fact]
//		public void AddVoteToComment_WithValidValues_ReturnsVote()
//		{
//			// arrange
//			const int userId = 1;
//			const int commentId = 1;
//			const int value = 1;
//			
//			Domain.User.User user = new Domain.User.User(){UserId = userId};
//			Comment comment = new Comment(){CommentId = commentId};
//			
//			var userRepo = new Mock<IUserRepository>();
//			userRepo
//				.Setup(x => x.ReadUser(userId))
//				.Returns(user);
//			var voteRepo = new Mock<IVoteRepository>();
//			voteRepo
//				.Setup(v => v.CreateVote(It.IsAny<Vote>()))
//				.Returns<Vote>(v => v);
//			var commentRepo = new Mock<ICommentRepository>();
//			commentRepo
//				.Setup(x => x.ReadComment(commentId))
//				.Returns(comment);
//			var ctrl = new CityOfIdeasController()
//			{
//				_userRepository = userRepo.Object,
//				_voteRepository = voteRepo.Object,
//				_commentRepository = commentRepo.Object
//			};
//
//			// act
//			Vote result = ctrl.AddVoteToComment(userId, commentId, value);
//			
//			// assert
//			Assert.NotNull(result);
//			Assert.Equal(commentId, result.Comment.CommentId);
//			Assert.Equal(userId, result.User.UserId);
//			Assert.Equal(value, result.Value);
//			Assert.Equal(1, user.Votes.Count);
//			Assert.Equal(1, comment.Votes.Count);
//		}
//
//		[Fact]
//		public void AddVoteToComment_WithInvalidUserId_Throws()
//		{
//			// arrange
//			const int userId = 1;
//			const int commentId = 1;
//			const int value = 1;
//			Comment comment = new Comment(){CommentId = commentId};
//
//			var userRepo = new Mock<IUserRepository>();
//			userRepo
//				.Setup(x => x.ReadUser(It.IsAny<int>()))
//				.Returns((Domain.User.User) null);
//			var commentRepo = new Mock<ICommentRepository>();
//			commentRepo
//				.Setup(x => x.ReadComment(commentId))
//				.Returns(comment);
//			var ctrl = new CityOfIdeasController()
//			{
//				_userRepository = userRepo.Object,
//				_commentRepository = commentRepo.Object,
//			};
//
//			// act
//			Action result = () => ctrl.AddVoteToComment(userId, commentId, value);
//			
//			// assert
//			Assert.Throws<ArgumentException>(result);
//			Assert.Empty(comment.Votes);
//		}
//		
//		[Fact]
//		public void AddVoteToComment_WithInvalidCommentId_Throws()
//		{
//			// arrange
//			const int userId = 1;
//			const int commentId = 1;
//			const int value = 1;
//			Domain.User.User user = new Domain.User.User() {UserId = userId};
//
//			var userRepo = new Mock<IUserRepository>();
//			userRepo
//				.Setup(x => x.ReadUser(userId))
//				.Returns(user);
//			var commentRepo = new Mock<ICommentRepository>();
//			commentRepo
//				.Setup(x => x.ReadComment(It.IsAny<int>()))
//				.Returns((Comment) null);
//			var voteRepo = new Mock<IVoteRepository>();
//			voteRepo
//				.Setup(x => x.CreateVote(It.IsAny<Vote>()))
//				.Returns<Vote>(v => v);
//			var ctrl = new CityOfIdeasController()
//			{
//				_userRepository = userRepo.Object,
//				_commentRepository = commentRepo.Object,
//				_voteRepository = voteRepo.Object
//			};
//
//			// act
//			Action result = () => ctrl.AddVoteToComment(userId, commentId, value);
//			
//			// assert
//			Assert.Throws<ArgumentException>(result);
//		}
//		
//		[Fact]
//		public void AddComment_WithValidValues_ReturnsComment()
//		{
//			// arrange
//			const int userId = 1;
//			const int ideaId = 1;
//			IEnumerable<Field> content = new List<Field>()
//			{
//				new Field() {Content = "Test"}
//			};
//			
//			Domain.User.User user = new Domain.User.User(){UserId = userId};
//			Idea idea = new Idea(){IdeaId = ideaId};
//			
//			var userRepo = new Mock<IUserRepository>();
//			userRepo
//				.Setup(x => x.ReadUser(userId))
//				.Returns(user);
//			var ideaRepo = new Mock<IIdeaRepository>();
//			ideaRepo
//				.Setup(x => x.ReadIdea(ideaId))
//				.Returns(idea);
//			var commentRepo = new Mock<ICommentRepository>();
//			commentRepo
//				.Setup(x => x.CreateComment(It.IsAny<Comment>()))
//				.Returns<Comment>(c => c);
//			var ctrl = new CityOfIdeasController()
//			{
//				_userRepository = userRepo.Object,
//				_ideaRepository = ideaRepo.Object,
//				_commentRepository = commentRepo.Object
//			};
//
//			// act
//			Comment result = ctrl.AddComment(userId, ideaId, content);
//			
//			// assert
//			Assert.NotNull(result);
//			Assert.Equal(ideaId, result.Idea.IdeaId);
//			Assert.Equal(userId, result.User.UserId);
//			Assert.Equal(content, result.Fields);
//			Assert.Equal(1, user.Comments.Count);
//			Assert.Equal(1, idea.Comments.Count);
//		}
//
//		[Fact]
//		public void AddComment_WithInvalidUserId_Throws()
//		{
//			// arrange
//			const int userId = 1;
//			const int ideaId = 1;
//			Idea idea = new Idea(){IdeaId = ideaId};
//			IEnumerable<Field> content = new List<Field>()
//			{
//				new Field() {Content = "Test"}
//			};
//
//			var userRepo = new Mock<IUserRepository>();
//			userRepo
//				.Setup(x => x.ReadUser(It.IsAny<int>()))
//				.Returns((Domain.User.User) null);
//			var ideaRepo = new Mock<IIdeaRepository>();
//			ideaRepo
//				.Setup(x => x.ReadIdea(ideaId))
//				.Returns(idea);
//			var commentRepo = new Mock<ICommentRepository>();
//			commentRepo
//				.Setup(x => x.CreateComment(It.IsAny<Comment>()))
//				.Returns<Comment>(c => c);
//			var ctrl = new CityOfIdeasController()
//			{
//				_userRepository = userRepo.Object,
//				_ideaRepository = ideaRepo.Object,
//				_commentRepository = commentRepo.Object
//			};
//
//			// act
//			Action result = () => ctrl.AddComment(userId, ideaId, content);
//			
//			// assert
//			Assert.Throws<ArgumentException>(result);
//			Assert.Empty(idea.Votes);
//		}
//		
//		[Fact]
//		public void AddComment_WithInvalidIdeaId_Throws()
//		{
//			// arrange
//			const int userId = 1;
//			const int ideaId = 1;
//			Domain.User.User user = new Domain.User.User(){UserId = userId};
//			IEnumerable<Field> content = new List<Field>()
//			{
//				new Field() {Content = "Test"}
//			};
//
//
//			var userRepo = new Mock<IUserRepository>();
//			userRepo
//				.Setup(x => x.ReadUser(userId))
//				.Returns(user);
//			var ideaRepo = new Mock<IIdeaRepository>();
//			ideaRepo
//				.Setup(x => x.ReadIdea(It.IsAny<int>()))
//				.Returns((Idea) null);
//			var ctrl = new CityOfIdeasController()
//			{
//				_userRepository = userRepo.Object,
//				_ideaRepository = ideaRepo.Object
//			};
//
//			// act
//			Action result = () => ctrl.AddComment(userId, ideaId, content);
//			
//			// assert
//			Assert.Throws<ArgumentException>(result);
//		}
//		
//		[Fact]
//		public void AnswerOpenQuestion_WithValidValues_ReturnsAnswer()
//		{
//			// arrange
//			const int userId = 1;
//			const int questionId = 1;
//			String content = "Test";
//			
//			Domain.User.User user = new Domain.User.User(){UserId = userId};
//			OpenQuestion question = new OpenQuestion(){QuestionId = questionId};
//			
//			var userRepo = new Mock<IUserRepository>();
//			userRepo
//				.Setup(x => x.ReadUser(userId))
//				.Returns(user);
//			var answerRepo = new Mock<IAnswerRepository>();
//			answerRepo
//				.Setup(x => x.CreateAnswer(It.IsAny<Answer>()))
//				.Returns<Answer>(a => a);
//			var questionRepo = new Mock<IQuestionRepository>();
//			questionRepo
//				.Setup(x => x.ReadOpenQuestion(questionId))
//				.Returns(question);
//			var ctrl = new CityOfIdeasController()
//			{
//				_userRepository = userRepo.Object,
//				_answerRepository = answerRepo.Object,
//				_questionRepository = questionRepo.Object
//			};
//
//			// act
//			Answer result = ctrl.AnswerOpenQuestion(userId, questionId, content);
//			
//			// assert
//			Assert.NotNull(result);
//			Assert.Equal(questionId, result.OpenQuestion.QuestionId);
//			Assert.Equal(userId, result.User.UserId);
//			Assert.Equal(content, result.Content);
//			Assert.Equal(1, user.Answers.Count);
//			Assert.Equal(1, question.Answers.Count);
//		}
//
//		[Fact]
//		public void AnswerOpenQuestion_WithInvalidUserId_Throws()
//		{
//			// arrange
//			const int userId = 1;
//			const int questionId = 1;
//			String content = "Test";
//			
//			OpenQuestion question = new OpenQuestion(){QuestionId = questionId};
//			
//			var userRepo = new Mock<IUserRepository>();
//			userRepo
//				.Setup(x => x.ReadUser(It.IsAny<int>()))
//				.Returns((Domain.User.User) null);
//			var answerRepo = new Mock<IAnswerRepository>();
//			answerRepo
//				.Setup(x => x.CreateAnswer(It.IsAny<Answer>()))
//				.Returns<Answer>(a => a);
//			var questionRepo = new Mock<IQuestionRepository>();
//			questionRepo
//				.Setup(x => x.ReadOpenQuestion(questionId))
//				.Returns(question);
//			var ctrl = new CityOfIdeasController()
//			{
//				_userRepository = userRepo.Object,
//				_answerRepository = answerRepo.Object,
//				_questionRepository = questionRepo.Object
//			};
//
//			// act
//			Action result = () => ctrl.AnswerOpenQuestion(userId, questionId, content);
//			
//			// assert
//			Assert.Throws<ArgumentException>(result);
//		}
//		
//		[Fact]
//		public void AnswerOpenQuestion_WithInvalidQuestionId_Throws()
//		{
//			// arrange
//			const int userId = 1;
//			const int questionId = 1;
//			String content = "Test";
//			
//			Domain.User.User user = new Domain.User.User(){UserId = userId};
//			
//			var userRepo = new Mock<IUserRepository>();
//			userRepo
//				.Setup(x => x.ReadUser(userId))
//				.Returns(user);
//			var answerRepo = new Mock<IAnswerRepository>();
//			answerRepo
//				.Setup(x => x.CreateAnswer(It.IsAny<Answer>()))
//				.Returns<Answer>(a => a);
//			var questionRepo = new Mock<IQuestionRepository>();
//			questionRepo
//				.Setup(x => x.ReadOpenQuestion(It.IsAny<int>()))
//				.Returns((OpenQuestion) null);
//			var ctrl = new CityOfIdeasController()
//			{
//				_userRepository = userRepo.Object,
//				_answerRepository = answerRepo.Object,
//				_questionRepository = questionRepo.Object
//			};
//
//			// act
//			Action result = () => ctrl.AnswerOpenQuestion(userId, questionId, content);
//			
//			// assert
//			Assert.Throws<ArgumentException>(result);
//			Assert.Empty(user.Answers);
//		}
//		
//		[Fact]
//		public void AnswerChoiceQuestion_WithValidValues_ReturnsAnswer()
//		{
//			// arrange
//			const int userId = 1;
//			const int optionId = 1;
//			
//			Domain.User.User user = new Domain.User.User(){UserId = userId};
//			Option option = new Option(){OptionId = optionId};
//			
//			var userRepo = new Mock<IUserRepository>();
//			userRepo
//				.Setup(x => x.ReadUser(userId))
//				.Returns(user);
//			var answerRepo = new Mock<IAnswerRepository>();
//			answerRepo
//				.Setup(x => x.CreateAnswer(It.IsAny<Answer>()))
//				.Returns<Answer>(a => a);
//			var questionRepo = new Mock<IQuestionRepository>();
//			questionRepo
//				.Setup(x => x.ReadOption(optionId))
//				.Returns(option);
//			var ctrl = new CityOfIdeasController()
//			{
//				_userRepository = userRepo.Object,
//				_questionRepository = questionRepo.Object,
//				_answerRepository = answerRepo.Object
//			};
//
//			// act
//			Answer result = ctrl.AnswerChoiceQuestion(userId, optionId);
//			
//			// assert
//			Assert.NotNull(result);
//			Assert.Equal(optionId, result.Option.OptionId);
//			Assert.Equal(userId, result.User.UserId);
//			Assert.Equal(1, user.Answers.Count);
//			Assert.Equal(1, option.Answers.Count);
//		}
//
//		[Fact]
//		public void AnswerChoiceQuestion_WithInvalidUserId_Throws()
//		{
//			// arrange
//			const int userId = 1;
//			const int optionId = 1;
//			
//			Option option = new Option(){OptionId = optionId};
//			
//			var userRepo = new Mock<IUserRepository>();
//			userRepo
//				.Setup(x => x.ReadUser(It.IsAny<int>()))
//				.Returns((Domain.User.User) null);
//			var answerRepo = new Mock<IAnswerRepository>();
//			answerRepo
//				.Setup(x => x.CreateAnswer(It.IsAny<Answer>()))
//				.Returns<Answer>(a => a);
//			var questionRepo = new Mock<IQuestionRepository>();
//			questionRepo
//				.Setup(x => x.ReadOption(optionId))
//				.Returns(option);
//			var ctrl = new CityOfIdeasController()
//			{
//				_userRepository = userRepo.Object,
//				_answerRepository = answerRepo.Object,
//				_questionRepository = questionRepo.Object
//			};
//
//			// act
//			Action result = () => ctrl.AnswerChoiceQuestion(userId, optionId);
//			
//			// assert
//			Assert.Throws<ArgumentException>(result);
//		}
//		
//		[Fact]
//		public void AnswerChoice_WithInvalidOptionId_Throws()
//		{
//			// arrange
//			const int userId = 1;
//			const int optionId = 1;
//			
//			Domain.User.User user = new Domain.User.User(){UserId = userId};
//			
//			var userRepo = new Mock<IUserRepository>();
//			userRepo
//				.Setup(x => x.ReadUser(userId))
//				.Returns(user);
//			var answerRepo = new Mock<IAnswerRepository>();
//			answerRepo
//				.Setup(x => x.CreateAnswer(It.IsAny<Answer>()))
//				.Returns<Answer>(a => a);
//			var questionRepo = new Mock<IQuestionRepository>();
//			questionRepo
//				.Setup(x => x.ReadOption(It.IsAny<int>()))
//				.Returns((Option) null);
//			var ctrl = new CityOfIdeasController()
//			{
//				_userRepository = userRepo.Object,
//				_answerRepository = answerRepo.Object,
//				_questionRepository = questionRepo.Object
//			};
//
//			// act
//			Action result = () => ctrl.AnswerChoiceQuestion(userId, optionId);
//			
//			// assert
//			Assert.Throws<ArgumentException>(result);
//			Assert.Empty(user.Answers);
//		}
//	}
}