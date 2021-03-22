using Backend.Infrastructure.Services.Logger;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Framework
{
    public class ErrorHandlerMiddleware : IMiddleware
    {
        private readonly IExceptionToResponseMapper _exceptionToResponseMapper;
        private readonly ILoggerManager _logger;

        public ErrorHandlerMiddleware(IExceptionToResponseMapper exceptionToResponseMapper,
            ILoggerManager logger)
        {
            _exceptionToResponseMapper = exceptionToResponseMapper;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // If was any error, handle it properly with custom exceptions
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                await HandleErrorAsync(context, exception);
            }
        }

        // write the error in format { code: "", reason: "" }
        private async Task HandleErrorAsync(HttpContext context, Exception exception)
        {
            var exceptionResponse = _exceptionToResponseMapper.Map(exception);
            // assign status code depending on the exceptionResponse object
            // if status code was null, assign 500 (IntervalServerError)
            // else statusCode from property 
            context.Response.StatusCode = (int)(exceptionResponse?.StatusCode ?? HttpStatusCode.InternalServerError);
            var response = exceptionResponse?.Response;
            if (response is null)
            {
                await context.Response.WriteAsync(string.Empty);
            }

            context.Response.ContentType = "application/json";
            // serializes to string 
            var payload = JsonConvert.SerializeObject(exceptionResponse.Response);
            // write to the response body
            await context.Response.WriteAsync(payload);
        }
    }
}
