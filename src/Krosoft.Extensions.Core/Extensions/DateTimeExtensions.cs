namespace Krosoft.Extensions.Core.Extensions;

public static class DateTimeExtensions
{
    private const int Second = 1;
    private const int Minute = 60 * Second;
    private const int Hour = 60 * Minute;
    private const int Day = 24 * Hour;
    private const int Month = 30 * Day;

    /// <summary>
    /// Converts the specified DateTime to its relative date.
    /// </summary>
    /// <param name="dt">The DateTime to convert.</param>
    /// <returns>
    /// A string value based on the relative date
    /// of the datetime as compared to the current date.
    /// </returns>
    public static string ToRelativeDate(this DateTime dt)
    {
        var ts = new TimeSpan(DateTime.UtcNow.Ticks - dt.Ticks);
        var delta = Math.Abs(ts.TotalSeconds);

        if (delta < 0)
        {
            return "dans un instant";
        }

        if (delta < 1 * Minute)
        {
            return "il y a un instant";
        }

        if (delta < 2 * Minute)
        {
            return "il y a une minute";
        }

        if (delta < 45 * Minute)
        {
            var minutes = ts.Minutes;
            if (minutes < 0)
            {
                minutes = 60 + minutes;
            }

            return string.Format("il y a {0} minutes", minutes);
        }

        if (delta < 90 * Minute)
        {
            return "il y a une heure";
        }

        if (delta < 24 * Hour)
        {
            return string.Format("il y a {0} heures", ts.Hours);
        }

        if (delta < 30 * Day)
        {
            return string.Format("il y a {0} jours", ts.Days);
        }

        if (delta < 12 * Month)
        {
            var months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));

            if (months <= 1)
            {
                return "il y a un mois";
            }

            return string.Format("il y a {0} mois", months);
        }

        var years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
        if (years <= 1)
        {
            return "il y a plus d'un an";
        }

        return string.Format("il y a {0} ans", years);
    }
}