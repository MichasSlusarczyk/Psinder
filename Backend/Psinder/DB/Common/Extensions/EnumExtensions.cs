using System.ComponentModel;

namespace Psinder.DB.Common.Extensions;

public static class EnumExtensions
{
    public static string ValueToString(this Enum value)
    {
        return value.ToString();
    }

    public static string GetDescription(this Enum value)
    {
        var description = value.ToString();
        var attribute = value.GetAttribute<DescriptionAttribute>();
        if (attribute != null)
        {
            description = attribute.Description;
        }

        return description;
    }

    public static T GetAttribute<T>(this Enum value) where T : Attribute
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = ((T[])field.GetCustomAttributes(typeof(T), false)).FirstOrDefault();
        return attribute;
    }
}
