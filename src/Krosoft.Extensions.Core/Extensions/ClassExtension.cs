namespace Krosoft.Extensions.Core.Extensions;

public static class ClassExtension
{
    /// <summary>
    /// Permet de remplacer les valeurs des propriétés (de type string) qui sont null par vide.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="myObject"></param>
    public static void ReplacePropertiesNullByEmpty<T>(this T? myObject) where T : class
    {
        if (myObject != null)
        {
            var properties = myObject.GetType().GetProperties();
            foreach (var info in properties)
            {
                if (info.PropertyType == typeof(string) && info.GetValue(myObject, null) == null)
                {
                    info.SetValue(myObject, string.Empty, null);
                }
            }
        }
    }
}