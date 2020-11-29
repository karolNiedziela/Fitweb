using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Api.Framework
{
    public static class Extensions
    {
        public static IApplicationBuilder UseMyExceptionHandler(this IApplicationBuilder builder)
            => builder.UseMiddleware(typeof(MyExceptionHandlerMiddleware));
    }
}
