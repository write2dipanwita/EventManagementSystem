using EventManagementSystem.Application.EventManagement.Commands.CreateEvent;
using EventManagementSystem.Application.EventManagement.Queries.GetALLEvents;
using EventManagementSystem.Application.RegistrationManagement.Commands;
using EventManagementSystem.Application.RegistrationManagement.DTOs;
using EventManagementSystem.Application.RegistrationManagement.Queries.GetRegistrationsByEventId;
using EventManagementSystem.WebAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using System.Security.Claims;

namespace API.Tests
{
	public class RegistrationControllerTests
	{
		private readonly Mock<IMediator> _mockMediator;
		private readonly RegistrationController _controller;

		public RegistrationControllerTests()
		{
			_mockMediator = new Mock<IMediator>();
			_controller = new RegistrationController(_mockMediator.Object);
		}

		// ✅ Test: GetRegistrationsByEventId returns 200 OK with data
		[Fact]
		public async Task GetRegistrationsByEventId_ReturnsOk_WhenRegistrationsExist()
		{
			// Arrange
			int eventId = 1;
			var registrations = new List<RegistrationDTO>
		{
			new RegistrationDTO { Name = "John Doe", Email = "john@example.com" },
			new RegistrationDTO {  Name = "Jane Doe", Email = "jane@example.com" }
		};

			_mockMediator.Setup(m => m.Send(It.IsAny<GetRegistrationsByEventIdQuery>(), default))
						 .ReturnsAsync(registrations);

			// Act
			var result = await _controller.GetRegistrationsByEventId(eventId);

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result);
			var returnedRegistrations = Assert.IsType<List<RegistrationDTO>>(okResult.Value);
			Assert.Equal(2, returnedRegistrations.Count);
		}

		// ❌ Test: GetRegistrationsByEventId returns 404 Not Found when no registrations exist
		[Fact]
		public async Task GetRegistrationsByEventId_ReturnsNotFound_WhenNoRegistrationsExist()
		{
			// Arrange
			int eventId = 2;
			_mockMediator.Setup(m => m.Send(It.IsAny<GetRegistrationsByEventIdQuery>(), default))
						 .ReturnsAsync(new List<RegistrationDTO>());

			// Act
			var result = await _controller.GetRegistrationsByEventId(eventId);

			// Assert
			Assert.IsType<NotFoundObjectResult>(result);
		}

		// 🔴 Test: GetRegistrationsByEventId returns 500 Internal Server Error on exception
		[Fact]
		public async Task GetRegistrationsByEventId_ReturnsInternalServerError_OnException()
		{
			// Arrange
			int eventId = 3;
			_mockMediator.Setup(m => m.Send(It.IsAny<GetRegistrationsByEventIdQuery>(), default))
						 .ThrowsAsync(new Exception("Database error"));

			// Act
			var result = await _controller.GetRegistrationsByEventId(eventId);

			// Assert
			var objectResult = Assert.IsType<ObjectResult>(result);
			Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
		}

		// ✅ Test: RegisterForEvent returns 201 Created when successful
		[Fact]
		public async Task RegisterForEvent_ReturnsCreated_WhenSuccessful()
		{
			// Arrange
			int eventId = 1;
			var command = new RegisterForEventCommand(eventId, "Alice", "1234567890", "alice@example.com");
			var expectedResponse = new RegistrationDTO {  Name = "Alice", Email = "alice@example.com" };

			_mockMediator.Setup(m => m.Send(It.IsAny<RegisterForEventCommand>(), default))
						 .ReturnsAsync(expectedResponse);

			// Act
			var result = await _controller.RegisterForEvent(eventId, command);

			// Assert
			var createdResult = Assert.IsType<CreatedAtActionResult>(result);
			var returnedRegistration = Assert.IsType<RegistrationDTO>(createdResult.Value);
			Assert.Equal("Alice", returnedRegistration.Name);
		}

		// ❌ Test: RegisterForEvent returns 500 Internal Server Error on exception
		[Fact]
		public async Task RegisterForEvent_ReturnsInternalServerError_OnException()
		{
			// Arrange
			int eventId = 2;
			var command = new RegisterForEventCommand(eventId, "Bob", "0987654321", "bob@example.com");

			_mockMediator.Setup(m => m.Send(It.IsAny<RegisterForEventCommand>(), default))
						 .ThrowsAsync(new Exception("Database error"));

			// Act
			var result = await _controller.RegisterForEvent(eventId, command);

			// Assert
			var objectResult = Assert.IsType<ObjectResult>(result);
			Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
		}
	}
}