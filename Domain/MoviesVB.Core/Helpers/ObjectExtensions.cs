using System;
using System.Collections.Generic;
using System.Linq;

namespace MoviesVB.Core.Helpers
{
    public static class ObjectExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return (source == null || !source.Any());
        }

        public static bool IsNullOrEmpty(this Guid? source)
        {
            return (source == null || source == Guid.Empty);
        }

        public static bool IsNull<T>(this T source)
        {
            return source == null;
        }

        public static bool IsNotNull<T>(this T source)
        {
            return source != null;
        }

        public static bool IsDefault<T>(this T source)
            where T : struct
        {
            return source.Equals(default(T));
        }

        public static bool IsEmpty(this Guid source)
        {
            return source == Guid.Empty;
        }

        public static bool IsAnyOf<T>(this T source, params T[] values)
        {
            if (values.IsNullOrEmpty())
            {
                return false;
            }

            return values.Contains(source);
        }
    }
}
