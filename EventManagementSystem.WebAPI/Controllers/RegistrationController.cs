
using EventManagementSystem.Application.RegistrationManagement.Commands;
using EventManagementSystem.Application.RegistrationManagement.Queries.GetRegistrationsByEventId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementSystem.WebAPI.Controllers
{
	[ApiController]
	[Route("api/registrations")]
	public class RegistrationController : ControllerBase
	{
		private readonly IMediator _mediator;

		public RegistrationController(IMediator mediator)
		{
			_mediator = mediator;
		}
		[HttpGet("{eventId}/registrations")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetRegistrationsByEventId(int eventId)
		{
			try
			{
				var registrations = await _mediator.Send(new GetRegistrationsByEventIdQuery(eventId));

				if (registrations == null || !registrations.Any())
					return NotFound($"No registrations found for event {eventId}.");

				return Ok(registrations);
			}
			catch (Exception ex)
			{
				//_logger.LogError(ex, $"Error retrieving registrations for event {eventId}");
				return StatusCode(500, "An error occurred while retrieving registrations.");
			}
		}
		[HttpPost("{eventId}/register")]
		[AllowAnonymous] 
		public async Task<IActionResult> RegisterForEvent(int eventId, [FromBody] RegisterForEventCommand command)
		{
			try
			{
				var registerCommand = new RegisterForEventCommand(eventId, command.Name, command.PhoneNumber, command.Email);

				var registration = await _mediator.Send(registerCommand);

				return CreatedAtAction(nameof(GetRegistrationsByEventId), new { eventId }, registration);
			}
			catch (Exception ex)
			{
				//_logger.LogError(ex, $"Error registering for event {eventId}");
				return StatusCode(500, "An error occurred while processing your registration.");
			}
		}
	}
}
