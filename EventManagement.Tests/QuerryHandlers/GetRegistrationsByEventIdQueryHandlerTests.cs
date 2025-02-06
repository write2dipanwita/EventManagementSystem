using AutoMapper;
using EventManagementSystem.Application.RegistrationManagement.DTOs;
using EventManagementSystem.Application.RegistrationManagement.Queries.GetRegistrationsByEventId;
using EventManagementSystem.Core.RegistrationManagement.Entities;
using EventManagementSystem.Core.RegistrationManagement.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.Tests.QuerryHandlers
{
	public class GetRegistrationsByEventIdQueryHandlerTests
	{
		private readonly Mock<IRegistrationRepository> _registrationRepositoryMock;
		private readonly Mock<IMapper> _mapperMock;
		private readonly GetRegistrationsByEventIdQueryHandler _handler;

		public GetRegistrationsByEventIdQueryHandlerTests()
		{
			_registrationRepositoryMock = new Mock<IRegistrationRepository>();
			_mapperMock = new Mock<IMapper>();
			_handler = new GetRegistrationsByEventIdQueryHandler(_registrationRepositoryMock.Object, _mapperMock.Object);
		}

		[Fact]
		public async Task Handle_Should_Return_Registrations_When_EventId_Is_Valid()
		{
			// Arrange
			var eventId = 1;
			var registrations = new List<Registration>
		{
			new Registration(eventId, "John Doe", "john@example.com", "1234567890"),
			new Registration(eventId, "Jane Smith", "jane@example.com", "9876543210")
		};

			var registrationDTOs = new List<RegistrationDTO>
		{
			new RegistrationDTO { Name = "John Doe", Email = "john@example.com", Phone = "1234567890" },
			new RegistrationDTO { Name = "Jane Smith", Email = "jane@example.com", Phone = "9876543210" }
		};

			_registrationRepositoryMock
				.Setup(repo => repo.GetRegistrationsByEventIdAsync(eventId))
				.ReturnsAsync(registrations);

			_mapperMock
				.Setup(mapper => mapper.Map<IEnumerable<RegistrationDTO>>(registrations))
				.Returns(registrationDTOs);

			var query = new GetRegistrationsByEventIdQuery(eventId);

			// Act
			var result = await _handler.Handle(query, CancellationToken.None);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(2, result.Count());
			Assert.Equal("John Doe", result.First().Name);
			_registrationRepositoryMock.Verify(repo => repo.GetRegistrationsByEventIdAsync(eventId), Times.Once);
			_mapperMock.Verify(mapper => mapper.Map<IEnumerable<RegistrationDTO>>(registrations), Times.Once);
		}

		[Fact]
		public async Task Handle_Should_Return_Empty_When_No_Registrations_Found()
		{
			// Arrange
			var eventId = 2;
			var registrations = new List<Registration>();

			_registrationRepositoryMock
				.Setup(repo => repo.GetRegistrationsByEventIdAsync(eventId))
				.ReturnsAsync(registrations);

			_mapperMock
				.Setup(mapper => mapper.Map<IEnumerable<RegistrationDTO>>(registrations))
				.Returns(new List<RegistrationDTO>());

			var query = new GetRegistrationsByEventIdQuery(eventId);

			// Act
			var result = await _handler.Handle(query, CancellationToken.None);

			// Assert
			Assert.NotNull(result);
			Assert.Empty(result);
			_registrationRepositoryMock.Verify(repo => repo.GetRegistrationsByEventIdAsync(eventId), Times.Once);
		}
	}
}
