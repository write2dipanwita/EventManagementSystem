
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EventManagementSystem.Application.EventManagement.DTOs
{
	public class EventDTO
	{
		
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Location { get; set; }
		[JsonPropertyName("start_time")]
		public DateTime StartTime { get; set; }
		[JsonPropertyName("end_time")]
		public DateTime EndTime { get; set; }
	}
}
