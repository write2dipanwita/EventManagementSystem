using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace EventManagementSystem.API.Middleware
{
	public class ExceptionHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionHandlingMiddleware> _logger;

		public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context); 
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unhandled exception occurred: {Message}", ex.Message);
				await HandleExceptionAsync(context, ex);
			}
		}

		private static Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			context.Response.ContentType = "application/json";

			var response = exception switch
			{
				ValidationException => new ErrorResponse("Validation error", exception.Message, (int)HttpStatusCode.BadRequest),
				UnauthorizedAccessException => new ErrorResponse("Unauthorized", exception.Message, (int)HttpStatusCode.Unauthorized),
				KeyNotFoundException => new ErrorResponse("Not Found", exception.Message, (int)HttpStatusCode.NotFound),
				ApplicationException => new ErrorResponse("Application error", exception.Message, (int)HttpStatusCode.BadRequest),
				_ => new ErrorResponse("Internal Server Error",exception.Message, (int)HttpStatusCode.InternalServerError)
			};

			context.Response.StatusCode = response.StatusCode;
			return context.Response.WriteAsync(JsonSerializer.Serialize(response));
		}

		private record ErrorResponse(string Error, string Message, int StatusCode);
	}
	
}
