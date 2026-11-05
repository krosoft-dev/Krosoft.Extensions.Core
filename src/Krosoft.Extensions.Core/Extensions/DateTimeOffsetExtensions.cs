namespace Krosoft.Extensions.Core.Extensions;

/// <summary>
/// Extensions sur <see cref="DateTimeOffset" />.
/// </summary>
public static class DateTimeOffsetExtensions
{
    /// <summary>
    /// Renvoie le premier jour du mois de <paramref name="date" /> à minuit, heure locale du serveur, exprimé en UTC.
    /// </summary>
    /// <remarks>
    /// L'année et le mois sont lus dans le calendrier de <paramref name="date" /> (son propre décalage), puis minuit du
    /// 1er est interprété dans le fuseau local du serveur (<see cref="TimeZoneInfo.Local" />). Le résultat est converti
    /// en UTC car Npgsql n'accepte que des valeurs UTC pour une colonne <c>timestamptz</c>.
    /// </remarks>
    /// <param name="date">Date de référence.</param>
    /// <returns>Début du mois, en UTC (décalage nul).</returns>
    public static DateTimeOffset StartOfMonth(this DateTimeOffset date)
        => new DateTimeOffset(new DateTime(date.Year, date.Month, 1)).ToUniversalTime();
}
