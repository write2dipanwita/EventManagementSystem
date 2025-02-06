using EventManagementSystem.Core.Common;
using System;

namespace EventManagementSystem.Core.EventManagement.Entities
{
	public class Event : BaseEntity
	{
		public string Name { get; private set; }
		public string Description { get; private set; }
		public string Location { get; private set; }
		public DateTime StartTime { get; private set; }
		public DateTime EndTime { get; private set; }
		public string CreatedBy { get; private set; }

		private Event() { }  // Required for EF Core

		public Event(string name, string description, string location, DateTime startTime, DateTime endTime, string createdBy)
		{
			Name = name;
			Description = description;
			Location = location;
			StartTime = startTime;
			EndTime = endTime;
			CreatedBy = createdBy;
		}
	

		
	
	}
}
