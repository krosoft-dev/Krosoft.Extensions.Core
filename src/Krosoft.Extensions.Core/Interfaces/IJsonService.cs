namespace Krosoft.Extensions.Core.Interfaces;

/// <summary>
/// Service de sérialisation et de désérialisation JSON.
/// </summary>
public interface IJsonService
{
    /// <summary>
    /// Désérialise une chaîne JSON vers une instance du type demandé.
    /// </summary>
    /// <typeparam name="T">Le type cible de la désérialisation.</typeparam>
    /// <param name="jsonString">La chaîne JSON à désérialiser.</param>
    /// <returns>L'instance désérialisée, ou <c>null</c> si la désérialisation ne produit aucun objet.</returns>
    T? Deserialize<T>(string jsonString);

    /// <summary>
    /// Sérialise un objet en chaîne JSON.
    /// </summary>
    /// <typeparam name="T">Le type de l'objet à sérialiser.</typeparam>
    /// <param name="obj">L'objet à sérialiser.</param>
    /// <returns>La représentation JSON de l'objet.</returns>
    string Serialize<T>(T obj);
}
