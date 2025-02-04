using EventManagementSystem.Application.DTOs;
using EventManagementSystem.Application.EventManagement.Commands.CreateEvent;
using EventManagementSystem.Application.EventManagement.Queries.GetALLEvents;
using EventManagementSystem.Core.EventManagement.Entities;
using EventManagementSystem.WebAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using System.Security.Claims;

namespace API.Tests
{
	public class AuthControllerTests
	{
		private readonly Mock<UserManager<IdentityUser>> _mockUserManager;
		private readonly Mock<SignInManager<IdentityUser>> _mockSignInManager;
		private readonly Mock<IConfiguration> _mockConfiguration;
		private readonly AuthController _controller;

		public AuthControllerTests()
		{
			_mockUserManager = new Mock<UserManager<IdentityUser>>(
				new Mock<IUserStore<IdentityUser>>().Object, null, null, null, null, null, null, null, null);

			_mockSignInManager = new Mock<SignInManager<IdentityUser>>(
				_mockUserManager.Object,
				new Mock<IHttpContextAccessor>().Object,
				new Mock<IUserClaimsPrincipalFactory<IdentityUser>>().Object,
				null, null, null, null);

			_mockConfiguration = new Mock<IConfiguration>();

			
			_mockConfiguration.Setup(config => config["Jwt:Key"]).Returns("MySuperSecretKeyWithMoreThan32Characters!!");
			_mockConfiguration.Setup(config => config["Jwt:Issuer"]).Returns("https://localhost:7138/");
			_mockConfiguration.Setup(config => config["Jwt:Audience"]).Returns("https://localhost:7138/");

			_controller = new AuthController(_mockUserManager.Object, _mockSignInManager.Object, _mockConfiguration.Object);
		}

		
		[Fact]
		public async Task Login_ReturnsOk_WhenCredentialsAreValid()
		{
			
			var userDto = new UserDTO { Username = "testuser", Password = "Password123!" };
			var identityUser = new IdentityUser { UserName = userDto.Username, Id = "12345" };

			_mockUserManager.Setup(u => u.FindByNameAsync(userDto.Username))
							.ReturnsAsync(identityUser);

			_mockSignInManager.Setup(s => s.PasswordSignInAsync(identityUser, userDto.Password, false, false))
							  .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

			
			var result = await _controller.Login(userDto);

			
			var okResult = Assert.IsType<OkObjectResult>(result);
			var tokenProperty = okResult.Value.GetType().GetProperty("Token");
			Assert.NotNull(tokenProperty);
		}

		
		[Fact]
		public async Task Login_ReturnsUnauthorized_WhenCredentialsAreInvalid()
		{
			
			var userDto = new UserDTO { Username = "testuser", Password = "WrongPassword" };
			var identityUser = new IdentityUser { UserName = userDto.Username };

			_mockUserManager.Setup(u => u.FindByNameAsync(userDto.Username))
							.ReturnsAsync(identityUser);

			_mockSignInManager.Setup(s => s.PasswordSignInAsync(identityUser, userDto.Password, false, false))
							  .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

			
			var result = await _controller.Login(userDto);

			
			var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
			Assert.Equal("Invalid username or password", unauthorizedResult.Value);
		}

		
		[Fact]
		public async Task Login_ReturnsUnauthorized_WhenUserDoesNotExist()
		{
			
			var userDto = new UserDTO { Username = "nonexistentuser", Password = "Password123!" };

			_mockUserManager.Setup(u => u.FindByNameAsync(userDto.Username))
							.ReturnsAsync((IdentityUser)null);

			
			var result = await _controller.Login(userDto);

			
			var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
			Assert.Equal("Invalid username or password", unauthorizedResult.Value);
		}

		
		[Fact]
		public async Task Login_ReturnsBadRequest_WhenUsernameOrPasswordIsEmpty()
		{
			var userDto = new UserDTO { Username = "", Password = "" };
			var result = await _controller.Login(userDto);
			Assert.IsType<BadRequestObjectResult>(result);
		}
	}
}