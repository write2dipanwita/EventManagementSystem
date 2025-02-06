using AutoMapper;
using EventManagementSystem.Application.Common.Interfaces;
using EventManagementSystem.Application.EventManagement.DTOs;
using MediatR;

namespace EventManagementSystem.Application.EventManagement.Queries.GetALLEvents
{
	public class GetAllEventsQueryHandler : IRequestHandler<GetAllEventsQuery, IEnumerable<EventDTO>>
	{
		private readonly IEventRepository _eventRepository;
		private readonly IMapper _mapper;

		public GetAllEventsQueryHandler(IEventRepository eventRepository, IMapper mapper)
		{
			_eventRepository = eventRepository;
			_mapper = mapper;
		}

		public async Task<IEnumerable<EventDTO>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
		{
			var events = await _eventRepository.GetAllAsync();
			return _mapper.Map<List<EventDTO>>(events);
		}
	}
}