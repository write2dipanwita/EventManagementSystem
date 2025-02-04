using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using EventManagementSystem.Infrastructure.Persistance;
using EventManagementSystem.Application.Common.Interfaces;
using EventManagementSystem.Infrastructure.Repositories;
using EventManagementSystem.Core.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using EventManagementSystem.Core.RegistrationManagement.Repositories;

namespace EventManagementSystem.Infrastructure
{
	public static class InfrastructureServiceRegistration
	{
		public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
			services.AddHttpContextAccessor();
			services.AddIdentity<IdentityUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]); // ✅ Fixed key retrieval
			services.ConfigureApplicationCookie(options =>
			{
				options.Cookie.HttpOnly = true;
				options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
				options.LoginPath = "/api/auth/login"; // Redirects to login
				options.AccessDeniedPath = "/api/auth/access-denied"; // Deny unauthorized access
				options.SlidingExpiration = true;
				options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
			});
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.RequireHttpsMetadata = false;
				options.SaveToken = true;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidIssuer = configuration["Jwt:Issuer"],
					ValidAudience = configuration["Jwt:Audience"],
					ClockSkew = TimeSpan.Zero // Ensures immediate token expiration
				};
			});

			services.AddAuthorization(); // ✅ Only kept one instance

			services.AddScoped<IEventRepository, EventRepository>();
			services.AddScoped<IRegistrationRepository, RegistrationRepository>();


			return services;
		}
	}
}
