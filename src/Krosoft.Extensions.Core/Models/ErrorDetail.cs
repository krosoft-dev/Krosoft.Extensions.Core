namespace Krosoft.Extensions.Core.Models;

/// <summary>
/// Détail d'une erreur.
/// </summary>
public record ErrorDetail(string TypeName, string PropertyName, IEnumerable<string> Errors);