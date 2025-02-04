using AutoMapper;
using EventManagementSystem.Application.RegistrationManagement.DTOs;
using EventManagementSystem.Core.RegistrationManagement.Entities;

namespace EventManagementSystem.Application.RegistrationManagement.Mappings
{
	public class RegsitrationProfile : Profile
	{
		public RegsitrationProfile()
		{
			CreateMap<Registration, RegistrationDTO>();
		}
	}

}
