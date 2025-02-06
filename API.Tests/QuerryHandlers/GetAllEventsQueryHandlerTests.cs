using AutoMapper;
using EventManagementSystem.Application.Common.Interfaces;
using EventManagementSystem.Application.EventManagement.DTOs;
using EventManagementSystem.Application.EventManagement.Queries.GetALLEvents;
using EventManagementSystem.Core.EventManagement.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.Tests.QuerryHandlers
{
	public class GetAllEventsQueryHandlerTests
	{
		private readonly Mock<IEventRepository> _eventRepositoryMock;
		private readonly Mock<IMapper> _mapperMock;
		private readonly GetAllEventsQueryHandler _handler;

		public GetAllEventsQueryHandlerTests()
		{
			_eventRepositoryMock = new Mock<IEventRepository>();
			_mapperMock = new Mock<IMapper>();
			_handler = new GetAllEventsQueryHandler(_eventRepositoryMock.Object, _mapperMock.Object);
		}

		[Fact]
		public async Task Handle_Should_Return_All_Events()
		{
			// Arrange
			var events = new List<Event>
		{
			new Event("Event 1", "Description 1", "Location 1", DateTime.UtcNow, DateTime.UtcNow.AddHours(2), "Admin"),
			new Event("Event 2", "Description 2", "Location 2", DateTime.UtcNow, DateTime.UtcNow.AddHours(3), "Admin")
		};

			var eventDTOs = new List<EventDTO>
		{
			new EventDTO { Name = "Event 1", Description = "Description 1", Location = "Location 1", StartTime = DateTime.UtcNow, EndTime = DateTime.UtcNow.AddHours(2) },
			new EventDTO { Name = "Event 2", Description = "Description 2", Location = "Location 2", StartTime = DateTime.UtcNow, EndTime = DateTime.UtcNow.AddHours(3) }
		};

			_eventRepositoryMock
				.Setup(repo => repo.GetAllAsync())
				.ReturnsAsync(events);

			_mapperMock
				.Setup(mapper => mapper.Map<IEnumerable<EventDTO>>(events))
				.Returns(eventDTOs);

			var query = new GetAllEventsQuery();

			// Act
			var result = await _handler.Handle(query, CancellationToken.None);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(2, result.Count());
			Assert.Equal("Event 1", result.First().Name);
			_eventRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
			_mapperMock.Verify(mapper => mapper.Map<IEnumerable<EventDTO>>(events), Times.Once);
		}

		[Fact]
		public async Task Handle_Should_Return_Empty_List_When_No_Events_Are_Found()
		{
			// Arrange
			var events = new List<Event>();

			_eventRepositoryMock
				.Setup(repo => repo.GetAllAsync())
				.ReturnsAsync(events);

			_mapperMock
				.Setup(mapper => mapper.Map<IEnumerable<EventDTO>>(events))
				.Returns(new List<EventDTO>());

			var query = new GetAllEventsQuery();

			// Act
			var result = await _handler.Handle(query, CancellationToken.None);

			// Assert
			Assert.NotNull(result);
			Assert.Empty(result);
			_eventRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
		}
	}
}
