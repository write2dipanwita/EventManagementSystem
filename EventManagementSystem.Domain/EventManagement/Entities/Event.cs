using EventManagementSystem.Core.Common;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Event name is required.");
			if (startTime >= endTime) throw new ArgumentException("Start time must be before end time.");
			if (string.IsNullOrEmpty(createdBy)) throw new ArgumentException("CreatedBy is required.");

			Name = name;
			Description = description;
			Location = location;
			StartTime = startTime;
			EndTime = endTime;
			CreatedBy = createdBy;
		}
	}
}
