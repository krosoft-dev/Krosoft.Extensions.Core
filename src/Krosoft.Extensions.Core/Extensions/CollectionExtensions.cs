namespace Krosoft.Extensions.Core.Extensions;

/// <summary>
/// Méthodes d'extensions pour les classes implémentant l'interface <see cref="ICollection{T}" />.
/// </summary>
public static class CollectionExtensions
{
    public static void AddRange<T>(this ICollection<T> list, IEnumerable<T> collection)
    {
        foreach (var item in collection)
        {
            list.Add(item);
        }
    }
}