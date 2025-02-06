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
		private readonly IMapper _mapper;
		private readonly ILogger<RegisterForEventCommandHandler> _logger;


		public RegisterForEventCommandHandler(IRegistrationRepository registrationRepository, IMapper mapper, ILogger<RegisterForEventCommandHandler> logger)
		{
			_registrationRepository = registrationRepository;
			_mapper = mapper;
			_logger = logger;
		}

		public async Task<RegistrationDTO> Handle(RegisterForEventCommand request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Received registration request for Event ID {EventId} by {Email}.", request.EventId, request.Email);

			var registration = new Registration(request.EventId, request.Name, request.PhoneNumber, request.Email);

			await _registrationRepository.AddAsync(registration);
			_logger.LogInformation("Successfully registered {Email} for Event ID {EventId}.", request.Email, request.EventId);


			return _mapper.Map<RegistrationDTO>(registration);
		}
	}


}
