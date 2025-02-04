using EventManagementSystem.Application.EventManagement.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.Application.EventManagement.Commands.CreateEvent
{
	public class CreateEventCommand : IRequest<EventDTO>
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string Location { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public string CreatedBy { get; set; }
	}
}
