using EventManagementSystem.Application.Common.Interfaces;
using EventManagementSystem.Core.RegistrationManagement.Entities;
using EventManagementSystem.Core.RegistrationManagement.Repositories;
using EventManagementSystem.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
		private readonly ILogger<RegistrationRepository> _logger;

		public RegistrationRepository(ApplicationDbContext context, ILogger<RegistrationRepository> logger)
		{
			_context = context;
			_logger = logger;
		}

		
		public async Task<IEnumerable<Registration>> GetRegistrationsByEventIdAsync(int eventId)
		{
			_logger.LogInformation("Fetching registrations for event ID {EventId}.", eventId);

			var registrations = await _context.Registrations
				.Where(r => r.EventId == eventId)
				.AsNoTracking()
				.ToListAsync();

			if (!registrations.Any())
			{
				_logger.LogWarning("No registrations found for event ID {EventId}.", eventId);
			}

			return registrations;
		}
		public async Task<Registration> AddAsync(Registration registration)
		{
			_logger.LogInformation("Adding new registration for event ID {EventId}, Email: {Email}", registration.EventId, registration.Email);

			await _context.Registrations.AddAsync(registration);
			await _context.SaveChangesAsync();
			_logger.LogInformation("Registration successful for event ID {EventId}, Email: {Email}", registration.EventId, registration.Email);

			return registration;
		}

	}
}
