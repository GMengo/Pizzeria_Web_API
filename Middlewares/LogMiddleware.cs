using System.Net;
using pizzeria_web_api.Repositories;

// LogMiddleware.cs
// SFIDA 5: Completa il middleware
namespace CalculatorWebApi.Middlewares
{
    public class LogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICustomLogger _logger;

        public LogMiddleware(RequestDelegate next, ICustomLogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.WriteRequest(context);

            await _next(context);

            _logger.WriteResponse(context);
        }
    }
}
