using EventManagementSystem.Application.IdentityManagement;
using FluentValidation.TestHelper;
using Xunit;

namespace EventManagementSystem.Tests.Validators
{
	

	public class LoginRequestValidatorTests
	{
		private readonly LoginRequestValidator _validator;

		public LoginRequestValidatorTests()
		{
			_validator = new LoginRequestValidator();
		}

		[Fact]
		public void Should_Have_Error_When_Username_Is_Empty()
		{
			var request = new LoginRequest { Username = "", Password = "ValidPass123!" };
			var result = _validator.TestValidate(request);
			result.ShouldHaveValidationErrorFor(x => x.Username);
		}

		[Fact]
		public void Should_Have_Error_When_Username_Is_Too_Short()
		{
			var request = new LoginRequest { Username = "abc", Password = "ValidPass123!" };
			var result = _validator.TestValidate(request);
			result.ShouldHaveValidationErrorFor(x => x.Username);
		}

		[Fact]
		public void Should_Have_Error_When_Password_Is_Empty()
		{
			var request = new LoginRequest { Username = "ValidUser", Password = "" };
			var result = _validator.TestValidate(request);
			result.ShouldHaveValidationErrorFor(x => x.Password);
		}

		[Fact]
		public void Should_Have_Error_When_Password_Is_Too_Short()
		{
			var request = new LoginRequest { Username = "ValidUser", Password = "123" };
			var result = _validator.TestValidate(request);
			result.ShouldHaveValidationErrorFor(x => x.Password);
		}

		[Fact]
		public void Should_Not_Have_Error_When_Request_Is_Valid()
		{
			var request = new LoginRequest { Username = "ValidUser", Password = "SecurePass123!" };
			var result = _validator.TestValidate(request);
			result.ShouldNotHaveAnyValidationErrors();
		}
	}

}
