using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;

public class EventsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
	private readonly HttpClient _client;

	public EventsControllerTests(WebApplicationFactory<Program> factory)
	{
		_client = factory.CreateClient();
	}

	[Fact]
	public async Task GetEvents_ReturnsSuccess()
	{
		// Act
		var response = await _client.GetAsync("/api/events");

		// Assert
		response.EnsureSuccessStatusCode();
	}
}
