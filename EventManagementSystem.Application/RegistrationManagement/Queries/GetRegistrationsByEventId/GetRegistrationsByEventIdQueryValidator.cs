using FluentValidation;
using EventManagementSystem.Application.RegistrationManagement.Queries.GetRegistrationsByEventId;


namespace EventManagementSystem.Application.RegistrationManagement.Queries.GetRegistrationsByEventId
{
	
	public class GetRegistrationsByEventIdQueryValidator : AbstractValidator<GetRegistrationsByEventIdQuery>
	{
		public GetRegistrationsByEventIdQueryValidator()
		{
			RuleFor(x => x.EventId)
				.GreaterThan(0).WithMessage("Event ID must be greater than zero.");
		}
	}

}
