using AutoMapper;
using EventManagementSystem.Application.EventManagement.Commands.CreateEvent;
using EventManagementSystem.Application.RegistrationManagement.Commands;
using EventManagementSystem.Application.RegistrationManagement.DTOs;
using EventManagementSystem.Core.RegistrationManagement.Entities;
using EventManagementSystem.Core.RegistrationManagement.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.Tests.CommandHandlers
{
	public class RegisterForEventCommandHandlerTests
	{
		private readonly Mock<IRegistrationRepository> _registrationRepositoryMock;
		private readonly Mock<IMapper> _mapperMock;
		private readonly RegisterForEventCommandHandler _handler;
		private readonly Mock<ILogger<RegisterForEventCommandHandler>> _loggerMock;

		public RegisterForEventCommandHandlerTests()
		{
			_registrationRepositoryMock = new Mock<IRegistrationRepository>();
			_mapperMock = new Mock<IMapper>();
			_loggerMock = new Mock<ILogger<RegisterForEventCommandHandler>>();
			_handler = new RegisterForEventCommandHandler(_registrationRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
		}

		[Fact]
		public async Task Handle_Should_Register_User_Successfully()
		{
			// Arrange
			var command = new RegisterForEventCommand
			{
				EventId = 1,
				Name = "John Doe",
				PhoneNumber = "1234567890",
				Email = "john.doe@example.com"
			};

			var registrationEntity = new Registration(command.EventId, command.Name, command.Email, command.PhoneNumber);

			var registrationDTO = new RegistrationDTO
			{
				Name = registrationEntity.Name,
				Email = registrationEntity.Email,
				Phone = registrationEntity.Phone
			};

			_registrationRepositoryMock
				.Setup(repo => repo.AddAsync(It.IsAny<Registration>()))
				.ReturnsAsync(registrationEntity);

			_mapperMock
				.Setup(mapper => mapper.Map<RegistrationDTO>(registrationEntity))
				.Returns(registrationDTO);

			// Act
			var result = await _handler.Handle(command, CancellationToken.None);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(command.Name, result.Name);
			Assert.Equal(command.Email, result.Email);
			_registrationRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Registration>()), Times.Once);
			_mapperMock.Verify(mapper => mapper.Map<RegistrationDTO>(registrationEntity), Times.Once);
		}

		
	}
}
