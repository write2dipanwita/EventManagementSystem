using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace EventManagementSystem.Application.Common.Behaviors
{
	public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	{
		private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;

		public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
		{
			_logger = logger;
		}

		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			var stopwatch = Stopwatch.StartNew();
			var response = await next();
			stopwatch.Stop();

			var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
			if (elapsedMilliseconds > 1000) 
			{
				_logger.LogWarning(" Slow CQRS operation detected: {RequestName} took {ElapsedMilliseconds}ms",
					typeof(TRequest).Name, elapsedMilliseconds);
			}

			return response;
		}
	}
}
