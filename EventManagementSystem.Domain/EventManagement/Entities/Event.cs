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
		

		private Event() { }

		public Event(string name, string description, string location, DateTime startTime, DateTime endTime, string createdBy)
		{
			ValidateEvent(name, description, location, startTime, endTime, createdBy);

			Name = name;
			Description = description;
			Location = location;
			StartTime = startTime;
			EndTime = endTime;
			CreatedBy = createdBy;
		}

		public void UpdateEvent(string name, string description, string location, DateTime startTime, DateTime endTime)
		{
			ValidateEvent(name, description, location, startTime, endTime, CreatedBy);

			Name = name;
			Description = description;
			Location = location;
			StartTime = startTime;
			EndTime = endTime;
		}

		
		private void ValidateEvent(string name, string description, string location, DateTime startTime, DateTime endTime, string createdBy)
		{
			if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Event name is required.");
			if (startTime >= endTime) throw new ArgumentException("Start time must be before end time.");
			if (string.IsNullOrEmpty(createdBy)) throw new ArgumentException("CreatedBy is required.");
			if (string.IsNullOrWhiteSpace(location)) throw new ArgumentException("Event location is required.");
		}
	}
}
