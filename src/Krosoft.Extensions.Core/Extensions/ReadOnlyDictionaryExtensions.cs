using System.Collections.ObjectModel;

namespace Krosoft.Extensions.Core.Extensions;

/// <summary>
/// Méthodes d'extensions pour les classes implémentant l'interface <see cref="IReadOnlyDictionary{TKey,TValue}" />.
/// </summary>
public static class ReadOnlyDictionaryExtensions
{
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
    public static TValue? GetValueOrDefault<TKey, TValue>(this ReadOnlyDictionary<TKey, TValue> dictionary,
                                                          TKey key,
                                                          TValue? defaultValue = default) where TKey : notnull =>
        dictionary.TryGetValue(key, out var value)
            ? value
            : defaultValue;
}