using System.Reflection;
using System.Xml.Serialization;

namespace Krosoft.Extensions.Core.Helpers;

public class EnumHelper
{
    public static T ParseEnumOrDefault<T>(string? value, T defaultValue) where T : struct
    {
        // Si c'est un enum simple
        if (Enum.TryParse<T>(value, out var enumValue))
        {
            return enumValue;
        }

        return defaultValue;
    }

    public static IEnumerable<T> GetValues<T>()
    {
        if (!typeof(T).IsEnum)
        {
            throw new InvalidOperationException("Type must be enumeration type.");
        }

        return GetValuesImpl<T>();
    }

    private static IEnumerable<T> GetValuesImpl<T>()
    {
        return typeof(T).GetFields()
                        .Where(field => field.IsLiteral && !string.IsNullOrEmpty(field.Name))
                        .Select(field => (T)field.GetValue(null)!);
    }

    public static string? GetXmlEnumValue(Enum value)
    {
        var fi = value.GetType().GetField(value.ToString());

        if (fi != null)
        {
            var attr = (XmlEnumAttribute?)Attribute.GetCustomAttribute(fi, typeof(XmlEnumAttribute));
            if (attr != null)
            {
                return attr!.Name;
            }
        }

        return null; // fallback
    }

    public static T ParseXmlEnumValue<T>(string value) where T : struct, Enum
    {
        var type = typeof(T);
        foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            var attribute = field.GetCustomAttribute<XmlEnumAttribute>();
            if (attribute?.Name == value)
            {
                return (T)Enum.Parse(typeof(T), field.Name);
            }
        }

        throw new ArgumentException($"No enum member with XmlEnum value '{value}' found");
    }
}