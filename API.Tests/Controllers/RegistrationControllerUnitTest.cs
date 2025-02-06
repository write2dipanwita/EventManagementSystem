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
			}
	}
}
