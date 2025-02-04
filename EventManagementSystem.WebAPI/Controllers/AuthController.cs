
using EventManagementSystem.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
		private readonly IConfiguration _configuration;

		public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_configuration = configuration;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] UserDTO model)
		{
			if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
				return BadRequest("Username and password are required.");

			var user = await _userManager.FindByNameAsync(model.Username);
			if (user == null) return Unauthorized("Invalid username or password");

			var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
			if (!result.Succeeded) return Unauthorized("Invalid username or password");

			var token = GenerateJwtToken(user);
			return Ok(new { Token = token });
		}

		private string GenerateJwtToken(IdentityUser user)
		{
			
			string secretKey = _configuration["Jwt:Key"];

			if (string.IsNullOrEmpty(secretKey))
			{
				throw new ArgumentNullException("JWT Secret Key is missing in appsettings.json");
			}
			var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				{
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim(ClaimTypes.Role, "Admin") // Only Admins can create events
            }),
				Expires = DateTime.UtcNow.AddHours(2),
				Issuer = _configuration["Jwt:Issuer"],   // 👈 Must match appsettings.json
				Audience = _configuration["Jwt:Audience"],
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
	

}
