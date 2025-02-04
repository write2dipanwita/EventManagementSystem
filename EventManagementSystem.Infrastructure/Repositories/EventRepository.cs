using EventManagementSystem.Application.Common.Interfaces;
using EventManagementSystem.Core.EventManagement.Entities;
using EventManagementSystem.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;


namespace EventManagementSystem.Infrastructure.Repositories
{
	public class EventRepository : IEventRepository
	{
		private readonly ApplicationDbContext _context;

		public EventRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<Event> AddAsync(Event newEvent)
		{
			_context.Events.Add(newEvent);
			await _context.SaveChangesAsync();
			return newEvent;
		}

		public async Task<IEnumerable<Event>> GetAllAsync()
		{
			return await _context.Events.ToListAsync();
		}

		
	}
}
