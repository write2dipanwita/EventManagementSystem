using EventManagementSystem.Application.EventManagement.DTOs;
using MediatR;

namespace EventManagementSystem.Application.EventManagement.Queries.GetALLEvents
{
	public class GetAllEventsQuery : IRequest<IEnumerable<EventDTO>>
	{
	}
}