using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework
{
    public static class ReflectionExtensions
    {
        public static bool Closes(this Type type, Type openGenericType)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == openGenericType;
        }
    }
}