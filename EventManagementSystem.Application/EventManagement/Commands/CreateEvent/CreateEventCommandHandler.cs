using AutoMapper;
using EventManagementSystem.Application.Common.Interfaces;
using EventManagementSystem.Application.EventManagement.DTOs;
using EventManagementSystem.Core.EventManagement.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.Application.EventManagement.Commands.CreateEvent
{
	public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, EventDTO>
	{
		private readonly IEventRepository _eventRepository;
		private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly ILogger<CreateEventCommandHandler> _logger;

		public CreateEventCommandHandler(
			IEventRepository eventRepository,
			IMapper mapper,
			IHttpContextAccessor httpContextAccessor,
			ILogger<CreateEventCommandHandler> logger)
		{
			_eventRepository = eventRepository;
			_mapper = mapper;
			_httpContextAccessor = httpContextAccessor;
			_logger = logger;
		}
		public async Task<EventDTO> Handle(CreateEventCommand request, CancellationToken cancellationToken)
		{
			var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId))
			{
				throw new UnauthorizedAccessException("User not authenticated.");
			}

			if (request.StartTime >= request.EndTime)
			{
				throw new ValidationException("Start time must be before end time.");
			}			

			var eventEntity = new Event(
				request.Name,
				request.Description,
				request.Location,
				request.StartTime,
				request.EndTime,
				userId
			);

			await _eventRepository.AddAsync(eventEntity);
			return _mapper.Map<EventDTO>(eventEntity);
		}

		
	}
}
