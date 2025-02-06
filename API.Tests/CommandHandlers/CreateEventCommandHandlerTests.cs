using AutoMapper;
using Castle.Core.Logging;
using EventManagementSystem.Application.Common.Interfaces;
using EventManagementSystem.Application.EventManagement.Commands.CreateEvent;
using EventManagementSystem.Application.EventManagement.DTOs;
using EventManagementSystem.Application.RegistrationManagement.Commands;
using EventManagementSystem.Core.EventManagement.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.Tests.CommandHandlers
{
	public class CreateEventCommandHandlerTests
	{
		private readonly Mock<IEventRepository> _eventRepositoryMock;
		private readonly Mock<IMapper> _mapperMock;
		private readonly CreateEventCommandHandler _handler;
		private readonly Mock<ILogger<CreateEventCommandHandler>> _loggerMock;
		private readonly Mock<IHttpContextAccessor> _httpContextAccessor;


		public CreateEventCommandHandlerTests()
		{
			_eventRepositoryMock = new Mock<IEventRepository>();
			_mapperMock = new Mock<IMapper>();
			_loggerMock =  new Mock<ILogger<CreateEventCommandHandler>>();
			_httpContextAccessor = new Mock<IHttpContextAccessor>();
			_handler = new CreateEventCommandHandler(_eventRepositoryMock.Object, _mapperMock.Object, _httpContextAccessor.Object, _loggerMock.Object);

		}

		[Fact]
		public async Task Handle_Should_Create_Event_Successfully()
		{
			// Arrange
			var command = new CreateEventCommand
			{
				Name = "Event Name",
				Description = "Event Description",
				Location = "Event Location",
				StartTime = DateTime.UtcNow.AddHours(1),
				EndTime = DateTime.UtcNow.AddHours(3)
			};

			var eventEntity = new Event(command.Name, command.Description, command.Location, command.StartTime, command.EndTime, "Admin");

			var eventDTO = new EventDTO
			{
				Name = eventEntity.Name,
				Description = eventEntity.Description,
				Location = eventEntity.Location,
				StartTime = eventEntity.StartTime,
				EndTime = eventEntity.EndTime
			};

			_eventRepositoryMock
				.Setup(repo => repo.AddAsync(It.IsAny<Event>()))
				.ReturnsAsync(eventEntity);

			_mapperMock
				.Setup(mapper => mapper.Map<EventDTO>(eventEntity))
				.Returns(eventDTO);

			// Act
			var result = await _handler.Handle(command, CancellationToken.None);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(command.Name, result.Name);
			Assert.Equal(command.Description, result.Description);
			_eventRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Event>()), Times.Once);
			_mapperMock.Verify(mapper => mapper.Map<EventDTO>(eventEntity), Times.Once);
		}

		[Fact]
		public async Task Handle_Should_Throw_Exception_If_Event_Already_Exists()
		{
			// Arrange
			var command = new CreateEventCommand
			{
				Name = "Duplicate Event",
				Description = "Event Description",
				Location = "Event Location",
				StartTime = DateTime.UtcNow.AddHours(1),
				EndTime = DateTime.UtcNow.AddHours(3)
			};

			_eventRepositoryMock
				.Setup(repo => repo.EventExistsAsync(command.Name, command.StartTime, command.Location))
				.ReturnsAsync(true); // Simulating event already exists

			var handler = new CreateEventCommandHandler(_eventRepositoryMock.Object, _mapperMock.Object, _httpContextAccessor.Object, _loggerMock.Object);

			// Act & Assert
			await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));

			_eventRepositoryMock.Verify(repo => repo.EventExistsAsync(command.Name, command.StartTime, command.Location), Times.Once);
			_eventRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Event>()), Times.Never);
		}
	}
}
