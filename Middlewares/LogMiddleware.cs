using System.Net;
using pizzeria_web_api.Services;

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
            DateTime startTime = DateTime.Now; 
            _logger.WriteRequest(context);

            await _next(context);
            DateTime endTime = DateTime.Now;
            int duration = (int)(endTime - startTime).TotalMilliseconds;
            // aggiunta durata della chiamata, tempo della risposta - tempo della richiesta

            _logger.WriteResponse(context, duration);
        }
    }
}
