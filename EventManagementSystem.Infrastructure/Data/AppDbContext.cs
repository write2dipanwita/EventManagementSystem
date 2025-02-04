
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
			base.OnModelCreating(modelBuilder); // ✅ Ensures Identity Tables are Configured Properly

			// 🔹 Ensure `CreatedBy` is Required in Events
			modelBuilder.Entity<Event>()
			   .Property(e => e.CreatedBy)
			   .IsRequired();


			// 🔹 Define Foreign Key for `Registrations` with `Cascade Delete`
			modelBuilder.Entity<Registration>()
				.HasOne(r => r.Event)
				.WithMany()
				.HasForeignKey(r => r.EventId)
				.OnDelete(DeleteBehavior.Cascade); // ✅ Ensures cascading delete

			// 🔹 Apply Any Additional Configurations from the Assembly
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
		}
	}
	
}
