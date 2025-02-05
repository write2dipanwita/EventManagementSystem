using AutoMapper;
using EventManagementSystem.Application.Common.Interfaces;
using EventManagementSystem.Application.EventManagement.DTOs;
using Microsoft.Extensions.Logging;

namespace EventManagementSystem.Application.EventManagement.Services
{
	public class EventService : IEventService
	{
		private readonly IEventRepository _eventRepository;
		private readonly IMapper _mapper;

		public EventService(IEventRepository eventRepository, IMapper mapper)
		{
			_eventRepository = eventRepository;
			_mapper = mapper;
		}

		public async Task<IEnumerable<EventDTO>> GetAllEventsAsync()
		{

			var events = await _eventRepository.GetAllAsync();

			if (events == null || !events.Any())
			{
				return new List<EventDTO>();
			}

			return _mapper.Map<List<EventDTO>>(events);
		}
	}
}
