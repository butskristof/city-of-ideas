using System;
using System.Collections.Generic;
using System.Linq;
using COI.BL.Domain.Ideation;
using COI.DAL.Ideation.EF;
using Xunit;
using Xunit.Abstractions;

namespace COI.DAL.Tests.Ideation
{
	public class CommentEfRepositoryTests
	{
		private readonly ITestOutputHelper _testOutputHelper;

		public CommentEfRepositoryTests(ITestOutputHelper testOutputHelper)
		{
			_testOutputHelper = testOutputHelper;
		}

		[Fact]
		public void ReadCommentsForIdea_WithEmptyDb_ReturnsEmptyList()
		{
			// arrange
			const int ideaId = 1;
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				IEnumerable<Comment> comments = null;
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new CommentRepository(ctx);
					comments = repo.ReadCommentsForIdea(ideaId).ToList();
				}

				// assert
				Assert.NotNull(comments);
				Assert.Empty(comments);
			}
		}

		// todo fix testing
//		public static IEnumerable<object[]> GetComments()
//		{
//			Idea idea1 = new Idea { IdeaId = 1 };
//			Idea idea2 = new Idea { IdeaId = 2 };
//			yield return new object[]
//			{
//				new List<Comment> { new Comment { CommentId = 1, Idea = idea2 } }, 
//				0
//			};
//			yield return new object[]
//			{
//				new List<Comment> { new Comment { CommentId = 1, Idea = idea1 } }, 
//				1
//			};
//			yield return new object[]
//			{
//				new List<Comment> { new Comment { CommentId = 1, Idea = idea1 }, new Comment { CommentId = 2, Idea = idea1 } }, 
//				2
//			};
//			yield return new object[]
//			{
//				new List<Comment> { new Comment { CommentId = 1, Idea = idea1 }, new Comment { CommentId = 2, Idea = idea2 } },
//				1
//			};
//			yield return new object[]
//			{
//				new List<Comment> { new Comment { CommentId = 1, Idea = idea1 }, new Comment { CommentId = 2, Idea = idea1 }, new Comment { CommentId = 3, Idea = idea2 } },
//				2
//			};
//		}
//
//		[Theory]
//		[MemberData(nameof(GetComments))]
//		public void ReadCommentsForIdea_WithData_ReturnsList(List<Comment> inputData, int expectedCount)
//		{
//			// arrange
//			const int ideaId = 1;
//			using (var factory = new CityOfIdeasDbContextFactory())
//			{
//				IEnumerable<Comment> comments = null;
//				using (var ctx = factory.CreateContext())
//				{
////					foreach (Comment comment in inputData)
////					{
////						ctx.Comments.Add(comment);
////					}
//					for (int i = 0; i < inputData.Count; ++i)
//					{
//						_testOutputHelper.WriteLine("Hello! {0}", i);
//						ctx.Comments.Add(inputData[i]);
//					}
////					inputData.ForEach(c => ctx.Comments.Add(c));
//					ctx.SaveChanges();
//					
//					// act
//					var repo = new CommentRepository(ctx);
//					comments = repo.ReadCommentsForIdea(ideaId).ToList();
//				}
//				
//				// assert
//				Assert.NotNull(comments);
//				Assert.Equal(expectedCount, comments.Count());
//
//				using (var ctx = factory.CreateContext())
//				{
//					ctx.Database.EnsureDeleted();
//				}
//			}
//		}

		[Fact]
		public void ReadComment_WithValidId_ReturnsObject()
		{
			// arrange
			var testComment = new Comment();
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				Comment retrievedComment = null;
				using (var ctx = factory.CreateContext())
				{
					ctx.Comments.Add(testComment);
					ctx.SaveChanges();
					
					// act
					var id = ctx.Comments.FirstOrDefault().CommentId;
					var repo = new CommentRepository(ctx);
					retrievedComment = repo.ReadComment(id);
				}
				
				// assert
				Assert.NotNull(retrievedComment);
				Assert.Equal(testComment, retrievedComment);
			}
		}
		
		[Fact]
		public void ReadComment_WithInvalidId_ReturnsNull()
		{
			var invalidId = 0;
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				Comment retrievedComment = null;
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new CommentRepository(ctx);
					retrievedComment = repo.ReadComment(invalidId);
				}
				
				// assert
				Assert.Null(retrievedComment);
			}
		}

		[Fact]
		public void CreateComment_WithValidObject_ReturnsObjectWithId()
		{
			// arrange
			var testComment = new Comment();
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				Comment retrievedComment = null;
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new CommentRepository(ctx);
					retrievedComment = repo.CreateComment(testComment);
				}
				
				// assert
				Assert.NotNull(retrievedComment);
				Assert.Equal(testComment, retrievedComment);
				Assert.Equal(1, retrievedComment.CommentId);
			}
		}
		
		[Fact]
		public void CreateComment_WithDuplicateObject_Throws()
		{
			// arrange
			var testComment = new Comment();
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				Action duplicateAdd = null;
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new CommentRepository(ctx);
					repo.CreateComment(testComment);
					// this will throw an exception
					duplicateAdd = () => repo.CreateComment(testComment);
					
					// assert
					var exception = Assert.Throws<ArgumentException>(duplicateAdd);
					Assert.Equal("Comment already in database.", exception.Message);
				}
			}
		}

		[Fact]
		public void UpdateComment_WithValidObject_ReturnsObjectWithId()
		{
			// arrange
			var testComment = new Comment();
			var newTime = DateTime.Now.AddHours(-2);
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				Comment retrievedComment = null;
				using (var ctx = factory.CreateContext())
				{
					ctx.Comments.Add(testComment);
					ctx.SaveChanges();

					testComment.Created = newTime;
					
					// act
					var repo = new CommentRepository(ctx);
					retrievedComment = repo.UpdateComment(testComment);
				}
				
				// assert
				Assert.NotNull(retrievedComment);
				Assert.Equal(testComment, retrievedComment);
				Assert.Equal(newTime, retrievedComment.Created);
			}
		}

		[Fact]
		public void UpdateComment_WithInvalidId_Throws()
		{
			// arrange
			var testComment = new Comment();
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				using (var ctx = factory.CreateContext())
				{
					ctx.Comments.Add(testComment);
					ctx.SaveChanges();

					testComment.CommentId = 0;

					// act
					var repo = new CommentRepository(ctx);
					Action result = () => repo.UpdateComment(testComment);

					// assert
					var exception = Assert.Throws<ArgumentException>(result);
					Assert.Equal("Comment to update not found.", exception.Message);
				}
			}
		}

		[Fact]
		public void DeleteComment_WithValidId_DeletesComment()
		{
			// arrange
			var testComment = new Comment();
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				Comment retrievedComment0 = null;
				Comment retrievedComment1 = null;
				using (var ctx = factory.CreateContext())
				{
					ctx.Comments.Add(testComment);
					ctx.SaveChanges();
					
					// act
					var id = ctx.Comments.FirstOrDefault().CommentId;
					var repo = new CommentRepository(ctx);
					retrievedComment0 = repo.DeleteComment(id);
					retrievedComment1 = repo.ReadComment(id);
				}
				
				// assert
				Assert.NotNull(retrievedComment0);
				Assert.Equal(testComment, retrievedComment0);
				Assert.Null(retrievedComment1);
			}
		}

		[Fact]
		public void DeleteComment_WithInvalidId_Throws()
		{
			// arrange
			var invalidId = 0;
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new CommentRepository(ctx);
					Action result = () => repo.DeleteComment(invalidId);
					
					// assert
					var exception = Assert.Throws<ArgumentException>(result);
					Assert.Equal("Comment to delete not found.", exception.Message);
				}
			}
		}
	}
}