namespace Krosoft.Extensions.Core.Helpers;

public class EnumHelper
{
    internal static T ParseEnumOrDefault<T>(string? value, T defaultValue) where T : struct
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
}