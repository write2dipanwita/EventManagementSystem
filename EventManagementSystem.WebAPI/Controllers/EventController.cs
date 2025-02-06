using EventManagementSystem.Application.EventManagement.Commands.CreateEvent;
using EventManagementSystem.Application.EventManagement.DTOs;
using EventManagementSystem.Application.EventManagement.Queries.GetALLEvents;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace EventManagementSystem.WebAPI.Controllers
{
	[ApiController]
	[Route("api/events")]
	public class EventController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly ILogger<EventController> _logger;

		public EventController(IMediator mediator, ILogger<EventController> logger)
		{
			_mediator = mediator;
			_logger = logger;
		}


		[HttpGet]
		public async Task<ActionResult<IEnumerable<EventDTO>>> GetAllEvents()
		{
			try
			{
				var events = await _mediator.Send(new GetAllEventsQuery());

				if (events == null || !events.Any())
					return NotFound("No events found.");

				return Ok(events);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error retrieving events.");
				return StatusCode(500, "An error occurred while retrieving events.");
			}

		}

		[Authorize(Roles = "Admin")] 
		[HttpPost("create")]
		public async Task<IActionResult> CreateEvent([FromBody] CreateEventCommand command)
		{
			_logger.LogInformation("Received event creation request: {EventName}", command.Name);
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId))
			{
				throw new UnauthorizedAccessException("User ID not found.");
			}


			var createdEvent = await _mediator.Send(command);

			_logger.LogInformation("Event '{EventName}' successfully created by User ID {UserId}.", createdEvent.Name, userId);

			return CreatedAtAction(nameof(GetAllEvents), new { id = createdEvent.Id }, createdEvent);
		}
	}
}
