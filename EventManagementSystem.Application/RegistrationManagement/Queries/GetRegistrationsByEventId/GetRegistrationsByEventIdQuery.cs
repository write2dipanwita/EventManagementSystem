using EventManagementSystem.Application.RegistrationManagement.DTOs;
using MediatR;

namespace EventManagementSystem.Application.RegistrationManagement.Queries.GetRegistrationsByEventId
{
	public class GetRegistrationsByEventIdQuery : IRequest<IEnumerable<RegistrationDTO>>
	{
		public int EventId { get; }
		public GetRegistrationsByEventIdQuery(int eventId)
		{
			EventId = eventId;
		}
	}
}
