
using EventManagementSystem.Core.EventManagement.Entities;
using EventManagementSystem.Core.RegistrationManagement.Entities;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.Infrastructure.Persistance
{
	public class ApplicationDbContext : IdentityDbContext<IdentityUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		public DbSet<Event> Events { get; set; }
		public DbSet<Registration> Registrations { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Event>()
			   .Property(e => e.CreatedBy)
			   .IsRequired();
			modelBuilder.Entity<Registration>()
			.Property(r => r.EventId)      
			.IsRequired();

			
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
		}
	}
	
}
