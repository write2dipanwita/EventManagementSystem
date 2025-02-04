using AutoMapper;
using EventManagementSystem.Application.EventManagement.DTOs;
using EventManagementSystem.Core.EventManagement.Entities;


namespace EventManagementSystem.Application.EventManagement.Mappings
{
	public class EventProfile : Profile
	{
		public EventProfile()
		{
			CreateMap<Event, EventDTO>();
		}
	}
}
