using System;
using Xunit;

namespace COI.BL.Domain.Tests.User
{
	public class UserTests
	{
		[Fact]
		public void GetName_WithNoFirstOrLastNameSet_ReturnsEmptyString()
		{
			// arrange
			var user = new Domain.User.User();
			
			// act
			var name = user.GetName();
			
			// assert
			Assert.Empty(name);
		}
		
		[Fact]
		public void GetName_WithNoLastNameSet_ReturnsOnlyFirstName()
		{
			// arrange
			var fakename = "Jos";
			var user = new Domain.User.User()
			{
				FirstName = fakename
			};
			
			// act
			var name = user.GetName();
			
			// assert
			Assert.Equal(fakename, name);
		}
		
		[Fact]
		public void GetName_WithNoFirstNameSet_ReturnsOnlyLastName()
		{
			// arrange
			var fakename = "Peeters";
			var user = new Domain.User.User()
			{
				LastName = fakename
			};
			
			// act
			var name = user.GetName();
			
			// assert
			Assert.Equal(fakename, name);
		}
		
		[Fact]
		public void GetName_WithFullNameSet_ReturnsFullNameWithSpaceInBetween()
		{
			// arrange
			var fakefirstname = "Fons";
			var fakelastname = "Peeters";
			var user = new Domain.User.User()
			{
				FirstName = fakefirstname,
				LastName = fakelastname
			};
			
			// act
			var name = user.GetName();
			
			// assert
			Assert.Equal(String.Format("{0} {1}", fakefirstname, fakelastname), name);
		}
	}
}