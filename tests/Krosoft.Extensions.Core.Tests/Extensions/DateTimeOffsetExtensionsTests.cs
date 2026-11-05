using Krosoft.Extensions.Core.Extensions;

namespace Krosoft.Extensions.Core.Tests.Extensions;

[TestClass]
public class DateTimeOffsetExtensionsTests
{
    /// <summary>
    /// Minuit du 1er du mois en heure locale du serveur, exprimé en UTC.
    /// </summary>
    private static DateTimeOffset ExpectedStartOfMonth(int year, int month)
    {
        var localMidnight = new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Unspecified);
        return new DateTimeOffset(localMidnight, TimeZoneInfo.Local.GetUtcOffset(localMidnight)).ToUniversalTime();
    }

    [TestMethod]
    public void StartOfMonth_PremierJourDuMois_RenvoieMinuitLocalEnUtc()
    {
        var date = new DateTimeOffset(2026, 11, 1, 0, 0, 0, TimeSpan.Zero);

        var result = date.StartOfMonth();

        Check.That(result).IsEqualTo(ExpectedStartOfMonth(2026, 11));
        Check.That(result.Offset).IsEqualTo(TimeSpan.Zero);
        Check.That(result.ToLocalTime().DateTime).IsEqualTo(new DateTime(2026, 11, 1));
    }

    [TestMethod]
    public void StartOfMonth_DernierJourDuMois_RenvoieLePremierDuMeme()
    {
        var date = new DateTimeOffset(2026, 2, 28, 23, 59, 59, TimeSpan.Zero);

        var result = date.StartOfMonth();

        Check.That(result).IsEqualTo(ExpectedStartOfMonth(2026, 2));
        Check.That(result.Offset).IsEqualTo(TimeSpan.Zero);
        Check.That(result.ToLocalTime().DateTime).IsEqualTo(new DateTime(2026, 2, 1));
    }

    [TestMethod]
    public void StartOfMonth_Janvier_NeRecalePasSurLAnneePrecedente()
    {
        var date = new DateTimeOffset(2027, 1, 15, 12, 0, 0, TimeSpan.Zero);

        var result = date.StartOfMonth();

        Check.That(result).IsEqualTo(ExpectedStartOfMonth(2027, 1));
        Check.That(result.ToLocalTime().DateTime).IsEqualTo(new DateTime(2027, 1, 1));
    }

    [TestMethod]
    public void StartOfMonth_31Decembre_RenvoieLe1erDecembreDeLaMemeAnnee()
    {
        var date = new DateTimeOffset(2026, 12, 31, 23, 59, 59, TimeSpan.Zero);

        var result = date.StartOfMonth();

        Check.That(result).IsEqualTo(ExpectedStartOfMonth(2026, 12));
        Check.That(result.ToLocalTime().DateTime).IsEqualTo(new DateTime(2026, 12, 1));
    }

    [TestMethod]
    public void StartOfMonth_OffsetNonNul_UtiliseLeCalendrierDeLaDateFournie()
    {
        // 2026-04-01T00:30+02:00 correspond au 31 mars 22:30 UTC : le mois retenu est celui de l'offset fourni (avril).
        var date = new DateTimeOffset(2026, 4, 1, 0, 30, 0, TimeSpan.FromHours(2));

        var result = date.StartOfMonth();

        Check.That(result).IsEqualTo(ExpectedStartOfMonth(2026, 4));
        Check.That(result.Offset).IsEqualTo(TimeSpan.Zero);
        Check.That(result.ToLocalTime().DateTime).IsEqualTo(new DateTime(2026, 4, 1));
    }

    [TestMethod]
    public void StartOfMonth_OffsetNegatif_UtiliseLeCalendrierDeLaDateFournie()
    {
        // 2026-05-31T22:00-05:00 correspond au 1er juin 03:00 UTC : le mois retenu reste mai.
        var date = new DateTimeOffset(2026, 5, 31, 22, 0, 0, TimeSpan.FromHours(-5));

        var result = date.StartOfMonth();

        Check.That(result).IsEqualTo(ExpectedStartOfMonth(2026, 5));
        Check.That(result.ToLocalTime().DateTime).IsEqualTo(new DateTime(2026, 5, 1));
    }
}
