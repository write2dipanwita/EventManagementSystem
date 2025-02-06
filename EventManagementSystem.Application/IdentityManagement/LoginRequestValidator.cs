using FluentValidation;

namespace EventManagementSystem.Application.IdentityManagement
{
	
	public class LoginRequestValidator : AbstractValidator<LoginRequest>
	{
		public LoginRequestValidator()
		{
			RuleFor(x => x.Username)
				.NotEmpty().WithMessage("Username is required.")
				.MinimumLength(4).WithMessage("Username must be at least 4 characters long.");

			RuleFor(x => x.Password)
				.NotEmpty().WithMessage("Password is required.")
				.MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
		}
	}
}
