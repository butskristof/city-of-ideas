using System;
using System.Collections.Generic;
using System.Linq;
using COI.BL.Domain.Ideation;
using COI.BL.Domain.User;
using COI.BL.Ideation;
using COI.DAL.Ideation;
using Moq;
using Xunit;

namespace COI.BL.Tests.Ideation
{
	public class IdeationManagerTests
	{
		[Fact]
		public void GetIdeas_WithEmptyRepo_ReturnsEmptyCollection()
		{
			// arrange
			var ideaRepo = new Mock<IIdeaRepository>();
			ideaRepo
				.Setup(x => x.ReadIdeas())
				.Returns(new List<Idea>());
			var commentRepo = new Mock<ICommentRepository>();
			var mgr = new IdeationManager(ideaRepo.Object, commentRepo.Object);
			
			// act
			var result = mgr.GetIdeas();
			
			// assert
			Assert.NotNull(result);
			Assert.Empty(result);
		}

		[Fact]
		public void GetIdeas_WithIdeasInRepo_ReturnsIdeas()
		{
			// arrange
			var ideaRepo = new Mock<IIdeaRepository>();
			ideaRepo
				.Setup(x => x.ReadIdeas())
				.Returns(new List<Idea>() { new Idea(), new Idea()});
			var commentRepo = new Mock<ICommentRepository>();
			var mgr = new IdeationManager(ideaRepo.Object, commentRepo.Object);
			
			// act
			var result = mgr.GetIdeas();
			
			// assert
			Assert.NotNull(result);
			Assert.Equal(ideaRepo.Object.ReadIdeas().Count(), result.Count());
			Assert.Equal(ideaRepo.Object.ReadIdeas(), result);
		}
		
		[Fact]
		public void GetIdea_WithValidId_ReturnsIdeaObject()
		{
			// arrange
			const int id = 1;
			
			var ideaRepo = new Mock<IIdeaRepository>();
			ideaRepo
				.Setup(x => x.ReadIdea(id))
				.Returns(new Idea() {IdeaId = id});
			var commentRepo = new Mock<ICommentRepository>();
			var mgr = new IdeationManager(ideaRepo.Object, commentRepo.Object);
			
			// act
			Idea result = mgr.GetIdea(id);
			
			// assert
			Assert.NotNull(result);
			Assert.Equal(id, result.IdeaId);
			Assert.Equal(ideaRepo.Object.ReadIdea(id), result);
		}
		
		[Fact]
		public void GetIdea_WithInvalidId_ReturnsNull()
		{
			// arrange
			const int id = 1;
			
			var ideaRepo = new Mock<IIdeaRepository>();
			ideaRepo
				.Setup(x => x.ReadIdea(It.IsAny<int>()))
				.Returns((Idea) null);
			var commentRepo = new Mock<ICommentRepository>();
			var mgr = new IdeationManager(ideaRepo.Object, commentRepo.Object);
			
			// act
			Idea result = mgr.GetIdea(id);
			
			// assert
			Assert.Null(result);
		}

		[Fact]
		public void AddVoteToIdea_WithValidIdeaId_AddsVote()
		{
			// arrange
			const int ideaId = 1;
			
			var ideaRepo = new Mock<IIdeaRepository>();
			ideaRepo
				.Setup(x => x.ReadIdea(ideaId))
				.Returns(new Idea() {IdeaId = ideaId});
			var commentRepo = new Mock<ICommentRepository>();
			var mgr = new IdeationManager(ideaRepo.Object, commentRepo.Object);
			
			// act
			Vote newVote = new Vote();
			mgr.AddVoteToIdea(ideaId, newVote);
			Idea idea = mgr.GetIdea(ideaId);
			
			// assert
			Assert.Equal(1, idea.Votes.Count);
			Assert.Equal(newVote, idea.Votes.First());
		}

		[Fact]
		void AddVoteToIdea_WithInvalidIdeaId_Throws()
		{
			// arrange
			const int ideaId = 1;
			
			var ideaRepo = new Mock<IIdeaRepository>();
			var commentRepo = new Mock<ICommentRepository>();
			var mgr = new IdeationManager(ideaRepo.Object, commentRepo.Object);
			
			// act
			Action result = () => mgr.AddVoteToIdea(ideaId, new Vote());
			
			// assert
			Assert.Throws<ArgumentException>(result);
		}

		[Fact]
		public void AddVoteToComment_WithValidCommentId_AddsVote()
		{
			// arrange
			const int commentId = 1;
			
			var ideaRepo = new Mock<IIdeaRepository>();
			var commentRepo = new Mock<ICommentRepository>();
			commentRepo
				.Setup(r => r.ReadComment(commentId))
				.Returns(new Comment() { CommentId = commentId});
			var mgr = new IdeationManager(ideaRepo.Object, commentRepo.Object);
			
			// act
			Vote newVote = new Vote();
			mgr.AddVoteToComment(commentId, newVote);
			Comment comment = mgr.GetComment(commentId);
			
			// assert
			Assert.Equal(1, comment.Votes.Count);
			Assert.Equal(newVote, comment.Votes.First());
		}

		[Fact]
		public void AddVoteToComment_WithInvalidCommentId_Throws()
		{
			// arrange
			const int commentId = 1;
			
			var ideaRepo = new Mock<IIdeaRepository>();
			var commentRepo = new Mock<ICommentRepository>();
			var mgr = new IdeationManager(ideaRepo.Object, commentRepo.Object);
			
			// act
			Action result = () => mgr.AddVoteToComment(commentId, new Vote());
			
			// assert
			Assert.Throws<ArgumentException>(result);
		}

		[Fact]
		public void AddCommentToIdea_WithInvalidIdeaId_Throws()
		{
			// arrange
			const int ideaId = 1;
			
			var ideaRepo = new Mock<IIdeaRepository>();
			var commentRepo = new Mock<ICommentRepository>();
			var mgr = new IdeationManager(ideaRepo.Object, commentRepo.Object);
			
			// act
			Action result = () => mgr.AddCommentToIdea(ideaId, new Comment());
			
			// assert
			Assert.Throws<ArgumentException>(result);
		}

		[Fact]
		public void AddCommentToIdea_WithValidIdeaId_AddsComment()
		{
			// arrange
			const int ideaId = 1;
			
			var ideaRepo = new Mock<IIdeaRepository>();
			ideaRepo
				.Setup(x => x.ReadIdea(ideaId))
				.Returns(new Idea() {IdeaId = ideaId});
			var commentRepo = new Mock<ICommentRepository>();
			var mgr = new IdeationManager(ideaRepo.Object, commentRepo.Object);
			
			// act
			Comment newComment = new Comment();
			mgr.AddCommentToIdea(ideaId, newComment);
			Idea idea = mgr.GetIdea(ideaId);
			
			// assert
			Assert.Equal(1, idea.Comments.Count);
			Assert.Equal(newComment, idea.Comments.First());
		}

		[Fact]
		public void AddCommentToIdea_WithInvalidIdeaIdAndFields_Throws()
		{
			// arrange
			const int ideaId = 1;
			
			var ideaRepo = new Mock<IIdeaRepository>();
			var commentRepo = new Mock<ICommentRepository>();
			var mgr = new IdeationManager(ideaRepo.Object, commentRepo.Object);
			
			// act
			Action result = () => mgr.AddCommentToIdea(ideaId, new List<Field>()
			{
				new Field() { Content = "Test"}
			});
			
			// assert
			Assert.Throws<ArgumentException>(result);
		}

		[Fact]
		public void AddCommentToIdea_WithValidIdeaIdAndFields_ReturnsComment()
		{
			// arrange
			const int ideaId = 1;
			const String fieldContent = "Test";
			
			var ideaRepo = new Mock<IIdeaRepository>();
			ideaRepo
				.Setup(x => x.ReadIdea(ideaId))
				.Returns(new Idea() {IdeaId = ideaId});
			var commentRepo = new Mock<ICommentRepository>();
			commentRepo
				.Setup(c => c.CreateComment(It.IsAny<Comment>()))
				.Returns<Comment>(c => c);
			var mgr = new IdeationManager(ideaRepo.Object, commentRepo.Object);
			
			// act
			var returnedComment = mgr.AddCommentToIdea(ideaId, new List<Field>()
			{
				new Field() { Content = fieldContent}
			});
			Idea idea = mgr.GetIdea(ideaId);
			
			// assert
			Assert.Equal(1, idea.Comments.Count);
			Assert.Equal(returnedComment, idea.Comments.First());
			Assert.Equal(fieldContent, idea.Comments.First().Fields.First().Content);
		}

		[Fact]
		public void GetComment_WithValidId_ReturnsIdeaObject()
		{
			// arrange
			const int commentId = 1;
			
			var ideaRepo = new Mock<IIdeaRepository>();
			var commentRepo = new Mock<ICommentRepository>();
			commentRepo
				.Setup(x => x.ReadComment(commentId))
				.Returns(new Comment() {CommentId = commentId});
			var mgr = new IdeationManager(ideaRepo.Object, commentRepo.Object);
			
			// act
			Comment result = mgr.GetComment(commentId);
			
			// assert
			Assert.NotNull(result);
			Assert.Equal(commentId, result.CommentId);
			Assert.Equal(commentRepo.Object.ReadComment(commentId), result);
		}
		
		[Fact]
		public void GetComment_WithInvalidId_ReturnsNull()
		{
			// arrange
			const int commentId = 1;
			
			var ideaRepo = new Mock<IIdeaRepository>();
			var commentRepo = new Mock<ICommentRepository>();
			commentRepo
				.Setup(x => x.ReadComment(It.IsAny<int>()))
				.Returns((Comment) null);
			var mgr = new IdeationManager(ideaRepo.Object, commentRepo.Object);
			
			// act
			Comment result = mgr.GetComment(commentId);
			
			// assert
			Assert.Null(result);
		}

		[Fact]
		public void GetCommentsForIdea_WithInvalidIdeaId_ReturnsEmptyCollection()
		{
			// arrange
			const int ideaId = 0;
			
			var ideaRepo = new Mock<IIdeaRepository>();
			var commentRepo = new Mock<ICommentRepository>();
			commentRepo
				.Setup(x => x.ReadCommentsForIdea(It.IsAny<int>()))
				.Returns(new List<Comment>());
			var mgr = new IdeationManager(ideaRepo.Object, commentRepo.Object);
			
			// act
			IEnumerable<Comment> result = mgr.GetCommentsForIdea(ideaId);
			
			// assert
			Assert.NotNull(result);
			Assert.Empty(result);
		}

		[Fact]
		public void GetCommentsForIdea_WithValidIdeaIdAndNoTickets_ReturnsEmptyCollection()
		{
			// arrange
			const int ideaId = 0;
			
			var ideaRepo = new Mock<IIdeaRepository>();
			var commentRepo = new Mock<ICommentRepository>();
			commentRepo
				.Setup(x => x.ReadCommentsForIdea(It.IsAny<int>()))
				.Returns(new List<Comment>());
			var mgr = new IdeationManager(ideaRepo.Object, commentRepo.Object);
			
			// act
			IEnumerable<Comment> result = mgr.GetCommentsForIdea(ideaId);
			
			// assert
			Assert.NotNull(result);
			Assert.Empty(result);
		}

		[Fact]
		public void GetCommentsForIdea_WithValidIdeaId_ReturnsListOfComments()
		{
			// arrange
			const int ideaId = 0;
			
			var ideaRepo = new Mock<IIdeaRepository>();
			var commentRepo = new Mock<ICommentRepository>();
			commentRepo
				.Setup(x => x.ReadCommentsForIdea(It.IsAny<int>()))
				.Returns(new List<Comment>() {new Comment(), new Comment()});
			var mgr = new IdeationManager(ideaRepo.Object, commentRepo.Object);
			
			// act
			var result = mgr.GetCommentsForIdea(ideaId);
			
			// assert
			Assert.NotNull(result);
			Assert.Equal(commentRepo.Object.ReadCommentsForIdea(ideaId).Count(), result.Count());
			Assert.Equal(commentRepo.Object.ReadCommentsForIdea(ideaId), result);
		}

		[Fact]
		public void GetIdeaScore_WithValidIdeaId_ReturnsInt()
		{
			// arrange
			const int id = 1;
			
			var idea = new Idea()
			{
				IdeaId = id,
				Votes = new List<Vote>()
				{
					new Vote() { Value = 1 },
					new Vote() { Value = 1 },
				}
			};
			
			var ideaRepo = new Mock<IIdeaRepository>();
			ideaRepo
				.Setup(x => x.ReadIdea(id))
				.Returns(idea);
			var commentRepo = new Mock<ICommentRepository>();
			var mgr = new IdeationManager(ideaRepo.Object, commentRepo.Object);
			
			// act
			var result = mgr.GetIdeaScore(id);
			
			// assert
			Assert.NotEqual(0, result);
			Assert.Equal(idea.GetScore(), result);
		}
		
		[Fact]
		public void GetIdeaScore_WithInvalidIdeaId_Throws()
		{
			// arrange
			const int ideaId = 1;
			
			var ideaRepo = new Mock<IIdeaRepository>();
			var commentRepo = new Mock<ICommentRepository>();
			var mgr = new IdeationManager(ideaRepo.Object, commentRepo.Object);
			
			// act
			Action result = () => mgr.GetIdeaScore(ideaId);
			
			// assert
			Assert.Throws<ArgumentException>(result);
		}

		[Fact]
		public void GetCommentScore_WithValidCommentId_ReturnsInt()
		{
			// arrange
			const int id = 1;
			
			var comment = new Comment()
			{
				CommentId = id,
				Votes = new List<Vote>()
				{
					new Vote() { Value = 1 },
					new Vote() { Value = 1 },
				}
			};
			
			var ideaRepo = new Mock<IIdeaRepository>();
			var commentRepo = new Mock<ICommentRepository>();
			commentRepo
				.Setup(x => x.ReadComment(id))
				.Returns(comment);
			var mgr = new IdeationManager(ideaRepo.Object, commentRepo.Object);
			
			// act
			var result = mgr.GetCommentScore(id);
			
			// assert
			Assert.NotEqual(0, result);
			Assert.Equal(comment.GetScore(), result);
		}
		
		[Fact]
		public void GetCommentScore_WithInvalidCommentId_Throws()
		{
			// arrange
			const int id = 1;
			
			var ideaRepo = new Mock<IIdeaRepository>();
			var commentRepo = new Mock<ICommentRepository>();
			var mgr = new IdeationManager(ideaRepo.Object, commentRepo.Object);
			
			// act
			Action result = () => mgr.GetCommentScore(id);
			
			// assert
			Assert.Throws<ArgumentException>(result);
		}
	}
}