namespace Krosoft.Extensions.Core.Interfaces;

/// <summary>
/// Service d'accès à la date et à l'heure courantes.
/// </summary>
public interface IDateTimeService
{
    /// <summary>
    /// Obtient la date et l'heure courantes.
    /// </summary>
    DateTimeOffset Now { get; }
}
