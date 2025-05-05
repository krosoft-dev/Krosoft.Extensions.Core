namespace Krosoft.Extensions.Core.Helpers;

public static class ObjectHelper
{
    public static object? GetPropertyValue<T>(object src, string propName)
    {
        var propertyInfo = typeof(T).GetProperty(propName);
        if (propertyInfo != null)
        {
            return propertyInfo.GetValue(src, null);
        }

        return null;
    }
}