using AutoMapper;
using EventManagementSystem.Application.Common.Interfaces;
using EventManagementSystem.Application.EventManagement.DTOs;
using EventManagementSystem.Core.EventManagement.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EventManagementSystem.Application.EventManagement.Commands.CreateEvent
{
	public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, EventDTO>
	{
		private readonly IEventRepository _eventRepository;
		private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public CreateEventCommandHandler(IEventRepository eventRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
		{
			_eventRepository = eventRepository;
			_mapper = mapper;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<EventDTO> Handle(CreateEventCommand request, CancellationToken cancellationToken)
		{
			var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (request.StartTime >= request.EndTime)
			{
				throw new ArgumentException("Start time must be before end time.");
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
