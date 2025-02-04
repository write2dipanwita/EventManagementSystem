using AutoMapper;
using EventManagementSystem.Application.Common.Interfaces;
using EventManagementSystem.Application.RegistrationManagement.DTOs;
using EventManagementSystem.Core.RegistrationManagement.Repositories;
using MediatR;

namespace EventManagementSystem.Application.RegistrationManagement.Queries.GetRegistrationsByEventId
{
	public class GetRegistrationsByEventIdQueryHandler : IRequestHandler<GetRegistrationsByEventIdQuery, IEnumerable<RegistrationDTO>>
	{
		private readonly IRegistrationRepository _registrationRepository;
		private readonly IMapper _mapper;

		public GetRegistrationsByEventIdQueryHandler(IRegistrationRepository registrationRepository, IMapper mapper)
		{
			_registrationRepository = registrationRepository;
			_mapper = mapper;
		}

		public async Task<IEnumerable<RegistrationDTO>> Handle(GetRegistrationsByEventIdQuery request, CancellationToken cancellationToken)
		{
			var registrations = await _registrationRepository.GetRegistrationsByEventIdAsync(request.EventId);
			return _mapper.Map<IEnumerable<RegistrationDTO>>(registrations);
		}
	}

}
