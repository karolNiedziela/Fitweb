using Backend.Infrastructure.Exceptions;
using Backend.Infrastructure.Services.Logger;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Backend.Api.Framework
{
    public class MyExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerManager _logger;

        public MyExceptionHandlerMiddleware(RequestDelegate next, ILoggerManager logger)
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
            catch(Exception exception)
            {
                _logger.LogError($"Something went wrong {exception}.");
                await HandleExceptionAsync(context, exception);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var errorCode = "error";
            var statusCode = (int)HttpStatusCode.BadRequest;
            var exceptionType = exception.GetType();
            switch (exception)
            {
                case Exception e when exceptionType == typeof(UnauthorizedAccessException):
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    break;

                case ServiceException e when exceptionType == typeof(ServiceException):
                    statusCode = (int)HttpStatusCode.BadRequest;
                    errorCode = e.Code;
                    break;

                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }


            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            var response = new { statusCode = statusCode, errorCode = errorCode, message = exception.Message };
            var payload = JsonConvert.SerializeObject(response);
            
            return context.Response.WriteAsync(payload);
        }
    }
}
