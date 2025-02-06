using EventManagementSystem.Core.EventManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.Application.Common.Interfaces
{
	public interface IEventRepository
	{
		Task<IEnumerable<Event>> GetAllAsync();
		Task<Event> AddAsync(Event newEvent);
		Task<bool> EventExistsAsync(string name, DateTime startTime, string location);
	}
}
