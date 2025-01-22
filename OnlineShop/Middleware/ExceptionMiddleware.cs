using Microsoft.AspNetCore.Http;
using System.Text.Json;
using OnlineShop.Exceptions;
using System.ComponentModel.DataAnnotations;


namespace OnlineShop.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = exception switch
            {
                EmailSendException => StatusCodes.Status500InternalServerError,
                SettingUpdateException => StatusCodes.Status500InternalServerError,
                ValidationException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            var response = new
            {
                error = exception.Message,
                details = exception.InnerException?.Message,
                stackTrace = context.Response.StatusCode == StatusCodes.Status500InternalServerError
                    ? exception.StackTrace 
                    : null
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

    }

}