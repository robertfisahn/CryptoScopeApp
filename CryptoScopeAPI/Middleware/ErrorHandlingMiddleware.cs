using System.Net;

using Newtonsoft.Json;

using CryptoScopeAPI.Exceptions;

namespace CryptoScopeAPI.Middleware
{
    public class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> _logger) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (NotFoundException notFoundException)
            {
                await HandleExceptionAsync(context, HttpStatusCode.NotFound, notFoundException.Message);
            }
            catch (HttpRequestException httpEx)
            {
                var code = httpEx.StatusCode ?? HttpStatusCode.BadGateway;
                await HandleExceptionAsync(context, code, httpEx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception.");
                await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, "Something went wrong.");
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message)
        {
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";
            var result = JsonConvert.SerializeObject(new { error = message });
            return context.Response.WriteAsync(result);
        }
    }
}