using EventManagementSystem.Application.RegistrationManagement.Commands;
using EventManagementSystem.Application.RegistrationManagement.Queries.GetRegistrationsByEventId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.WebAPI.Controllers
{
	[ApiController]
	[Route("api/registrations")]
	public class RegistrationController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly ILogger<RegistrationController> _logger;

		public RegistrationController(IMediator mediator, ILogger<RegistrationController> logger)
		{
			_mediator = mediator;
			_logger = logger;
		}

		
		[HttpGet("{eventId}/registrations")]
		[Authorize(Roles = "Admin")] 
		public async Task<IActionResult> GetRegistrationsByEventId([FromRoute, Range(1, int.MaxValue, ErrorMessage = "Event ID must be greater than zero.")] int eventId)
		{
			_logger.LogInformation("Retrieving registrations for event ID {EventId}.", eventId);

			var registrations = await _mediator.Send(new GetRegistrationsByEventIdQuery(eventId));

			if (registrations == null || !registrations.Any())
			{
				_logger.LogWarning("No registrations found for event ID {EventId}.", eventId);
				return NotFound($"No registrations found for event {eventId}.");
			}

			_logger.LogInformation("Retrieved {Count} registrations for event ID {EventId}.", registrations.Count(), eventId);
			return Ok(registrations);
		}

		
		[HttpPost("{eventId}/register")]
		[AllowAnonymous] 
		public async Task<IActionResult> RegisterForEvent([FromRoute, Range(1, int.MaxValue, ErrorMessage = "Event ID must be greater than zero.")] int eventId,
			[FromBody, Required] RegisterForEventCommand command)
		{
			_logger.LogInformation("Received registration request for event ID {EventId} from {UserName}.", eventId, command.Name);

			var registerCommand = new RegisterForEventCommand(eventId, command.Name, command.PhoneNumber, command.Email);
			var registration = await _mediator.Send(registerCommand);

			_logger.LogInformation("User {UserName} successfully registered for event ID {EventId}.", command.Name, eventId);

			return CreatedAtAction(nameof(GetRegistrationsByEventId), new { eventId }, registration);


		}
	}
}
