
using EventManagementSystem.Application.IdentityManagement;
using EventManagementSystem.Infrastructure.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventManagementSystem.WebAPI.Controllers
{
	[Route("api/auth")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly JwtSettings _jwtSettings;
		private readonly ILogger<AuthController> _logger;
		public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IOptions<JwtSettings> jwtSettings, ILogger<AuthController> logger)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_jwtSettings = jwtSettings.Value;
			_logger = logger;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody, Required] LoginRequest model)
		{
			_logger.LogInformation("Login attempt for username: {Username}", model.Username);

			var user = await _userManager.FindByNameAsync(model.Username);
			if (user == null) return Unauthorized("Invalid username or password");

			if (await _userManager.IsLockedOutAsync(user))
				return Unauthorized("Your account is temporarily locked due to multiple failed login attempts. Please try again later.");

			var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
			if (!result.Succeeded)
			{
				await _userManager.AccessFailedAsync(user);
				return Unauthorized("Invalid username or password");
			}

			var token = await GenerateJwtToken(user);
			return Ok(new { Token = token });

		}

		private async Task<string> GenerateJwtToken(IdentityUser user)
		{
			string secretKey = _jwtSettings.Key ?? throw new ArgumentNullException("JWT Secret Key is missing in appsettings.json");

			var roles = await _userManager.GetRolesAsync(user);
			var claims = new List<Claim>
									{
										new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
										new Claim(ClaimTypes.NameIdentifier, user.Id ?? string.Empty)
									};
			claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpiryHours),
				Issuer = _jwtSettings.Issuer,
				Audience = _jwtSettings.Audience,
				SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
	

}
