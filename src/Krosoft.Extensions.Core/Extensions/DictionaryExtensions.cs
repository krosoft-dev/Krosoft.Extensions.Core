using Krosoft.Extensions.Core.Tools;

namespace Krosoft.Extensions.Core.Extensions;

/// <summary>
/// Méthodes d'extensions pour les classes implémentant l'interface <see cref="IDictionary{TKey,TValue}" />.
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// Ajoute une nouvelle entrée au dictionnaire si la clé n'est pas déjà présente.
    /// Remplace la valeur associée à la clé si celle-ci existe déjà.
    /// </summary>
    /// <typeparam name="TKey">Type de la clé.</typeparam>
    /// <typeparam name="TValue">Type de la valeur.</typeparam>
    /// <param name="source">Dictionnaire source.</param>
    /// <param name="key">Clé.</param>
    /// <param name="value">Valeur.</param>
    public static void AddOrReplace<TKey, TValue>(this IDictionary<TKey, TValue>? source, TKey key, TValue value)
    {
        Guard.IsNotNull(nameof(source), source);

        if (source!.ContainsKey(key))
        {
            source[key] = value;
        }
        else
        {
            source.Add(key, value);
        }
    }

    public static void AddOrUpdate<T>(this IDictionary<T, decimal> dico, T key, decimal solde)
    {
        Guard.IsNotNull(nameof(dico), dico);
        if (dico.ContainsKey(key))
        {
            dico[key] += solde;
        }
        else
        {
            dico.Add(key, solde);
        }
    }

    /// <summary>
    /// Ajout une valeur dans une liste stockée dans un dictionnaire.
    /// </summary>
    /// <typeparam name="TKey">Type de la clé du dictionnaire.</typeparam>
    /// <typeparam name="TValue">Type de la valeur des éléments de la liste.</typeparam>
    /// <param name="dictionary">Dictionnaire interrogé.</param>
    /// <param name="key">Clé.</param>
    /// <param name="val">Valeur à ajouter.</param>
    /// <param name="ignoreDoublon">Si vrai, on n'ajoute la valeur uniquement si elle n'est pas présente.</param>
    public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, List<TValue>> dictionary, TKey key, TValue val, bool ignoreDoublon = false)
    {
        Guard.IsNotNull(nameof(dictionary), dictionary);
        if (dictionary.ContainsKey(key))
        {
            if (ignoreDoublon)
            {
                if (!dictionary[key].Contains(val))
                {
                    dictionary[key].Add(val);
                }
            }
            else
            {
                dictionary[key].Add(val);
            }
        }
        else
        {
            dictionary.Add(key, new List<TValue> { val });
        }
    }

    /// <summary>
    /// Fusionne un dictionnaire dans un autre dictionnaire du même type.
    /// </summary>
    /// <typeparam name="TKey">Type des clés.</typeparam>
    /// <typeparam name="TValue">Type des valeurs.</typeparam>
    /// <param name="target">Dictionnaire à modifier.</param>
    /// <param name="source">Dictionnaire à fusionner.</param>
    /// <param name="replaceExistingKeys">
    /// <c>true</c> pour remplacer les valeurs des clés existantes, <c>false</c> pour lancer
    /// une exception sur les clés dupliqués.
    /// </param>
    public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> target, IDictionary<TKey, TValue> source, bool replaceExistingKeys = false)
    {
        Guard.IsNotNull(nameof(target), target);
        Guard.IsNotNull(nameof(source), source);

        if (replaceExistingKeys)
        {
            foreach (var element in source)
            {
                target.AddOrReplace(element.Key, element.Value);
            }
        }
        else
        {
            // Une exception sera lancée sir la clé existe déjà
            foreach (var element in source)
            {
                target.Add(element.Key, element.Value);
            }
        }
    }

    /// <summary>
    /// Cherche une valeur associée à une clé dans un dictionnaire.
    /// Si non trouvée, renvoie la valeur par défaut.
    /// </summary>
    /// <typeparam name="TKey">Type de la clé.</typeparam>
    /// <typeparam name="TValue">Type de la valeur.</typeparam>
    /// <param name="dictionary">Dictionnaire interrogé.</param>
    /// <param name="key">Clé.</param>
    /// <param name="defaultValue">Valeur par défaut.</param>
    /// <returns>Valeur trouvée ou valeur par défaut.</returns>
    public static TValue? GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue? defaultValue = default) =>
        dictionary.TryGetValue(key, out var value)
            ? value
            : defaultValue;
}