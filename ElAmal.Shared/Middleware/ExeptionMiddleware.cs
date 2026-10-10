using ElAmal.Shared.Errors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace ElAmal.Shared.Middleware
{
    public class ExeptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExeptionMiddleware> _logger;
        private readonly IWebHostEnvironment _environment;

        public ExeptionMiddleware(RequestDelegate next, ILogger<ExeptionMiddleware> logger, IWebHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
			try
			{ 
                await _next.Invoke(httpContext);
			}
			catch (Exception ex)
			{
                _logger.LogError(ex.Message);   

                httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError; // 500

                httpContext.Response.ContentType = "application/Json";

                var response = _environment.IsDevelopment() ? new ApiExceptionResponse((int)HttpStatusCode.InternalServerError,
                    ex.Message, ex.StackTrace!.ToString())
                    :
                    new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);

                var json = JsonSerializer.Serialize(response);


                await httpContext.Response.WriteAsync(json);

            }
        }
    }
}
