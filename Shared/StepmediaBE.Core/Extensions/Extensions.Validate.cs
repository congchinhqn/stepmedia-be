using System;
using System.Collections.Generic;
using System.Linq;

namespace Metatrade.Core.Extensions
{
    public static partial class MiscExtensions
    {
        public static void NotNull(this object argument, string argumentName)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        public static void NotNull(this object argument, string argumentName, string message)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName, message);
            }
        }

        public static void NotNullOrEmpty(this string argument, string argumentName)
        {
            if (string.IsNullOrEmpty(argument))
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        public static void NotNullOrEmpty(this string argument, string argumentName, string message)
        {
            if (string.IsNullOrEmpty(argument))
            {
                throw new ArgumentNullException(argumentName, message);
            }
        }

        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);
        }

        public static bool IsEmpty(this Guid value)
        {
            return value == Guid.Empty;
        }

        public static bool IsEmpty(this Guid? value)
        {
            if (value == null)
                return true;
            return value == Guid.Empty;
        }

        public static bool IsEmpty<T>(this IEnumerable<T> value)
        {
            if (value == null)
                return true;
            return !value.Any();
        }
    }
}