using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using System.Text;
using EventManagementSystem.Application.Common.Interfaces;
using EventManagementSystem.Infrastructure.Repositories;
using EventManagementSystem.Core.RegistrationManagement.Repositories;
using EventManagementSystem.Infrastructure.Configuration;
using EventManagementSystem.Infrastructure.Persistance;

namespace EventManagementSystem.Infrastructure
{
	public static class InfrastructureServiceRegistration
	{
		public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
		{
			
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

			
			services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
			var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>();

			if (string.IsNullOrEmpty(jwtSettings?.Key))
				throw new ArgumentNullException("JWT Secret Key is missing in appsettings.json");

			var key = Encoding.UTF8.GetBytes(jwtSettings.Key);

		
			services.AddHttpContextAccessor();
			services.AddIdentity<IdentityUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.RequireHttpsMetadata = true; 
				options.SaveToken = true;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidIssuer = jwtSettings.Issuer,
					ValidAudience = jwtSettings.Audience,
					ClockSkew = TimeSpan.Zero 
				};
			});

			
			services.ConfigureApplicationCookie(options =>
			{
				options.Cookie.HttpOnly = true;
				options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
				options.LoginPath = "/api/auth/login";
				options.AccessDeniedPath = "/api/auth/access-denied";
				options.SlidingExpiration = true;
				options.Cookie.SecurePolicy = CookieSecurePolicy.Always; 
			});

			
			services.AddAuthorization();
			services.AddScoped<IEventRepository, EventRepository>();
			services.AddScoped<IRegistrationRepository, RegistrationRepository>();

			return services;
		}
	}
}
