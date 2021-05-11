namespace StickersOnMap.Core.Infrastructure
{
    using System;
    using System.Net.Mime;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    public class MiddlewareException
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MiddlewareException> _logger;

        public MiddlewareException(RequestDelegate next, ILogger<MiddlewareException> logger) =>
            (_next, _logger) = (next, logger);

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                await WriteErrorResponse(httpContext);
            }
        }

        private Task WriteErrorResponse (HttpContext context)
        {
            const int status = StatusCodes.Status500InternalServerError;
            context.Response.StatusCode = status;
            context.Response.ContentType = MediaTypeNames.Application.Json;

            const string message = "Произошла ошибка во время выполнения запроса";
            return context.Response.WriteAsync($"\"message\": \"{message}\", \"status\": {status}", Encoding.UTF8);
        }
    }
}