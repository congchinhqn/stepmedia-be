﻿using System;
using System.Linq;
using System.Reflection;

namespace Metatrade.Core.Extensions
{
    public static class AttributeExtension
    {
        public static bool HasAttribute<T>(this ICustomAttributeProvider self)
            where T : Attribute
        {
            return self.GetCustomAttributes(true).Any(o => o is T);
        }

        public static bool HasAttribute<T>(this object self)
            where T : Attribute
        {
            return self != null && self.GetType().HasAttribute<T>();
        }
    }
}