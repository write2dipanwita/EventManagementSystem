
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EventManagementSystem.Application.EventManagement.DTOs
{
	public class EventDTO
	{
		
		public int Id { get; set; }
		[Required(ErrorMessage = "Event name is required.")]
		public string Name { get; set; }
		public string Description { get; set; }
		[Required(ErrorMessage = "Event location is required.")]
		public string Location { get; set; }
		[Required(ErrorMessage = "Start time is required.")]
		[JsonPropertyName("start_time")]
		public DateTime StartTime { get; set; }
		[Required(ErrorMessage = "End time is required.")]
		[JsonPropertyName("end_time")]
		public DateTime EndTime { get; set; }
	}
}
