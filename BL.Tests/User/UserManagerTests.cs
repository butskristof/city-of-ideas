using System;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using COI.BL.Domain.Ideation;
using COI.BL.Domain.Questionnaire;
using COI.BL.Domain.User;
using COI.BL.User;
using COI.DAL.Ideation;
using COI.DAL.User;
using Moq;
using Xunit;

namespace COI.BL.Tests.User
{
	public class UserManagerTests
	{
		[Fact]
		public void GetUser_WithValidId_ReturnsUserObject()
		{
			// arrange
			const int id = 1;
			
			var userRepo = new Mock<IUserRepository>();
			userRepo
				.Setup(x => x.ReadUser(id))
				.Returns(new Domain.User.User() {UserId = id});
			var voteRepo = new Mock<IVoteRepository>();
			var mgr = new UserManager(userRepo.Object, voteRepo.Object);
			
			// act
			Domain.User.User result = mgr.GetUser(id);
			
			// assert
			Assert.NotNull(result);
			Assert.Equal(id, result.UserId);
		}
		
		[Fact]
		public void GetUser_WithInvalidId_ReturnsNull()
		{
			// arrange
			const int id = 0;
			
			var userRepo = new Mock<IUserRepository>();
			userRepo
				.Setup(x => x.ReadUser(It.IsAny<int>()))
				.Returns((Domain.User.User) null);
			var voteRepo = new Mock<IVoteRepository>();
			var mgr = new UserManager(userRepo.Object, voteRepo.Object);
			
			// act
			Domain.User.User result = mgr.GetUser(id);
			
			// assert
			Assert.Null(result);
		}

		[Fact]
		public void AddVoteToUser_WithInvalidUserId_Throws()
		{
			// arrange
			var userRepo = new Mock<IUserRepository>();
			var voteRepo = new Mock<IVoteRepository>();
			var mgr = new UserManager(userRepo.Object, voteRepo.Object);

			const int userId = 1;
			
			// act
			Action result = () => mgr.AddVoteToUser(userId, new Vote());
			
			// assert
			Assert.Throws<ArgumentException>(result);
		}
		
		[Fact]
		public void AddVoteToUser_WithValidUserId_AddsVote()
		{
			// arrange
			const int userId = 1;
			var userRepo = new Mock<IUserRepository>();
			userRepo
				.Setup(r => r.ReadUser(userId))
				.Returns(new Domain.User.User()
				{
					UserId = userId
				});
			var voteRepo = new Mock<IVoteRepository>();
			var mgr = new UserManager(userRepo.Object, voteRepo.Object);
			
			// act
			Vote newVote = new Vote();
			mgr.AddVoteToUser(userId, newVote);
			Domain.User.User user = mgr.GetUser(userId);
			
			// assert
			Assert.Equal(1, user.Votes.Count);
			Assert.Equal(newVote, user.Votes.First());
		}

		[Fact]
		public void AddCommentToUser_WithInvalidUserId_Throws()
		{
			// arrange
			var userRepo = new Mock<IUserRepository>();
			var voteRepo = new Mock<IVoteRepository>();
			var mgr = new UserManager(userRepo.Object, voteRepo.Object);

			const int userId = 1;
			
			// act
			Action result = () => mgr.AddCommentToUser(userId, new Comment());
			
			// assert
			Assert.Throws<ArgumentException>(result);
		}
		
		[Fact]
		public void AddCommentToUser_WithValidUserId_AddsComment()
		{
			// arrange
			const int userId = 1;
			var userRepo = new Mock<IUserRepository>();
			userRepo
				.Setup(r => r.ReadUser(userId))
				.Returns(new Domain.User.User()
				{
					UserId = userId
				});
			var voteRepo = new Mock<IVoteRepository>();
			var mgr = new UserManager(userRepo.Object, voteRepo.Object);
			
			// act
			Comment newComment = new Comment();
			mgr.AddCommentToUser(userId, newComment);
			Domain.User.User user = mgr.GetUser(userId);
			
			// assert
			Assert.Equal(1, user.Comments.Count);
			Assert.Equal(newComment, user.Comments.First());
		}

		[Fact]
		public void AddAnswerToUser_WithInvalidUserId_Throws()
		{
			// arrange
			var userRepo = new Mock<IUserRepository>();
			var voteRepo = new Mock<IVoteRepository>();
			var mgr = new UserManager(userRepo.Object, voteRepo.Object);

			const int userId = 1;
			
			// act
			Action result = () => mgr.AddAnswerToUser(userId, new Answer());
			
			// assert
			Assert.Throws<ArgumentException>(result);
		}
		
		[Fact]
		public void AddAnswerToUser_WithValidUserId_AddsAnswer()
		{
			// arrange
			const int userId = 1;
			var userRepo = new Mock<IUserRepository>();
			userRepo
				.Setup(r => r.ReadUser(userId))
				.Returns(new Domain.User.User()
				{
					UserId = userId
				});
			var voteRepo = new Mock<IVoteRepository>();
			var mgr = new UserManager(userRepo.Object, voteRepo.Object);
			
			// act
			Answer newAnswer = new Answer();
			mgr.AddAnswerToUser(userId, newAnswer);
			Domain.User.User user = mgr.GetUser(userId);
			
			// assert
			Assert.Equal(1, user.Answers.Count);
			Assert.Equal(newAnswer, user.Answers.First());
		}

		[Fact]
		public void AddVoteToUser_WithValidValues_ReturnsVote()
		{
			// arrange
			const int userId = 1;
			const int value = 1;
			var userRepo = new Mock<IUserRepository>();
			userRepo
				.Setup(r => r.ReadUser(userId))
				.Returns(new Domain.User.User()
				{
					UserId = userId
				});
			var voteRepo = new Mock<IVoteRepository>();
			voteRepo
				.Setup(v => v.CreateVote(It.IsAny<Vote>()))
				.Returns<Vote>(v => v);
			var mgr = new UserManager(userRepo.Object, voteRepo.Object);
			
			// act
			Vote returnedVote = mgr.AddVoteToUser(userId, value);
			Domain.User.User user = mgr.GetUser(userId);
			
			// assert
			Assert.Equal(1, user.Votes.Count);
			Assert.Equal(returnedVote, user.Votes.First());
		}

		[Fact]
		public void AddVoteToUser_WithInvalidUserIdAndValue_Throws()
		{
			// arrange
			var userRepo = new Mock<IUserRepository>();
			var voteRepo = new Mock<IVoteRepository>();
			var mgr = new UserManager(userRepo.Object, voteRepo.Object);

			const int userId = 1;
			const int value = 1;
			
			// act
			Action result = () => mgr.AddVoteToUser(userId, value);
			
			// assert
			Assert.Throws<ArgumentException>(result);
		}
	}
}