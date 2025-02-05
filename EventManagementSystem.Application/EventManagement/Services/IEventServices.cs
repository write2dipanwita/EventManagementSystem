using EventManagementSystem.Application.EventManagement.DTOs;

namespace EventManagementSystem.Application.EventManagement.Services
{
	public interface IEventService
	{
		Task<IEnumerable<EventDTO>> GetAllEventsAsync();
	}
}
