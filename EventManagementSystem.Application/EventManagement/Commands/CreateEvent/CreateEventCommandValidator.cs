using FluentValidation;
using EventManagementSystem.Application.EventManagement.Commands.CreateEvent;

public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
	public CreateEventCommandValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("Event name is required.")
			.MaximumLength(100).WithMessage("Event name cannot exceed 100 characters.");

		RuleFor(x => x.Description)
			.NotEmpty().WithMessage("Event description is required.");

		RuleFor(x => x.Location)
			.NotEmpty().WithMessage("Event location is required.");

		RuleFor(x => x.StartTime)
			.LessThan(x => x.EndTime).WithMessage("Start time must be before end time.");

		
	}
}
