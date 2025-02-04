using EventManagementSystem.Application.RegistrationManagement.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.Application.RegistrationManagement.Commands
{
	public class RegisterForEventCommand : IRequest<RegistrationDTO>
	{
		public int EventId { get; private set; } 
		public string Name { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }

		
		public RegisterForEventCommand(int eventId, string name, string phoneNumber, string email)
		{
			EventId = eventId;
			Name = name;
			PhoneNumber = phoneNumber;
			Email = email;
		}
	}


}
