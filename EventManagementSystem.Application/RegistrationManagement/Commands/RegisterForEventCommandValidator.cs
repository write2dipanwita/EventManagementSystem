using FluentValidation;
using EventManagementSystem.Application.RegistrationManagement.Commands;

public class RegisterForEventCommandValidator : AbstractValidator<RegisterForEventCommand>
{
	public RegisterForEventCommandValidator()
	{
		RuleFor(x => x.EventId)
			.GreaterThan(0).WithMessage("Event ID must be greater than zero.");

		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("Participant name is required.")
			.MinimumLength(3).WithMessage("Name must be at least 3 characters long.")
			.MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

		RuleFor(x => x.PhoneNumber)
			.NotEmpty().WithMessage("Phone number is required.")
			.Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format.");

		RuleFor(x => x.Email)
			.NotEmpty().WithMessage("Email is required.")
			.EmailAddress().WithMessage("Invalid email format.");
	}
}
