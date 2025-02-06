
using EventManagementSystem.Application.RegistrationManagement.Queries.GetRegistrationsByEventId;
using FluentValidation.TestHelper;
using Xunit;
namespace EventManagementSystem.Tests.Validators
{


	public class GetRegistrationsByEventIdQueryValidatorTests
	{
		private readonly GetRegistrationsByEventIdQueryValidator _validator;

		public GetRegistrationsByEventIdQueryValidatorTests()
		{
			_validator = new GetRegistrationsByEventIdQueryValidator();
		}

		[Fact]
		public void Should_Have_Error_When_EventId_Is_Zero()
		{
			var query = new GetRegistrationsByEventIdQuery(0);
			var result = _validator.TestValidate(query);
			result.ShouldHaveValidationErrorFor(x => x.EventId);
		}

		[Fact]
		public void Should_Have_Error_When_EventId_Is_Negative()
		{
			var query = new GetRegistrationsByEventIdQuery(-5);
			var result = _validator.TestValidate(query);
			result.ShouldHaveValidationErrorFor(x => x.EventId);
		}

		[Fact]
		public void Should_Not_Have_Error_When_EventId_Is_Valid()
		{
			var query = new GetRegistrationsByEventIdQuery(10);
			var result = _validator.TestValidate(query);
			result.ShouldNotHaveAnyValidationErrors();
		}
	}

}
