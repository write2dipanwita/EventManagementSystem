using EventManagementSystem.Core.EventManagement.Entities;
using EventManagementSystem.Core.RegistrationManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.Core.RegistrationManagement.Repositories
{
	public interface IRegistrationRepository
	{
		Task<IEnumerable<Registration>> GetRegistrationsByEventIdAsync(int eventId);
		Task<Registration> AddAsync(Registration registration);
	}
}
