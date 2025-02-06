using EventManagementSystem.Application.Common.Interfaces;
using EventManagementSystem.Core.EventManagement.Entities;
using EventManagementSystem.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventManagementSystem.Infrastructure.Repositories
{
	public class EventRepository : IEventRepository
	{
		private readonly ApplicationDbContext _context;
		private readonly ILogger<EventRepository> _logger;

		public EventRepository(ApplicationDbContext context, ILogger<EventRepository> logger)
		{
			_context = context;
			_logger = logger;
		}
		
		public async Task<bool> EventExistsAsync(string name, DateTime startTime, string location)
		{
			return await _context.Events
				.AnyAsync(e => e.Name == name && e.StartTime == startTime && e.Location == location);
		}


		public async Task<Event> AddAsync(Event newEvent)
		{
			_logger.LogInformation("Adding new event: {EventName}", newEvent.Name);
			if (await EventExistsAsync(newEvent.Name, newEvent.StartTime, newEvent.Location))
			{
				throw new InvalidOperationException("An event with the same name, location, and start time already exists.");
			}

			_context.Events.Add(newEvent);
			await _context.SaveChangesAsync();

			_logger.LogInformation("Event {EventName} added successfully with ID {EventId}.", newEvent.Name, newEvent.Id);
			return newEvent;
		}

		
		public async Task<IEnumerable<Event>> GetAllAsync()
		{
			_logger.LogInformation("Fetching all events from the database.");

			var events = await _context.Events.AsNoTracking().ToListAsync();

			if (events.Count == 0)
			{
				_logger.LogWarning("No events found in the database.");
			}

			_logger.LogInformation("Retrieved {Count} events from the database.", events.Count);
			return events;
		}

		
	}
}
