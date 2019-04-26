using System;
using System.Collections.Generic;
using System.Linq;
using COI.DAL.Project.EF;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace COI.DAL.Tests.Project
{
	public class ProjectEfRepositoryTests
	{
		[Fact]
		public void ReadProjects_WithEmptyDb_ReturnsEmptyList()
		{
			// arrange
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				IEnumerable<BL.Domain.Project.Project> projs = null;
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new ProjectRepository(ctx);
					projs = repo.ReadProjects().ToList();
				}

				// assert
				Assert.NotNull(projs);
				Assert.Empty(projs);
			}
		}

		[Fact]
		public void ReadProjects_WithData_ReturnsList()
		{
			// arrange
			var testProj = new BL.Domain.Project.Project()
			{
				Title = "Test"
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				IEnumerable<BL.Domain.Project.Project> projs = null;
				using (var ctx = factory.CreateContext())
				{
					ctx.Projects.Add(testProj);
					ctx.SaveChanges();
					
					// act
					var repo = new ProjectRepository(ctx);
					projs = repo.ReadProjects().ToList();
				}
				
				// assert
				Assert.NotNull(projs);
				Assert.Single(projs);
				Assert.Equal(testProj, projs.FirstOrDefault());
			}
		}

		[Fact]
		public void ReadProject_WithValidId_ReturnsObject()
		{
			// arrange
			var testProj = new BL.Domain.Project.Project()
			{
				Title = "Test"
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				BL.Domain.Project.Project retrievedProj = null;
				using (var ctx = factory.CreateContext())
				{
					ctx.Projects.Add(testProj);
					ctx.SaveChanges();
					
					// act
					var id = ctx.Projects.FirstOrDefault().ProjectId;
					var repo = new ProjectRepository(ctx);
					retrievedProj = repo.ReadProject(id);
				}
				
				// assert
				Assert.NotNull(retrievedProj);
				Assert.Equal(testProj, retrievedProj);
			}
		}
		
		[Fact]
		public void ReadProj_WithInvalidId_ReturnsNull()
		{
			var invalidId = 0;
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				BL.Domain.Project.Project retrievedProj = null;
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new ProjectRepository(ctx);
					retrievedProj = repo.ReadProject(invalidId);
				}
				
				// assert
				Assert.Null(retrievedProj);
			}
		}

		[Fact]
		public void CreateProject_WithValidObject_ReturnsObjectWithId()
		{
			// arrange
			var testProj = new BL.Domain.Project.Project()
			{
				Title = "Test"
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				BL.Domain.Project.Project retrievedProject = null;
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new ProjectRepository(ctx);
					retrievedProject = repo.CreateProject(testProj);
				}
				
				// assert
				Assert.NotNull(retrievedProject);
				Assert.Equal(testProj, retrievedProject);
				Assert.Equal(1, retrievedProject.ProjectId);
			}
		}
		
		[Fact]
		public void CreateProject_WithDuplicateObject_Throws()
		{
			// arrange
			var testProj = new BL.Domain.Project.Project()
			{
				Title = "Test"
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new ProjectRepository(ctx);
					repo.CreateProject(testProj);
					// this will throw an exception
					Action duplicateAdd = () => repo.CreateProject(testProj);
					
					// assert
					var exception = Assert.Throws<ArgumentException>(duplicateAdd);
					Assert.Equal("Project already in database.", exception.Message);
				}
			}
		}

        public static IEnumerable<object[]> GetProjects()
		{
			yield return new object[]
			{
				new BL.Domain.Project.Project(),
				"SQLite Error 19: 'NOT NULL constraint failed: Projects.Title'."
			};
		}

		[Theory]
		[MemberData(nameof(GetProjects))]
		public void CreateProject_WithInvalidObject_Throws(
			BL.Domain.Project.Project toCreate,
			string msg
		)
		{
			// arrange
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				Action result = null;
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new ProjectRepository(ctx);
					result = () => repo.CreateProject(toCreate);
					
					// assert
					var exception = Assert.Throws<ArgumentException>(result);
					Assert.Equal(msg, exception.Message);
				}
			}
		}
		
		[Fact]
		public void UpdateProject_WithValidObject_ReturnsObjectWithId()
		{
			// arrange
			var testProj = new BL.Domain.Project.Project()
			{
				Title = "Test"
			};
			var newTitle = "New Test";
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				BL.Domain.Project.Project retrievedProj = null;
				using (var ctx = factory.CreateContext())
				{
					ctx.Projects.Add(testProj);
					ctx.SaveChanges();

					testProj.Title = newTitle;
					
					// act
					var repo = new ProjectRepository(ctx);
					retrievedProj = repo.UpdateProject(testProj);
				}
				
				// assert
				Assert.NotNull(retrievedProj);
				Assert.Equal(testProj, retrievedProj);
				Assert.Equal(newTitle, retrievedProj.Title);
			}
		}

		[Fact]
		public void UpdateProject_WithInvalidId_Throws()
		{
			// arrange
			var testProj = new BL.Domain.Project.Project()
			{
				Title = "Test"
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				using (var ctx = factory.CreateContext())
				{
					ctx.Projects.Add(testProj);
					ctx.SaveChanges();

					testProj.ProjectId = 0;

					// act
					var repo = new ProjectRepository(ctx);
					Action result = () => repo.UpdateProject(testProj);

					// assert
					var exception = Assert.Throws<ArgumentException>(result);
					Assert.Equal("Project to update not found.", exception.Message);
				}
			}
		}

		[Theory]
		[InlineData(null)]
		public void UpdateProject_WithInvalidObject_Throws(
			string newTitle
		)
		{
			// arrange
			var testProj = new BL.Domain.Project.Project()
			{
				Title = "Test"
			};

			using (var factory = new CityOfIdeasDbContextFactory())
			{
				using (var ctx = factory.CreateContext())
				{
					ctx.Projects.Add(testProj);
					ctx.SaveChanges();

					testProj.Title = newTitle;

					// act
					var repo = new ProjectRepository(ctx);
					Action result = () => repo.UpdateProject(testProj);

					// assert
					var exception = Assert.Throws<DbUpdateException>(result);
				}
			}
		}

		[Fact]
		public void DeleteProject_WithValidId_DeletesProject()
		{
			// arrange
			var testProj = new BL.Domain.Project.Project()
			{
				Title = "Test"
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				BL.Domain.Project.Project retrievedProj0 = null;
				BL.Domain.Project.Project retrievedProj1 = null;
				using (var ctx = factory.CreateContext())
				{
					ctx.Projects.Add(testProj);
					ctx.SaveChanges();
					
					// act
					var id = ctx.Projects.FirstOrDefault().ProjectId;
					var repo = new ProjectRepository(ctx);
					retrievedProj0 = repo.DeleteProject(id);
					retrievedProj1 = repo.ReadProject(id);
				}
				
				// assert
				Assert.NotNull(retrievedProj0);
				Assert.Equal(testProj, retrievedProj0);
				Assert.Null(retrievedProj1);
			}
		}

		[Fact]
		public void DeleteProj_WithInvalidId_Throws()
		{
			// arrange
			var invalidId = 0;
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new ProjectRepository(ctx);
					Action result = () => repo.DeleteProject(invalidId);
					
					// assert
					var exception = Assert.Throws<ArgumentException>(result);
					Assert.Equal("Project to delete not found.", exception.Message);
				}
			}
		}
	}
}