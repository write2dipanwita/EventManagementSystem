using EventManagementSystem.Application.Common.Interfaces;
using EventManagementSystem.Core.RegistrationManagement.Entities;
using EventManagementSystem.Core.RegistrationManagement.Repositories;
using EventManagementSystem.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.Infrastructure.Repositories
{
	public class RegistrationRepository : IRegistrationRepository
	{
		private readonly ApplicationDbContext _context;

		public RegistrationRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		
		public async Task<IEnumerable<Registration>> GetRegistrationsByEventIdAsync(int eventId)
		{
			return await _context.Registrations
				.Where(r => r.EventId == eventId)
				.ToListAsync();
		}
		public async Task<Registration> AddAsync(Registration registration)
		{
			await _context.Registrations.AddAsync(registration);
			await _context.SaveChangesAsync();
			return registration;
		}

	}
}
