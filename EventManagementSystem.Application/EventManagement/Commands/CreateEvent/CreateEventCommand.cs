using EventManagementSystem.Application.EventManagement.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.Application.EventManagement.Commands.CreateEvent
{
	public class CreateEventCommand : IRequest<EventDTO>
	{
		[Required(ErrorMessage = "Event name is required.")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Event description is required.")]
		public string Description { get; set; }

		[Required(ErrorMessage = "Event location is required.")]
		public string Location { get; set; }

		[Required(ErrorMessage = "Start time is required.")]
		public DateTime StartTime { get; set; }

		[Required(ErrorMessage = "End time is required.")]
		public DateTime EndTime { get; set; }

		public string CreatedBy { get; set; }
	}
}
