using System.Dynamic;

namespace Krosoft.Extensions.Core.Extensions;

public static class ExpandoObjectExtensions
{
    public static string? GetString(this ExpandoObject expandoObject, string key)
    {
        IDictionary<string, object?> dictionary = expandoObject;

        if (dictionary.TryGetValue(key, out var value))
        {
            return value?.ToString();
        }

        return null;
    }
}