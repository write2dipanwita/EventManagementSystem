using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Moq;
using EventManagementSystem.WebAPI.Controllers;
using EventManagementSystem.Infrastructure.Configuration;
using EventManagementSystem.Application.DTOs;

public class AuthControllerTests
{
	private readonly Mock<UserManager<IdentityUser>> _mockUserManager;
	private readonly Mock<SignInManager<IdentityUser>> _mockSignInManager;
	private readonly Mock<IOptions<JwtSettings>> _mockJwtSettings;
	private readonly Mock<ILogger<AuthController>> _mockLogger;
	private readonly AuthController _controller;

	public AuthControllerTests()
	{
		_mockUserManager = new Mock<UserManager<IdentityUser>>(
			new Mock<IUserStore<IdentityUser>>().Object,
			null, null, null, null, null, null, null, null);

		_mockSignInManager = new Mock<SignInManager<IdentityUser>>(
			_mockUserManager.Object,
			new Mock<IHttpContextAccessor>().Object,
			new Mock<IUserClaimsPrincipalFactory<IdentityUser>>().Object,
			null, null, null, null);

		
		_mockJwtSettings = new Mock<IOptions<JwtSettings>>();
		_mockJwtSettings.Setup(settings => settings.Value).Returns(new JwtSettings
		{
			Key = "MySuperSecretKeyWithMoreThan32Characters!!",
			Issuer = "https://localhost:7138/",
			Audience = "https://localhost:7138/",
			ExpiryHours = 2
		});	
		_mockLogger = new Mock<ILogger<AuthController>>();		
		_controller = new AuthController(
			_mockUserManager.Object,
			_mockSignInManager.Object,
			_mockJwtSettings.Object,
			_mockLogger.Object
		);
	}

	
	[Fact]
	public async Task Login_ReturnsUnauthorized_WhenUserNotFound()
	{
		
		var loginRequest = new UserDTO { Username = "nonexistentuser", Password = "wrongpassword" };
		_mockUserManager.Setup(um => um.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((IdentityUser)null);

		
		var result = await _controller.Login(loginRequest);
		Assert.IsType<UnauthorizedObjectResult>(result);
	}
}
