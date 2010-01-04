using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MongoDB.Framework
{
    public static class ReflectionExtensions
    {
        public static bool Closes(this Type type, Type openGenericType)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == openGenericType;
        }

        public static bool Overrides(this Type type, string methodName, params Type[] parameterTypes)
        {
            try
            {
                MethodInfo method = !type.IsInterface
                                        ? type.GetMethod(methodName, parameterTypes)
                                        : GetMethodFromInterface(type, methodName, parameterTypes);
                if (method == null)
                    return false;
                else
                {
                    // make sure that the DeclaringType is not System.Object - if that is the
                    // declaring type then there is no override.
                    return !method.DeclaringType.Equals(typeof(object));
                }
            }
            catch (AmbiguousMatchException)
            {
                // an ambigious match means that there is an override and it
                // can't determine which one to use.
                return true;
            }
        }

        private static MethodInfo GetMethodFromInterface(System.Type type, string methodName, System.Type[] parameterTypes)
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;
            if (type == null)
                return null;

            MethodInfo method = type.GetMethod(methodName, flags, null, parameterTypes, null);
            if (method == null)
            {
                System.Type[] interfaces = type.GetInterfaces();
                foreach (var @interface in interfaces)
                {
                    method = GetMethodFromInterface(@interface, methodName, parameterTypes);
                    if (method != null)
                        return method;
                }
            }
            return method;
        }
    }
}