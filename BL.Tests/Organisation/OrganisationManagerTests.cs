using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using COI.BL.Ideation;
using COI.BL.Organisation;
using COI.DAL.Organisation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Moq;
using Xunit;

namespace COI.BL.Tests.Organisation
{
	public class OrganisationManagerTests
	{
		private Mock<IOrganisationRepository> _organisationRepository;
		private IOrganisationManager _organisationManager;
		private Domain.Organisation.Organisation testOrg;
		private string testName;
		private string testIdentifier;
		
		public OrganisationManagerTests()
		{
			_organisationRepository = new Mock<IOrganisationRepository>();
			_organisationManager = new OrganisationManager(_organisationRepository.Object);
			testName = "Test";
			testIdentifier = "test";
			testOrg = new BL.Domain.Organisation.Organisation()
			{
				Name = testName,
				Identifier = testIdentifier
			};
		}

		[Fact]
		public void GetOrganisations_WithEmptyRepo_ReturnsEmptyCollection()
		{
			// arrange
			_organisationRepository
				.Setup(x => x.ReadOrganisations())
				.Returns(new List<Domain.Organisation.Organisation>());
			
			// act
			var result = _organisationManager.GetOrganisations();
			
			// assert
			Assert.NotNull(result);
			Assert.Empty(result);
		}

		[Fact]
		public void GetOrganisations_WithData_ReturnsCollection()
		{
			// arrange
			_organisationRepository
				.Setup(x => x.ReadOrganisations())
				.Returns(new List<Domain.Organisation.Organisation>() {testOrg});
			
			// act
			var result = _organisationManager.GetOrganisations();
			
			// assert
			Assert.NotNull(result);
			Assert.Single(result);
			Assert.Equal(testOrg, result.FirstOrDefault());
		}

		[Fact]
		public void GetOrganisation_WithValidId_ReturnsObject()
		{
			// arrange
			_organisationRepository
				.Setup(x => x.ReadOrganisation(It.IsAny<int>()))
				.Returns(testOrg);
			const int orgId = 1;
			
			// act
			var result = _organisationManager.GetOrganisation(orgId);
			
			// assert
			Assert.NotNull(result);
			Assert.Equal(testOrg, result);
		}

		[Fact]
		public void GetOrganisation_WithInvalidId_ReturnsNull()
		{
			// arrange
			_organisationRepository
				.Setup(x => x.ReadOrganisation(It.IsAny<int>()))
				.Returns((Domain.Organisation.Organisation) null);
			const int orgId = 1;
			
			// act
			var result = _organisationManager.GetOrganisation(orgId);
			
			// assert
			Assert.Null(result);
		}

		[Fact]
		public void AddOrganisation_WithValidObject_ReturnsObjectWithId()
		{
			// arrange
			_organisationRepository
				.Setup(x => x.CreateOrganisation(It.IsAny<Domain.Organisation.Organisation>()))
				.Returns<Domain.Organisation.Organisation>(x => x);
			
			// act
			var result = _organisationManager.AddOrganisation(testName, testIdentifier);
			
			// assert
			Assert.NotNull(result);
//			Assert.Equal(testOrg, result);
		}

		[Fact]
		public void AddOrganisation_WithDuplicateObject_Throws()
		{
			// arrange
			_organisationRepository
				.Setup(x => x.CreateOrganisation(It.IsAny<Domain.Organisation.Organisation>()))
				.Throws<ArgumentException>();
			
			// act
			Action result = () => _organisationManager.AddOrganisation(testName, testIdentifier);
			
			// assert
			var exception = Assert.Throws<ArgumentException>(result);
		}
		
        public static IEnumerable<object[]> GetOrganisations()
		{
			yield return new object[]
			{
				null, null
			};
			yield return new object[]
			{
				"", ""
			};
			yield return new object[]
			{
				null, "i"
			};
			yield return new object[]
			{
				"", "i"
			};
			yield return new object[]
			{
				"i", null
			};
			yield return new object[]
			{
				"i", ""
			};
		}

        [Theory]
		[MemberData(nameof(GetOrganisations))]
        public void AddOrganisation_WithInvalidObject_Throws(
	        string name, string identifier)
        {
	        // arrange
	        
	        // act
			Action result = () => _organisationManager.AddOrganisation(name, identifier);
			
			// assert
			Assert.Throws<ValidationException>(result);
        }

        [Fact]
        public void ChangeOrganisation_WithValidObject_ReturnsUpdatedObject()
        {
			// arrange
			const int orgId = 1;
			_organisationRepository
				.Setup(x => x.ReadOrganisation(orgId))
				.Returns(testOrg);
			_organisationRepository
				.Setup(x => x.UpdateOrganisation(It.IsAny<Domain.Organisation.Organisation>()))
				.Returns<Domain.Organisation.Organisation>(x => x);
			string newName = "New Test";
			
			// act
			var result = _organisationManager.ChangeOrganisation(orgId, newName, testIdentifier);
			
			// assert
			Assert.NotNull(result);
        }

        [Fact]
        public void ChangeOrganisation_WithInvalidId_Throws()
        {
	        // arrange
	        const int orgId = 0;
	        _organisationRepository
		        .Setup(x => x.ReadOrganisation(orgId))
		        .Returns((Domain.Organisation.Organisation) null);
	        
	        // act
			Action result = () => _organisationManager.ChangeOrganisation(orgId, testName, testIdentifier);
			
			// assert
			Assert.Throws<ArgumentException>(result);
        }

        [Theory]
        [MemberData(nameof(GetOrganisations))]
        public void ChangeOrganisation_WithInvalidValues_Throws(
	        string newName, string newIdentifier
        )
        {
	        // arrange
	        const int orgId = 1;
			_organisationRepository
				.Setup(x => x.ReadOrganisation(orgId))
				.Returns(testOrg);
	        
	        // act
	        Action result = () => _organisationManager.ChangeOrganisation(orgId, newName, newIdentifier);
			
			// assert
			Assert.Throws<ValidationException>(result);
        }

        [Fact]
        public void RemoveOrganisation_WithValidId_DeletesOrganisation()
        {
	        // arrange
	        const int orgId = 1;
			_organisationRepository
				.Setup(x => x.DeleteOrganisation(orgId))
				.Returns(testOrg);
	        
	        // act
	        var result = _organisationManager.RemoveOrganisation(orgId);

	        // assert
	        Assert.NotNull(result);
	        Assert.Equal(testOrg, result);
        }
        
        [Fact]
        public void RemoveOrganisation_WithInvalidId_Throws()
        {
	        // arrange
	        const int orgId = 1;
	        _organisationRepository
		        .Setup(x => x.DeleteOrganisation(orgId))
		        .Throws<ArgumentException>();
	        
	        // act
	        Action result = () => _organisationManager.RemoveOrganisation(orgId);
			
			// assert
			Assert.Throws<ArgumentException>(result);
        }
	}
}