using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static bool Empty(this string value)
            => string.IsNullOrEmpty(value);
    }
}
