using EventManagementSystem.Application.RegistrationManagement.Commands;
using FluentValidation.TestHelper;
using Xunit;

namespace EventManagementSystem.Tests.Validators
{
	
	public class RegisterForEventCommandValidatorTests
	{
		private readonly RegisterForEventCommandValidator _validator;

		public RegisterForEventCommandValidatorTests()
		{
			_validator = new RegisterForEventCommandValidator();
		}

		[Fact]
		public void Should_Have_Error_When_EventId_Is_Zero()
		{
			var command = new RegisterForEventCommand
			{
				EventId = 0,
				Name = "Valid Name",
				PhoneNumber = "1234567890",
				Email = "valid@email.com"
			};
			var result = _validator.TestValidate(command);
			result.ShouldHaveValidationErrorFor(x => x.EventId);
		}

		[Fact]
		public void Should_Have_Error_When_EventId_Is_Negative()
		{
			var command = new RegisterForEventCommand
			{
				EventId = -1,
				Name = "Valid Name",
				PhoneNumber = "1234567890",
				Email = "valid@email.com"
			};
			var result = _validator.TestValidate(command);
			result.ShouldHaveValidationErrorFor(x => x.EventId);
		}

		[Fact]
		public void Should_Have_Error_When_Name_Is_Empty()
		{
			var command = new RegisterForEventCommand
			{
				EventId = 10,
				Name = "",
				PhoneNumber = "1234567890",
				Email = "valid@email.com"
			};
			var result = _validator.TestValidate(command);
			result.ShouldHaveValidationErrorFor(x => x.Name);
		}

		[Fact]
		public void Should_Have_Error_When_Name_Is_Too_Short()
		{
			var command = new RegisterForEventCommand
			{
				EventId = 10,
				Name = "A",
				PhoneNumber = "1234567890",
				Email = "valid@email.com"
			};
			var result = _validator.TestValidate(command);
			result.ShouldHaveValidationErrorFor(x => x.Name);
		}

		[Fact]
		public void Should_Have_Error_When_PhoneNumber_Is_Invalid()
		{
			var command = new RegisterForEventCommand
			{
				EventId = 10,
				Name = "Valid Name",
				PhoneNumber = "abc123",
				Email = "valid@email.com"
			};
			var result = _validator.TestValidate(command);
			result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
		}

		[Fact]
		public void Should_Have_Error_When_Email_Is_Invalid()
		{
			var command = new RegisterForEventCommand
			{
				EventId = 10,
				Name = "Valid Name",
				PhoneNumber = "1234567890",
				Email = "invalid-email"
			};
			var result = _validator.TestValidate(command);
			result.ShouldHaveValidationErrorFor(x => x.Email);
		}

		[Fact]
		public void Should_Not_Have_Error_When_Command_Is_Valid()
		{
			var command = new RegisterForEventCommand
			{
				EventId = 10,
				Name = "Valid Name",
				PhoneNumber = "1234567890",
				Email = "valid@email.com"
			};
			var result = _validator.TestValidate(command);
			result.ShouldNotHaveAnyValidationErrors();
		}
	}

}
