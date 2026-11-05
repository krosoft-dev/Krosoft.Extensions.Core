namespace Krosoft.Extensions.Core.Models.Dto;

/// <summary>
/// Compteur KPI : total et éléments créés depuis le début du mois.
/// </summary>
public record KpiDto
{
    /// <summary>
    /// Nombre total d'éléments.
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// Nombre d'éléments créés depuis le premier jour du mois en cours.
    /// </summary>
    public int Increase { get; set; }
}
