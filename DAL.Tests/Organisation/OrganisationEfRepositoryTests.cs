using System;
using System.Collections.Generic;
using System.Linq;
using COI.DAL.Organisation.EF;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace COI.DAL.Tests.Organisation
{
	public class OrganisationEfRepositoryTests
	{
		[Fact]
		public void ReadOrganisations_WithEmptyDb_ReturnsEmptyList()
		{
			// arrange
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				IEnumerable<BL.Domain.Organisation.Organisation> orgs = null;
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new OrganisationRepository(ctx);
					orgs = repo.ReadOrganisations().ToList();
				}

				// assert
				Assert.NotNull(orgs);
				Assert.Empty(orgs);
			}
		}

		[Fact]
		public void ReadOrganisations_WithData_ReturnsList()
		{
			// arrange
			var testOrg = new BL.Domain.Organisation.Organisation()
			{
				Name = "Test",
				Identifier = "test"
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				IEnumerable<BL.Domain.Organisation.Organisation> orgs = null;
				using (var ctx = factory.CreateContext())
				{
					ctx.Organisations.Add(testOrg);
					ctx.SaveChanges();
					
					// act
					var repo = new OrganisationRepository(ctx);
					orgs = repo.ReadOrganisations().ToList();
				}
				
				// assert
				Assert.NotNull(orgs);
				Assert.Single(orgs);
				Assert.Equal(testOrg, orgs.FirstOrDefault());
			}
		}

		[Fact]
		public void ReadOrganisation_WithValidId_ReturnsObject()
		{
			// arrange
			var testOrg = new BL.Domain.Organisation.Organisation()
			{
				Name = "Test",
				Identifier = "test"
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				BL.Domain.Organisation.Organisation retrievedOrg = null;
				using (var ctx = factory.CreateContext())
				{
					ctx.Organisations.Add(testOrg);
					ctx.SaveChanges();
					
					// act
					var id = ctx.Organisations.FirstOrDefault().OrganisationId;
					var repo = new OrganisationRepository(ctx);
					retrievedOrg = repo.ReadOrganisation(id);
				}
				
				// assert
				Assert.NotNull(retrievedOrg);
				Assert.Equal(testOrg, retrievedOrg);
			}
		}
		
		[Fact]
		public void ReadOrganisation_WithInvalidId_ReturnsNull()
		{
			var invalidId = 0;
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				BL.Domain.Organisation.Organisation retrievedOrg = null;
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new OrganisationRepository(ctx);
					retrievedOrg = repo.ReadOrganisation(invalidId);
				}
				
				// assert
				Assert.Null(retrievedOrg);
			}
		}

		[Fact]
		public void CreateOrganisation_WithValidObject_ReturnsObjectWithId()
		{
			// arrange
			var testOrg = new BL.Domain.Organisation.Organisation()
			{
				Name = "Test",
				Identifier = "test"
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				BL.Domain.Organisation.Organisation retrievedOrg = null;
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new OrganisationRepository(ctx);
					retrievedOrg = repo.CreateOrganisation(testOrg);
				}
				
				// assert
				Assert.NotNull(retrievedOrg);
				Assert.Equal(testOrg, retrievedOrg);
				Assert.Equal(1, retrievedOrg.OrganisationId);
			}
		}
		
		[Fact]
		public void CreateOrganisation_WithDuplicateObject_Throws()
		{
			// arrange
			var testOrg = new BL.Domain.Organisation.Organisation()
			{
				Name = "Test",
				Identifier = "test"
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				Action duplicateAdd = null;
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new OrganisationRepository(ctx);
					repo.CreateOrganisation(testOrg);
					// this will throw an exception
					duplicateAdd = () => repo.CreateOrganisation(testOrg);
					
					// assert
					var exception = Assert.Throws<ArgumentException>(duplicateAdd);
					Assert.Equal("Organisation already in database.", exception.Message);
				}
			}
		}

        public static IEnumerable<object[]> GetOrganisations()
		{
			yield return new object[]
			{
				new BL.Domain.Organisation.Organisation(),
				"SQLite Error 19: 'NOT NULL constraint failed: Organisations.Name'."
			};
			yield return new object[]
			{
				new BL.Domain.Organisation.Organisation() { Name = ""},
				"SQLite Error 19: 'NOT NULL constraint failed: Organisations.Identifier'."
			};
			yield return new object[]
			{
				new BL.Domain.Organisation.Organisation() { Identifier = ""},
				"SQLite Error 19: 'NOT NULL constraint failed: Organisations.Name'."
			};
		}

		[Theory]
		[MemberData(nameof(GetOrganisations))]
		public void CreateOrganisation_WithInvalidObject_Throws(
			BL.Domain.Organisation.Organisation toCreate,
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
					var repo = new OrganisationRepository(ctx);
					result = () => repo.CreateOrganisation(toCreate);
					
					// assert
					var exception = Assert.Throws<ArgumentException>(result);
					Assert.Equal(msg, exception.Message);
				}
			}
		}
		
		[Fact]
		public void UpdateOrganisation_WithValidObject_ReturnsObjectWithId()
		{
			// arrange
			var testOrg = new BL.Domain.Organisation.Organisation()
			{
				Name = "Test",
				Identifier = "test"
			};
			var newName = "New Test";
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				BL.Domain.Organisation.Organisation retrievedOrg = null;
				using (var ctx = factory.CreateContext())
				{
					ctx.Organisations.Add(testOrg);
					ctx.SaveChanges();

					testOrg.Name = newName;
					
					// act
					var repo = new OrganisationRepository(ctx);
					retrievedOrg = repo.UpdateOrganisation(testOrg);
				}
				
				// assert
				Assert.NotNull(retrievedOrg);
				Assert.Equal(testOrg, retrievedOrg);
				Assert.Equal(newName, retrievedOrg.Name);
			}
		}

		[Fact]
		public void UpdateOrganisation_WithInvalidId_Throws()
		{
			// arrange
			var testOrg = new BL.Domain.Organisation.Organisation()
			{
				Name = "Test",
				Identifier = "test"
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				using (var ctx = factory.CreateContext())
				{
					ctx.Organisations.Add(testOrg);
					ctx.SaveChanges();

					testOrg.OrganisationId = 0;

					// act
					var repo = new OrganisationRepository(ctx);
					Action result = () => repo.UpdateOrganisation(testOrg);

					// assert
					var exception = Assert.Throws<ArgumentException>(result);
					Assert.Equal("Organisation to update not found.", exception.Message);
				}
			}
		}

		[Theory]
		[InlineData("", null)]
		[InlineData(null, "")]
		[InlineData(null, null)]
		public void UpdateOrganisation_WithInvalidObject_Throws(
			string newName, string newIdentifier
		)
		{
			// arrange
			var testOrg = new BL.Domain.Organisation.Organisation()
			{
				Name = "Test",
				Identifier = "test"
			};

			using (var factory = new CityOfIdeasDbContextFactory())
			{
				using (var ctx = factory.CreateContext())
				{
					ctx.Organisations.Add(testOrg);
					ctx.SaveChanges();

					testOrg.Name = newName;
					testOrg.Identifier = newIdentifier;

					// act
					var repo = new OrganisationRepository(ctx);
					Action result = () => repo.UpdateOrganisation(testOrg);

					// assert
					var exception = Assert.Throws<DbUpdateException>(result);
				}
			}
		}

		[Fact]
		public void DeleteOrganisation_WithValidId_DeletesOrganisation()
		{
			// arrange
			var testOrg = new BL.Domain.Organisation.Organisation()
			{
				Name = "Test",
				Identifier = "test"
			};
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				BL.Domain.Organisation.Organisation retrievedOrg0 = null;
				BL.Domain.Organisation.Organisation retrievedOrg1 = null;
				using (var ctx = factory.CreateContext())
				{
					ctx.Organisations.Add(testOrg);
					ctx.SaveChanges();
					
					// act
					var id = ctx.Organisations.FirstOrDefault().OrganisationId;
					var repo = new OrganisationRepository(ctx);
					retrievedOrg0 = repo.DeleteOrganisation(id);
					retrievedOrg1 = repo.ReadOrganisation(id);
				}
				
				// assert
				Assert.NotNull(retrievedOrg0);
				Assert.Equal(testOrg, retrievedOrg0);
				Assert.Null(retrievedOrg1);
			}
		}

		[Fact]
		public void DeleteOrganisation_WithInvalidId_Throws()
		{
			// arrange
			var invalidId = 0;
			
			using (var factory = new CityOfIdeasDbContextFactory())
			{
				using (var ctx = factory.CreateContext())
				{
					// act
					var repo = new OrganisationRepository(ctx);
					Action result = () => repo.DeleteOrganisation(invalidId);
					
					// assert
					var exception = Assert.Throws<ArgumentException>(result);
					Assert.Equal("Organisation to delete not found.", exception.Message);
				}
			}
		}
	}
}