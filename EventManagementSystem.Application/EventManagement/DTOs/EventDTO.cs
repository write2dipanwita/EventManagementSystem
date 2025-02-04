using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.Application.EventManagement.DTOs
{
	public class EventDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Location { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
	}
}