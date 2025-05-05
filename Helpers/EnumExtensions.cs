using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace ITBaza.Helpers
{
    public static class EnumExtensions
    {
        public static string GetEnumDisplayName(this Enum value)
        {
            var attr = value.GetType()
                .GetMember(value.ToString())
                .First()
                .GetCustomAttribute<EnumMemberAttribute>();

            return attr?.Value ?? value.ToString();
        }
    }
}
