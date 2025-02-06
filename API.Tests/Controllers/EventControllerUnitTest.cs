using EventManagementSystem.Application.EventManagement.Commands.CreateEvent;
using EventManagementSystem.Application.EventManagement.DTOs;
using EventManagementSystem.Core.EventManagement.Entities;
using EventManagementSystem.WebAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using System.Security.Claims;

namespace API.Tests.Controllers
{
	public class EventControllerTests
	{
		private readonly Mock<IMediator> _mockMediator;
		private readonly Mock<ILogger<EventController>> _mockLogger;
		private readonly EventController _controller;

		public EventControllerTests()
		{
			_mockMediator = new Mock<IMediator>();
			_mockLogger = new Mock<ILogger<EventController>>();
			_controller = new EventController(_mockMediator.Object, _mockLogger.Object);
		}

		// ✅ Test: GetAllEvents() - Should Return 200 OK with Events
		[Fact]
		public async Task GetAllEvents_ReturnsOk_WhenEventsExist()
		{
			// Arrange
			var events = new List<EventDTO>
			{
				new EventDTO { Id = 1, Name = "Tech Meetup", Description = "Networking Event" },
				new EventDTO { Id = 2, Name = "AI Conference", Description = "AI & ML Talks" }
			};

			//_mockMediator.Setup(m => m.Send(It.IsAny<GetAllEventsQuery>(), default))
					//	 .ReturnsAsync(events);

			// Act
			var result = await _controller.GetAllEvents();

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var returnedEvents = Assert.IsType<List<EventDTO>>(okResult.Value);
			Assert.Equal(2, returnedEvents.Count);
		}

		// ❌ Test: GetAllEvents() - Should Return 404 Not Found When No Events Exist
		[Fact]
		public async Task GetAllEvents_ReturnsNotFound_WhenNoEventsExist()
		{
			// Arrange
			//_mockMediator.Setup(m => m.Send(It.IsAny<GetAllEventsQuery>(), default))
				//		 .ReturnsAsync(new List<EventDTO>());

			// Act
			var result = await _controller.GetAllEvents();

			// Assert
			Assert.IsType<NotFoundObjectResult>(result.Result);
		}

		// 🔴 Test: GetAllEvents() - Should Return 500 Internal Server Error on Exception
		[Fact]
		public async Task GetAllEvents_ReturnsInternalServerError_OnException()
		{
			// Arrange
			//_mockMediator.Setup(m => m.Send(It.IsAny<GetAllEventsQuery>(), default))
				//		 .ThrowsAsync(new Exception("Database error"));

			// Act
			var result = await _controller.GetAllEvents();

			// Assert
			var objectResult = Assert.IsType<ObjectResult>(result.Result);
			Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
		}

		// ✅ Test: CreateEvent() - Should Return 201 Created on Success
		[Fact]
		public async Task CreateEvent_ReturnsCreated_WhenEventIsValid()
		{
			// Arrange
			//var command = new CreateEventCommand { Name = "Tech Meetup", Description = "Networking Event" };
			var createdEvent = new EventDTO { Id = 1, Name = "Tech Meetup", Description = "Networking Event" };

			_mockMediator.Setup(m => m.Send(It.IsAny<CreateEventCommand>(), default))
						 .ReturnsAsync(createdEvent);

			// Simulate authenticated user
			var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
			{
				new Claim(ClaimTypes.NameIdentifier, "12345")
			}, "mock"));

			_controller.ControllerContext = new ControllerContext
			{
				HttpContext = new DefaultHttpContext { User = user }
			};

			// Act
			var result = await _controller.CreateEvent(null);

			// Assert
			var createdResult = Assert.IsType<CreatedAtActionResult>(result);
			var returnedEvent = Assert.IsType<EventDTO>(createdResult.Value);
			Assert.Equal("Tech Meetup", returnedEvent.Name);
		}

		// ❌ Test: CreateEvent() - Should Return 401 Unauthorized If User Is Not Logged In
		[Fact]
		public async Task CreateEvent_ReturnsUnauthorized_WhenUserNotAuthenticated()
		{
			// Arrange
			//var command = new CreateEventCommand { Name = "Tech Meetup", Description = "Networking Event" };

			// No user context set (unauthenticated)
			_controller.ControllerContext = new ControllerContext
			{
				HttpContext = new DefaultHttpContext()
			};

			// Act
			var result = await _controller.CreateEvent(null);

			// Assert
			var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
			Assert.Equal("User not found.", unauthorizedResult.Value);
		}
	}
}