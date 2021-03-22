using Backend.Core.Exceptions;
using Backend.Infrastructure.Exceptions;
using Backend.Infrastructure.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Framework
{
    public class ExceptionToResponseMapper : IExceptionToResponseMapper
    {
        // using ConcurrentDictionary is thread safe compared to dictionary, using dictionary could lead to lose some data
        private static readonly ConcurrentDictionary<Type, string> Codes = new ConcurrentDictionary<Type, string>();

        // switch expression
        // return ExceptionResponse with response: {code: exception type, reason: error message } and 
        // statusCode: BadRequest when exception was recognized and InternalServerError when exception was not recognized
        public ExceptionResponse Map(Exception exception)
            => exception switch
            {
                CoreException ex => new ExceptionResponse(new { code = GetCode(ex), reason = ex.Message },
                    HttpStatusCode.BadRequest),

                InfrastructureException ex => new ExceptionResponse(new { code = GetCode(ex), reason = ex.Message },
                    HttpStatusCode.BadRequest),

                // default 
                _ => new ExceptionResponse(new { code = "error", reason = "There was an error." },
                    HttpStatusCode.InternalServerError)
            };

        // Get exception code
        private static string GetCode(Exception exception)
        {
            // get Exception Type for example: InvalidEmailException
            var type = exception.GetType();
            // if exception types in in dictionary return code 
            if (Codes.TryGetValue(type, out var code))
            {
                return code;
            }

            // Get exception code, so for example InvalidEmailException will be converted to InvalidEmail
            var exceptionCode = exception.GetExceptionCode();
            // Adding exception type to with exception code to the dictionary 
            Codes.TryAdd(type, exceptionCode);

            return exceptionCode;
        }
    }
}
