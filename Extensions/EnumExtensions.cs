using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace System
{
    public static class EnumExtensions
    {
        public static string GetEnumDisplayName(this Enum enumValue) =>
            enumValue.GetType()?
                .GetMember(enumValue.ToString())?
                .First()
                .GetCustomAttribute<DisplayAttribute>()?
                .Name;
    }
}
