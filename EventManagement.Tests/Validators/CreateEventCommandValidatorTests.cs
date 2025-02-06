using EventManagementSystem.Application.EventManagement.Commands.CreateEvent;
using FluentValidation.TestHelper;
using System;
using Xunit;

namespace EventManagementSystem.Tests.Validators
{

	public class CreateEventCommandValidatorTests
	{
		private readonly CreateEventCommandValidator _validator;

		public CreateEventCommandValidatorTests()
		{
			_validator = new CreateEventCommandValidator();
		}

		[Fact]
		public void Should_Have_Error_When_Name_Is_Empty()
		{
			var command = new CreateEventCommand
			{
				Name = "",
				Description = "Valid Description",
				Location = "Valid Location",
				StartTime = DateTime.UtcNow,
				EndTime = DateTime.UtcNow.AddHours(2)
			};

			var result = _validator.TestValidate(command);
			result.ShouldHaveValidationErrorFor(x => x.Name);
		}

		[Fact]
		public void Should_Have_Error_When_Location_Is_Empty()
		{
			var command = new CreateEventCommand
			{
				Name = "Event Name",
				Description = "Valid Description",
				Location = "",
				StartTime = DateTime.UtcNow,
				EndTime = DateTime.UtcNow.AddHours(2)
			};

			var result = _validator.TestValidate(command);
			result.ShouldHaveValidationErrorFor(x => x.Location);
		}

		[Fact]
		public void Should_Have_Error_When_StartTime_Is_After_EndTime()
		{
			var command = new CreateEventCommand
			{
				Name = "Event Name",
				Description = "Valid Description",
				Location = "Valid Location",
				StartTime = DateTime.UtcNow.AddHours(3),
				EndTime = DateTime.UtcNow
			};

			var result = _validator.TestValidate(command);
			result.ShouldHaveValidationErrorFor(x => x.StartTime);
		}

		[Fact]
		public void Should_Have_Error_When_StartTime_And_EndTime_Are_Same()
		{
			var time = DateTime.UtcNow;
			var command = new CreateEventCommand
			{
				Name = "Event Name",
				Description = "Valid Description",
				Location = "Valid Location",
				StartTime = time,
				EndTime = time
			};

			var result = _validator.TestValidate(command);
			result.ShouldHaveValidationErrorFor(x => x.StartTime);
		}

		[Fact]
		public void Should_Not_Have_Error_When_Command_Is_Valid()
		{
			var command = new CreateEventCommand
			{
				Name = "Event Name",
				Description = "Valid Description",
				Location = "Valid Location",
				StartTime = DateTime.UtcNow,
				EndTime = DateTime.UtcNow.AddHours(2)
			};

			var result = _validator.TestValidate(command);
			result.ShouldNotHaveAnyValidationErrors();
		}
	}

}
