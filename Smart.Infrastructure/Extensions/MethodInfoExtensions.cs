﻿using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Smart.Infrastructure.Extensions
{
    public static class MethodInfoExtensions
    {
        public static bool HasAttribute<T>(this MethodInfo method)
        {
            return method.GetCustomAttributes(typeof(T), false).FirstOrDefault() is T;

        }

        public static bool IsAsync(this MethodInfo method)
        {
            return method.ReturnType == typeof(Task)
                || (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>));

        }
    }
}
