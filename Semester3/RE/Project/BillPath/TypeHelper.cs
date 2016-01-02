using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BillPath
{
    public static class TypeHelper
    {
        private static IEnumerable<Type> _primitiveTypes = new HashSet<Type>
        {
            typeof(object),

            typeof(byte),
            typeof(byte?),
            typeof(sbyte),
            typeof(sbyte?),

            typeof(short),
            typeof(short?),
            typeof(ushort),
            typeof(ushort?),

            typeof(int),
            typeof(int?),
            typeof(uint),
            typeof(uint?),

            typeof(long),
            typeof(long?),
            typeof(ulong),
            typeof(ulong?),

            typeof(float),
            typeof(float?),
            typeof(double),
            typeof(double?),
            typeof(decimal),
            typeof(decimal?),

            typeof(char),
            typeof(char?),
            typeof(string),

            typeof(DateTime),
            typeof(DateTime?),
            typeof(DateTimeOffset),
            typeof(DateTimeOffset?)
        };
        public static bool IsPrimitive(this Type type)
            => _primitiveTypes.Contains(type)
            || typeof(Enum).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo())
            || typeof(Delegate).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo());
    }
}