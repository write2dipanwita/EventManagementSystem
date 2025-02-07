using AutoMapper;
using EventManagementSystem.Application.Common.Interfaces;
using EventManagementSystem.Application.RegistrationManagement.DTOs;
using EventManagementSystem.Core.RegistrationManagement.Entities;
using EventManagementSystem.Core.RegistrationManagement.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventManagementSystem.Application.RegistrationManagement.Commands
{
	public class RegisterForEventCommandHandler : IRequestHandler<RegisterForEventCommand, RegistrationDTO>
	{
		private readonly IRegistrationRepository _registrationRepository;
		private readonly IEventRepository _eventRepository;
		private readonly IMapper _mapper;
		private readonly ILogger<RegisterForEventCommandHandler> _logger;


		public RegisterForEventCommandHandler(IRegistrationRepository registrationRepository, IEventRepository eventRepository, IMapper mapper, ILogger<RegisterForEventCommandHandler> logger)
		{
			_registrationRepository = registrationRepository;
			_eventRepository = eventRepository;
			_mapper = mapper;
			_logger = logger;
		}

		public async Task<RegistrationDTO> Handle(RegisterForEventCommand request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Received registration request for Event ID {EventId} by {Email}.", request.EventId, request.Email);
			var allEvents = await _eventRepository.GetAllAsync();

			// Filter to find the specific event by EventId
			var eventEntity = allEvents.FirstOrDefault(e => e.Id == request.EventId);

			if (eventEntity == null)
			{
				_logger.LogWarning("Registration failed: Event ID {EventId} does not exist.", request.EventId);
				throw new InvalidOperationException("Event does not exist.");
			}
			var registration = new Registration(request.EventId, request.Name, request.PhoneNumber, request.Email);

			await _registrationRepository.AddAsync(registration);
			_logger.LogInformation("Successfully registered {Email} for Event ID {EventId}.", request.Email, request.EventId);


			return _mapper.Map<RegistrationDTO>(registration);
		}
	}


}
