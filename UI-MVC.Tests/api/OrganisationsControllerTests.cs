using System.Collections.Generic;
using AutoMapper;
using COI.BL.Domain.Organisation;
using COI.BL.Organisation;
using COI.UI.MVC.Controllers.api;
using COI.UI.MVC.Models.DTO.Organisation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace COI.UI.MVC.Tests.api
{
	public class OrganisationsControllerTests
	{
		private readonly Mock<IOrganisationManager> mgr;
		private readonly IMapper _mapper;
		private readonly OrganisationsController ctrl;
		private Organisation testOrg;
		private string testName;
		private string testIdentifier;

		public OrganisationsControllerTests()
		{
			var config = new MapperConfiguration(opts =>
			{
				
			});
			_mapper = config.CreateMapper();
			mgr = new Mock<IOrganisationManager>();
			ctrl = new OrganisationsController(_mapper, mgr.Object);
			
			testName = "Test";
			testIdentifier = "test";
			testOrg = new Organisation()
			{
				Name = testName,
				Identifier = testIdentifier
			};
		}
		
		[Fact]
		public void GetOrganisations_WithEmptyMgr_ReturnsOkResult()
		{
			// arrange
			mgr.Setup(x => x.GetOrganisations())
				.Returns(new List<Organisation>());
			
			// act
			IActionResult result = ctrl.GetOrganisations();

			// assert
//			Assert.Null(null);
			var okResult = Assert.IsType<OkObjectResult>(result);
			var items = Assert.IsType<List<OrganisationDto>>(okResult.Value);
			Assert.Empty(items);
		}

		[Fact]
		public void GetOrganisations_WithData_ReturnsOkResult()
		{
			// arrange
			mgr.Setup(x => x.GetOrganisations())
				.Returns(new List<Organisation>(){testOrg});
			
			// act
			IActionResult result = ctrl.GetOrganisations();

			// assert
//			Assert.Null(null);
			var okResult = Assert.IsType<OkObjectResult>(result);
			var items = Assert.IsType<List<OrganisationDto>>(okResult.Value);
			Assert.Single(items);
		}
	}
}