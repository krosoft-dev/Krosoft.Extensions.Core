using System.Collections;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Reflection;
using Krosoft.Extensions.Core.Helpers;
using Krosoft.Extensions.Core.Tools;

namespace Krosoft.Extensions.Core.Extensions;

/// <summary>
/// Méthodes d'extensions pour les classes implémentant l'interface <see cref="IEnumerable{T}" />.
/// </summary>
public static class EnumerableExtensions
{
#if !NET7_0_OR_GREATER
    /// <summary>
    /// Effectue un distinct sur la collection sur le critère choisi.
    /// </summary>
    /// <typeparam name="TSource">Le type d'objet de la collection à parcourir.</typeparam>
    /// <typeparam name="TKey">Le type du critère.</typeparam>
    /// <param name="source">Collection à parcourir.</param>
    /// <param name="keySelector">Critère de filtre.</param>
    /// <returns>La collection filtrée.</returns>
    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        var seenKeys = new HashSet<TKey>();
        foreach (var element in source)
        {
            if (seenKeys.Add(keySelector(element)))
            {
                yield return element;
            }
        }
    }

    /// <summary>
    /// Méthode d'extension pour générer un hashSet à partir d'un IEnumerable
    /// </summary>
    /// <typeparam name="T">Type d'objet</typeparam>
    /// <param name="source">Liste à convertir</param>
    /// <returns>Un HashSet</returns>
    public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source) => new HashSet<T>(source);
#endif

    /// <summary>
    /// Extrait depuis une collection toutes les valeurs à partir du nom d'une propriété et renvoie ces valeurs formatées
    /// s'ils s'agient de DateTime.
    /// </summary>
    /// <typeparam name="T">le type de l'objet contenu dans la liste</typeparam>
    /// <param name="enumerable">la collection à parcourir</param>
    /// <param name="propertyName">le nom de la propriété</param>
    /// <param name="format">le format du DateTime à utiliser</param>
    /// <returns>une collection de dates formatées en string avec le format en paramètre</returns>
    public static IEnumerable ExtractingDateTimeInStrings<T>(this IEnumerable<T> enumerable, string propertyName, string format = "")
    {
        var type = typeof(T);
        var getter = typeof(T).GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        if (getter == null)
        {
            throw new InvalidOperationException($"L'objet de type '{type}' n'a pas de propriété ayant le nom '{propertyName}'.");
        }

        foreach (var o in enumerable)
        {
            var value = getter.GetValue(o, null);
            if (value != null)
            {
                yield return ((DateTime)value).ToString(format);
            }
        }
    }

    /// <summary>
    /// Extrait depuis une collection toutes les valeurs et renvoie ces valeurs en string.
    /// </summary>
    /// <param name="enumerable">la collection à parcourir</param>
    /// <returns>une collection de dates formatées en string avec le format en paramètre</returns>
    public static IEnumerable ExtractingInStrings(this IEnumerable enumerable)
    {
        foreach (var value in enumerable)
        {
            yield return value.ToString();
        }
    }

    /// <summary>
    /// Applique un arrondi à toutes les éléments du tableau.
    /// </summary>
    /// <param name="src">Tableau d'éléménts.</param>
    /// <param name="nombreDecimales">Nombre de décimales après la virgule.</param>
    public static decimal[] Round(this decimal[] src, int nombreDecimales = 2)
    {
        for (var i = 0; i < src.Length; i++)
        {
            src[i] = Math.Round(src[i], nombreDecimales);
        }

        return src;
    }

    /// <summary>
    /// Applique une action à l'ensemble des éléments de la liste.
    /// </summary>
    /// <typeparam name="T">Type des éléments.</typeparam>
    /// <param name="src">Liste d'éléments.</param>
    /// <param name="action">Action à effectuer sur les éléments.</param>
    public static void ForEach<T>(this IEnumerable<T> src, Action<T> action)
    {
        foreach (var item in src)
        {
            action(item);
        }
    }

    /// <summary>
    /// Permet de faire une énumération en récupérant l'index comme ceci :
    /// monenumerable.ForEach((monobjet, monindex) =>
    /// {
    /// //utilisation de monobjet et monindex
    /// });
    /// </summary>
    /// <typeparam name="T">Type du contenu de l'enumerable</typeparam>
    /// <param name="ie">Un objet de type IEnumerable à parcourir</param>
    /// <param name="action">Une fonction à effectuer sur le couple (objet,index)</param>
    public static void ForEach<T>(this IEnumerable<T> ie, Action<T, int> action)
    {
        var i = 0;
        foreach (var e in ie)
        {
            action(e, i++);
        }
    }

    /// <summary>
    /// Méthode d'extension pour générer un ConcurrentDictionary à partir d'une liste
    /// </summary>
    /// <typeparam name="TSource">Type de la liste</typeparam>
    /// <typeparam name="TKey">Type de la clé</typeparam>
    /// <typeparam name="TElement">Type de la valeur</typeparam>
    /// <param name="source">Liste source</param>
    /// <param name="keySelector">Key selector</param>
    /// <param name="elementSelector">Element selector</param>
    /// <returns>Un ConcurrentDictionary</returns>
    public static ConcurrentDictionary<TKey, TElement> ToConcurrentDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source,
                                                                                                       Func<TSource, TKey> keySelector,
                                                                                                       Func<TSource, TElement> elementSelector) where TKey : notnull =>
        new ConcurrentDictionary<TKey, TElement>(source.ToDictionary(keySelector, elementSelector));

    /// <summary>
    /// Execute un foreach en parallèle et en asynchrone.
    /// </summary>
    /// <typeparam name="T">Type des items à parcourir.</typeparam>
    /// <param name="source">Collection à parcourir.</param>
    /// <param name="partitionCount">Nombre de thread à éxécuter en parallèle.</param>
    /// <param name="func">Fonction à appliquer dans la boucle.</param>
    /// <returns>Tache asynchrone.</returns>
    public static Task ForEachAsync<T>(this IEnumerable<T> source, int partitionCount, Func<T, Task> func)
    {
        return Task.WhenAll(from partition in Partitioner.Create(source).GetPartitions(partitionCount)
                            select Task.Run(async delegate
                            {
                                using (partition)
                                {
                                    while (partition.MoveNext())
                                    {
                                        await func(partition.Current).ConfigureAwait(false);
                                    }
                                }
                            }));
    }

    /// <summary>
    /// Méthode d'extension pour générer un ConcurrentDictionary à partir d'une liste
    /// </summary>
    /// <param name="source"> Liste source </param>
    /// <param name="valueSelector"> Value selector. </param>
    /// <typeparam name="TKey">Type de la clé</typeparam>
    /// <typeparam name="TValue">Type de la valeur</typeparam>
    /// <returns> Un ConcurrentDictionary </returns>
    public static ConcurrentDictionary<TKey, TValue> ToConcurrentDictionary<TKey, TValue>(this IEnumerable<TValue> source,
                                                                                          Func<TValue, TKey> valueSelector) where TKey : notnull =>
        new ConcurrentDictionary<TKey, TValue>(source.ToDictionary(valueSelector));

    public static IReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>(this IEnumerable<TValue> source,
                                                                                       Func<TValue, TKey> valueSelector) where TKey : notnull =>
        new ReadOnlyDictionary<TKey, TValue>(source.ToDictionary(valueSelector));

    /// <summary>
    /// Découpe une collection en sous-collection de taille fixe.
    /// </summary>
    /// <typeparam name="T">Type des éléments de la collection.</typeparam>
    /// <param name="source">Collection à découper.</param>
    /// <param name="chunkSize">Taille max des sous-collections.</param>
    /// <returns>Collection de sous-collections à la taille demandée.</returns>
    public static IList<List<T>> ChunkBy<T>(this IEnumerable<T> source, int chunkSize)
    {
        Guard.IsNotNull(nameof(source), source);
        return source
               .Select((x, i) => new { Index = i, Value = x })
               .GroupBy(x => x.Index / chunkSize)
               .Select(x => x.Select(v => v.Value).ToList())
               .ToList();
    }

    /// <summary>
    /// Récupère une liste d'objet en utilisant une liste de clé et une fonction associée
    /// </summary>
    /// <param name="keys">Liste d'objet à rendre unique qui serviront pour la fonction</param>
    /// <param name="func">Fonction renvoyant la réponse à cette fonction</param>
    /// <typeparam name="TKey">Type de la clé</typeparam>
    /// <typeparam name="TSource">Type de la source</typeparam>
    /// <returns>Collection de sous-collections à la taille demandée.</returns>
    public static Task<IEnumerable<TSource>> ApplyWithAsync<TKey, TSource>(this IEnumerable<TKey> keys,
                                                                           Func<ISet<TKey>, Task<IEnumerable<TSource>>> func) =>
        ActionHelper.ApplyWithAsync(keys, func);

    public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<TValue> source,
                                                                       Func<TValue, TKey> valueSelector,
                                                                       bool useDistinct) where TKey : notnull
    {
        Guard.IsNotNull(nameof(source), source);

        var enumerable = source;

        if (useDistinct)
        {
            enumerable = enumerable.DistinctBy(valueSelector);
        }

        return enumerable.ToDictionary(valueSelector);
    }

    public static IReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>(this IEnumerable<TValue> source,
                                                                                       Func<TValue, TKey> valueSelector,
                                                                                       bool useDistinct) where TKey : notnull
    {
        Guard.IsNotNull(nameof(source), source);

        var enumerable = source;

        if (useDistinct)
        {
            enumerable = enumerable.DistinctBy(valueSelector);
        }

        return enumerable.ToReadOnlyDictionary(valueSelector);
    }

    public static IDictionary<string, TValue> ToDictionary<TValue>(this IEnumerable<TValue> source,
                                                                   Func<TValue, string>? selector,
                                                                   Func<string, string> modificator,
                                                                   bool useDistinct)
    {
        Guard.IsNotNull(nameof(source), source);
        Guard.IsNotNull(nameof(selector), source);

        return source.Where(x => selector?.Invoke(x) != null).ToDictionary(x => modificator(selector!(x)), useDistinct);
    }

    public static IReadOnlyDictionary<string, TValue> ToReadOnlyDictionary<TValue>(this IEnumerable<TValue> source,
                                                                                   Func<TValue, string>? selector,
                                                                                   Func<string, string> modificator,
                                                                                   bool useDistinct)
    {
        Guard.IsNotNull(nameof(source), source);
        Guard.IsNotNull(nameof(selector), source);

        return source.Where(x => selector?.Invoke(x) != null).ToReadOnlyDictionary(x => modificator(selector!(x)), useDistinct);
    }

    public static int MaxOrDefault<T>(this IEnumerable<T> enumeration, Func<T, int> selector)
    {
        var enumerable = enumeration.ToList();
        return enumerable.Any() ? enumerable.Max(selector) : default;
    }

#if !NET6_0_OR_GREATER
    /// <summary>
    /// Effectue un distinct sur la collection sur le critère choisi.
    /// </summary>
    /// <typeparam name="TSource">Le type d'objet de la collection à parcourir.</typeparam>
    /// <typeparam name="TKey">Le type du critère.</typeparam>
    /// <param name="source">Collection à parcourir.</param>
    /// <param name="keySelector">Critère de filtre.</param>
    /// <returns>La collection filtrée.</returns>
    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        Guard.IsNotNull(nameof(source), source);
        var seenKeys = new HashSet<TKey>();
        foreach (var element in source)
        {
            if (seenKeys.Add(keySelector(element)))
            {
                yield return element;
            }
        }
    }

#endif

    public static IDictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source,
                                                                                    Func<TSource, TKey> keySelector,
                                                                                    Func<TSource, TElement> elementSelector,
                                                                                    bool useDistinct) where TKey : notnull
    {
        Guard.IsNotNull(nameof(source), source);
        Guard.IsNotNull(nameof(keySelector), keySelector);
        Guard.IsNotNull(nameof(elementSelector), elementSelector);

        var enumerable = source;

        if (useDistinct)
        {
            enumerable = enumerable.DistinctBy(keySelector);
        }

        return enumerable.ToDictionary(keySelector, elementSelector);
    }
}