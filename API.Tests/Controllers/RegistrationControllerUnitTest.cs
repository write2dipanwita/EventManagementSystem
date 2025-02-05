using EventManagementSystem.Application.RegistrationManagement.Commands;
using EventManagementSystem.Application.RegistrationManagement.DTOs;
using EventManagementSystem.Application.RegistrationManagement.Queries.GetRegistrationsByEventId;
using EventManagementSystem.WebAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using System;

namespace API.Tests
{
	public class RegistrationControllerTests
	{
		private readonly Mock<IMediator> _mockMediator;
		private readonly Mock<ILogger<RegistrationController>> _mockLogger;
		private readonly RegistrationController _controller;

		public RegistrationControllerTests()
		{
			_mockMediator = new Mock<IMediator>();
			_mockLogger = new Mock<ILogger<RegistrationController>>();

			_controller = new RegistrationController(_mockMediator.Object, _mockLogger.Object)
			{
				ControllerContext = new ControllerContext
				{
					HttpContext = new DefaultHttpContext() 
				}
			};
		}

		
		[Fact]
		public async Task GetRegistrationsByEventId_ReturnsOk_WhenRegistrationsExist()
		{
			
			int eventId = 1;
			var registrations = new List<RegistrationDTO>
			{
				new RegistrationDTO { Name = "John Doe", Email = "john@example.com" },
				new RegistrationDTO { Name = "Jane Doe", Email = "jane@example.com" }
			};

			_mockMediator.Setup(m => m.Send(It.IsAny<GetRegistrationsByEventIdQuery>(), default))
						 .ReturnsAsync(registrations);

			
			var result = await _controller.GetRegistrationsByEventId(eventId);

			
			var okResult = Assert.IsType<OkObjectResult>(result);
			var returnedRegistrations = Assert.IsType<List<RegistrationDTO>>(okResult.Value);
			Assert.Equal(2, returnedRegistrations.Count);

			_mockMediator.Verify(m => m.Send(It.IsAny<GetRegistrationsByEventIdQuery>(), default), Times.Once);
		}

		[Fact]
		public async Task GetRegistrationsByEventId_ReturnsNotFound_WhenNoRegistrationsExist()
		{
			
			int eventId = 2;
			_mockMediator.Setup(m => m.Send(It.IsAny<GetRegistrationsByEventIdQuery>(), default))
						 .ReturnsAsync(new List<RegistrationDTO>());

			
			var result = await _controller.GetRegistrationsByEventId(eventId);

			
			Assert.IsType<NotFoundObjectResult>(result);
			_mockMediator.Verify(m => m.Send(It.IsAny<GetRegistrationsByEventIdQuery>(), default), Times.Once);
		}

		[Fact]
		public async Task GetRegistrationsByEventId_ReturnsInternalServerError_OnException()
		{
			
			int eventId = 3;
			_mockMediator.Setup(m => m.Send(It.IsAny<GetRegistrationsByEventIdQuery>(), default))
						 .ThrowsAsync(new Exception("Database error"));

			
			var result = await _controller.GetRegistrationsByEventId(eventId);

			
			var objectResult = Assert.IsType<ObjectResult>(result);
			Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

			_mockLogger.Verify(log => log.LogError(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
		}

		
		[Fact]
		public async Task RegisterForEvent_ReturnsCreated_WhenSuccessful()
		{
			
			int eventId = 1;
			var command = new RegisterForEventCommand(eventId, "Alice", "1234567890", "alice@example.com");
			var expectedResponse = new RegistrationDTO { Name = "Alice", Email = "alice@example.com" };

			_mockMediator.Setup(m => m.Send(It.IsAny<RegisterForEventCommand>(), default))
						 .ReturnsAsync(expectedResponse);

			var result = await _controller.RegisterForEvent(eventId, command);

			
			var createdResult = Assert.IsType<CreatedAtActionResult>(result);
			var returnedRegistration = Assert.IsType<RegistrationDTO>(createdResult.Value);
			Assert.Equal("Alice", returnedRegistration.Name);

			_mockMediator.Verify(m => m.Send(It.IsAny<RegisterForEventCommand>(), default), Times.Once);
		}

		[Fact]
		public async Task RegisterForEvent_ReturnsBadRequest_WhenModelStateIsInvalid()
		{
			
			int eventId = 1;
			var command = new RegisterForEventCommand(eventId, "", "1234567890", "alice@example.com"); // Name is empty

			_controller.ModelState.AddModelError("Name", "Name is required");

			
			var result = await _controller.RegisterForEvent(eventId, command);

			
			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
			Assert.IsType<SerializableError>(badRequestResult.Value);

			_mockMediator.Verify(m => m.Send(It.IsAny<RegisterForEventCommand>(), default), Times.Never);
		}

		[Fact]
		public async Task RegisterForEvent_ReturnsInternalServerError_OnException()
		{
			
			int eventId = 2;
			var command = new RegisterForEventCommand(eventId, "Bob", "0987654321", "bob@example.com");

			_mockMediator.Setup(m => m.Send(It.IsAny<RegisterForEventCommand>(), default))
						 .ThrowsAsync(new Exception("Database error"));

			
			var result = await _controller.RegisterForEvent(eventId, command);

			
			var objectResult = Assert.IsType<ObjectResult>(result);
			Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

			_mockLogger.Verify(log => log.LogError(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
		}
	}
}
