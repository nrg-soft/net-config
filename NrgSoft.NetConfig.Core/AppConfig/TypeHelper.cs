using System;

namespace NrgSoft.NetConfig.Core.AppConfig
{
    public static class TypeHelper
    {
        public static bool IsPrimitiveSupportedType(Type propertyType)
        {
            return propertyType == typeof(int) ||
                   propertyType == typeof(long) ||
                   propertyType == typeof(double) ||
                   propertyType == typeof(decimal) ||
                   propertyType == typeof(char) ||
                   propertyType == typeof(bool) ||
                   propertyType == typeof(TimeSpan) ||
                   propertyType == typeof(DateTime) ||
                   propertyType == typeof(string) ||
                   propertyType.IsPrimitive;
        }
    }
}
