using AutoMapper;
using EventManagementSystem.Application.RegistrationManagement.DTOs;
using EventManagementSystem.Core.RegistrationManagement.Entities;
using EventManagementSystem.Core.RegistrationManagement.Repositories;
using MediatR;

namespace EventManagementSystem.Application.RegistrationManagement.Commands
{
	public class RegisterForEventCommandHandler : IRequestHandler<RegisterForEventCommand, RegistrationDTO>
	{
		private readonly IRegistrationRepository _registrationRepository;
		private readonly IMapper _mapper;

		public RegisterForEventCommandHandler(IRegistrationRepository registrationRepository, IMapper mapper)
		{
			_registrationRepository = registrationRepository;
			_mapper = mapper;
		}

		public async Task<RegistrationDTO> Handle(RegisterForEventCommand request, CancellationToken cancellationToken)
		{
			var registration = new Registration(request.EventId, request.Name, request.PhoneNumber, request.Email);

			await _registrationRepository.AddAsync(registration);
			return _mapper.Map<RegistrationDTO>(registration);
		}
	}


}
