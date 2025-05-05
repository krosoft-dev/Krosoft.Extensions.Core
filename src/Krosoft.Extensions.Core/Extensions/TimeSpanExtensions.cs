namespace Krosoft.Extensions.Core.Extensions;

/// <summary>
/// Méthodes d'extensions pour les TimeSpan.
/// </summary>
public static class TimeSpanExtensions
{
    private static IEnumerable<string> GetReadableStringElements(this TimeSpan span)
    {
        yield return GetString(span.Days, "j");
        yield return GetString(span.Hours, "h");
        yield return GetString(span.Minutes, "m");
        yield return GetString(span.Seconds, "s");
        yield return GetString(span.Milliseconds, "ms");
    }

    private static string GetString(int ms, string unite) => ms == 0 ? string.Empty : $"{ms}{unite}";

    /// <summary>
    /// Affichage en chaine de charactères du TimeSpan donné.
    /// </summary>
    /// <param name="span">TimeSpan à afficher.</param>
    public static string ToShortString(this TimeSpan span)
    {
        var shortString = string.Join(", ", span.GetReadableStringElements()
                                                .Where(str => !string.IsNullOrWhiteSpace(str)));

        if (string.IsNullOrWhiteSpace(shortString))
        {
            shortString = "0ms";
        }

        return shortString;
    }
}