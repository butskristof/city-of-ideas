using System;
using System.Collections.Generic;
using System.Linq;
using COI.BL.Domain.Ideation;
using COI.DAL.Ideation.EF;
using Xunit;

namespace COI.DAL.Tests.Ideation
{
	public class CommentEfRepositoryTests
	{

		[Fact]
		public void ReadComment_WithInvalidId_ReturnsNull()
		{
			// arrange
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				var fakeid = 0;
				Comment result = null;
				using (var ctx = factory.CreateContext(false))
				{
					// act
					var repo = new CommentRepository(ctx);
					result = repo.ReadComment(fakeid);
				}
				
				// assert
				Assert.Null(result);
			}
		}

		[Fact]
		public void ReadComment_WithValidId_ReturnsComment()
		{
			// arrange
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				var fakeid = 1;
				using (var ctx = factory.CreateContext(true))
				{
					Comment result = null;
					
					// act
					var repo = new CommentRepository(ctx);
					result = repo.ReadComment(fakeid);
					
					// assert
					Assert.NotNull(result);
					Assert.Equal(fakeid, result.CommentId);
					Assert.Equal(1, result.Fields.Count);
				}
				
			}
		}

		[Fact]
		public void ReadCommentsForIdea_WithInvalidIdeaId_ReturnsEmptyList()
		{
			// arrange
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				var fakeid = 0;
				IEnumerable<Comment> result = null;
				using (var ctx = factory.CreateContext(true))
				{
					// act
					var repo = new CommentRepository(ctx);
					result = repo.ReadCommentsForIdea(fakeid).ToList();
				}

				// assert
				Assert.NotNull(result);
				Assert.Empty(result);
			}
		}
		
		[Fact]
		public void ReadCommentsForIdea_WithValidIdeaIdAndNoComments_ReturnsEmptyList()
		{
			// arrange
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				var fakeid = 3;
				IEnumerable<Comment> result = null;
				using (var ctx = factory.CreateContext(true))
				{
					// act
					var repo = new CommentRepository(ctx);
					result = repo.ReadCommentsForIdea(fakeid).ToList();
				}

				// assert
				Assert.NotNull(result);
				Assert.Empty(result);
			}
		}

		[Fact]
		public void ReadCommentsForIdea_WithValidIdeaIdAndComments_ReturnsComments()
		{
			// arrange
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				var fakeid = 1;
				IEnumerable<Comment> result = null;
				using (var ctx = factory.CreateContext(true))
				{
					// act
					var repo = new CommentRepository(ctx);
					result = repo.ReadCommentsForIdea(fakeid).ToList();
				}

				// assert
				Assert.NotNull(result);
				Assert.Equal(3, result.Count());
			}
		}

		[Fact]
		public void CreateComment_WithTicket_AddsTicketNumber()
		{
			// arrange
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				var expectedId = 1;
				Comment newComment = new Comment()
				{
					Created = DateTime.Now,
				};
				newComment.Fields.Add(new Field() { Content = "YES"});
				Comment returnedComment = null;
				using (var ctx = factory.CreateContext(false))
				{
					// act
					var repo = new CommentRepository(ctx);
					returnedComment = repo.CreateComment(newComment);
				}

				// assert
				Assert.NotNull(returnedComment);
				Assert.NotNull(returnedComment.CommentId);
				Assert.Equal(expectedId, returnedComment.CommentId);
				Assert.Equal(newComment.Fields, returnedComment.Fields);
			}
		}

		[Fact]
		public void UpdateComment_WithModifiedValues_UpdatesComment()
		{
			// arrange
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				var id = 2;
				String newContent = "New Content";
				
				// act
				using (var ctx = factory.CreateContext(true))
				{
					var repo = new CommentRepository(ctx);
					Comment comment = repo.ReadComment(id);
					comment.Fields.First().Content = newContent;
					
					repo.UpdateComment(comment);
				}
				
				// assert
				using (var ctx = factory.CreateContext(false))
				{
					Comment result = null;
					
					var repo = new CommentRepository(ctx);
					result = repo.ReadComment(id);
					
					Assert.NotNull(result);
					Assert.Equal(id, result.CommentId);
					Assert.Equal(newContent, result.Fields.First().Content);
				}
			}
		}
	}
}