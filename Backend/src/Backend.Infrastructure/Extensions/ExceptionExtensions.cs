using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Extensions
{
    public static class ExceptionExtensions
    {
        // Repace exception word to empty string from Exception type
        public static string GetExceptionCode(this Exception exception)
            => exception.GetType().Name.Replace("Exception", string.Empty);


    }
}
