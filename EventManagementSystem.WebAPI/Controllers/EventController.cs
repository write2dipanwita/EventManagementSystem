using EventManagementSystem.Application.EventManagement.Commands.CreateEvent;
using EventManagementSystem.Application.EventManagement.DTOs;
using EventManagementSystem.Application.EventManagement.Services;
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
		private readonly IEventService _eventService;
		private readonly ILogger<EventController> _logger;

		public EventController(IMediator mediator, ILogger<EventController> logger, IEventService eventService)
		{
			_mediator = mediator;
			_logger = logger;
			_eventService = eventService;
		}

		
		[HttpGet]
		public async Task<ActionResult<IEnumerable<EventDTO>>> GetAllEvents()
		{
			var events = await _eventService.GetAllEventsAsync();

			if (events == null || !events.Any())
			{
				_logger.LogWarning("No events found.");
				return NotFound("No events available.");
			}

			_logger.LogInformation("Retrieved {Count} events successfully.", events.Count());
			return Ok(events);
		}

	
		[Authorize(Roles = "Admin")] 
		[HttpPost("create")]
		public async Task<IActionResult> CreateEvent([FromBody, Required] CreateEventCommand command)
		{
			_logger.LogInformation("Received event creation request: {EventName}", command.Name);
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId))
			{
				_logger.LogWarning("Unauthorized user attempted to create an event.");
				return Unauthorized("User not found.");
			}

			command.CreatedBy = userId;

			var createdEvent = await _mediator.Send(command);

			_logger.LogInformation("Event '{EventName}' successfully created by User ID {UserId}.", createdEvent.Name, userId);

			return CreatedAtAction(nameof(GetAllEvents), new { id = createdEvent.Id }, createdEvent);
		}
	}
}
