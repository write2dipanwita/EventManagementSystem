using EventManagementSystem.Application.Common.Behaviors;
using EventManagementSystem.Application.EventManagement.Commands.CreateEvent;
using EventManagementSystem.Application.RegistrationManagement.Queries.GetRegistrationsByEventId;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
namespace EventManagementSystem.Application
{
	public static class ApplicationServiceRegistration
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
			services.AddAutoMapper(Assembly.GetExecutingAssembly());

			services.AddControllers()
		.AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(typeof(RegisterForEventCommandValidator).Assembly));
			services.AddControllers()
		.AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(typeof(GetRegistrationsByEventIdQueryValidator).Assembly));

			services.AddControllers()
		.AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(typeof(CreateEventCommandValidator).Assembly));
			services.AddControllers()
		.AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(typeof(RegisterForEventCommandValidator).Assembly));


			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));

			return services;
		}
	}
}
