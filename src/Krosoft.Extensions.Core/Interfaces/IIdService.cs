namespace Krosoft.Extensions.Core.Interfaces;

/// <summary>
/// Service de génération d'identifiants uniques.
/// </summary>
public interface IIdService
{
    /// <summary>
    /// Génère un nouvel identifiant <see cref="Guid" />.
    /// </summary>
    /// <returns>Un nouveau <see cref="Guid" />.</returns>
    Guid NewGuid();

    /// <summary>
    /// Génère un nouvel identifiant <see cref="Guid" /> en version 7 (ordonnable dans le temps).
    /// </summary>
    /// <returns>Un nouveau <see cref="Guid" /> en version 7.</returns>
    Guid NewGuidVersion7();
}
