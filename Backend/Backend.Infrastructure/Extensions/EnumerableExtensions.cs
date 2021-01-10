using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> AddRangeLazily<T>(this ICollection<T>col, IEnumerable<T> values)
        {
            foreach (T i in values)
            {
                yield return i;
                col.Add(i);
            }
        }
    }
}
