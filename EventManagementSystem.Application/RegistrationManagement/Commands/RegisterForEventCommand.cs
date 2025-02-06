using EventManagementSystem.Application.RegistrationManagement.DTOs;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.Application.RegistrationManagement.Commands
{
	public class RegisterForEventCommand : IRequest<RegistrationDTO>
	{
		public int EventId { get; set; }  
		public string Name { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }
		
	}
}
