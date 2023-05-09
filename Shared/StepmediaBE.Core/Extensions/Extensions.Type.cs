﻿using System;

 namespace Metatrade.Core.Extensions
{
    public static partial class MiscExtensions
    {
        public static bool IsNullable(this Type type)
        {
            type.NotNull(nameof(type));
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}
