using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace ApiApplication.Middleware
{
    public class ExecutionTrackingMiddleware : IMiddleware
    {
        private readonly ILogger<ExecutionTrackingMiddleware> _logger;
        private readonly Stopwatch _stopwatch;

        public ExecutionTrackingMiddleware(ILogger<ExecutionTrackingMiddleware> logger)
        {
            _logger = logger;
            _stopwatch = new Stopwatch();
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _stopwatch.Start();

            await next(context);

            _stopwatch.Stop();

            _logger.LogInformation("Request handled: {method} {path} in {duration}ms", context.Request.Method, context.Request.Path, _stopwatch.ElapsedMilliseconds);
        }
    }
}
