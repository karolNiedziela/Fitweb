using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.Extensions
{
    public static class DateTimeExtensions
    {
        public static long ToUnixTimeMilliseconds(this DateTime dateTime)
            => new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
    }
}
