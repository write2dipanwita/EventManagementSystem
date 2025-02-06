using EventManagementSystem.Core.EventManagement.Entities;
using EventManagementSystem.Core.RegistrationManagement.Entities;

namespace EventManagementSystem.Core.RegistrationManagement.Repositories
{
	public interface IRegistrationRepository
	{
		Task<IEnumerable<Registration>> GetRegistrationsByEventIdAsync(int eventId);
		Task<Registration> AddAsync(Registration registration);
	}
}
