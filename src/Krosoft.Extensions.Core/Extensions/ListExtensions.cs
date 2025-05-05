using Krosoft.Extensions.Core.Tools;

namespace Krosoft.Extensions.Core.Extensions;

/// <summary>
/// Méthodes d'extensions pour les classes implémentant l'interface <see cref="List{T}" />.
/// </summary>
public static class ListExtensions
{
    public static void AddRange<T>(this IList<T> list, IEnumerable<T> collection, bool checkContains = false)
    {
        if (checkContains)
        {
            foreach (var item in collection)
            {
                if (!list.Contains(item))
                {
                    list.Add(item);
                }
            }
        }
        else
        {
            foreach (var item in collection)
            {
                list.Add(item);
            }
        }
    }

    public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(this List<TValue> source,
                                                                       Func<TValue, TKey> valueSelector,
                                                                       bool useDistinct) where TKey : notnull
    {
        Guard.IsNotNull(nameof(source), source);

        return source.AsEnumerable().ToDictionary(valueSelector, useDistinct);
    }

    public static IDictionary<string, TValue> ToDictionary<TValue>(this List<TValue> source,
                                                                   Func<TValue, string> selector,
                                                                   Func<string, string> modificator,
                                                                   bool useDistinct)
    {
        Guard.IsNotNull(nameof(source), source);

        return source.AsEnumerable().ToDictionary(selector, modificator, useDistinct);
    }

    public static IReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>(this List<TValue> source,
                                                                                       Func<TValue, TKey> valueSelector,
                                                                                       bool useDistinct) where TKey : notnull
    {
        Guard.IsNotNull(nameof(source), source);

        return source.AsEnumerable().ToReadOnlyDictionary(valueSelector, useDistinct);
    }
}