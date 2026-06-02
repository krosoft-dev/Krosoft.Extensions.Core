using Krosoft.Extensions.Core.Extensions;

namespace Krosoft.Extensions.Core.Tests.Extensions;

[TestClass]
public class TimeSpanExtensionsTests
{
    [TestMethod]
    public void ToShortString_Zero_ReturnsZeroMs()
    {
        var result = TimeSpan.Zero.ToShortString();
        Check.That(result).IsEqualTo("0ms");
    }

    [TestMethod]
    public void ToShortString_MillisecondsOnly()
    {
        var result = TimeSpan.FromMilliseconds(500).ToShortString();
        Check.That(result).IsEqualTo("500ms");
    }

    [TestMethod]
    public void ToShortString_SecondsOnly()
    {
        var result = TimeSpan.FromSeconds(30).ToShortString();
        Check.That(result).IsEqualTo("30s");
    }

    [TestMethod]
    public void ToShortString_MinutesOnly()
    {
        var result = TimeSpan.FromMinutes(5).ToShortString();
        Check.That(result).IsEqualTo("5m");
    }

    [TestMethod]
    public void ToShortString_HoursOnly()
    {
        var result = TimeSpan.FromHours(2).ToShortString();
        Check.That(result).IsEqualTo("2h");
    }

    [TestMethod]
    public void ToShortString_DaysOnly()
    {
        var result = TimeSpan.FromDays(3).ToShortString();
        Check.That(result).IsEqualTo("3j");
    }

    [TestMethod]
    public void ToShortString_MinutesAndSeconds()
    {
        var result = new TimeSpan(0, 0, 2, 45).ToShortString();
        Check.That(result).IsEqualTo("2m, 45s");
    }

    [TestMethod]
    public void ToShortString_HoursMinutesSeconds()
    {
        var result = new TimeSpan(0, 1, 30, 15).ToShortString();
        Check.That(result).IsEqualTo("1h, 30m, 15s");
    }

    [TestMethod]
    public void ToShortString_DaysHoursMinutesSecondsMilliseconds()
    {
        var result = new TimeSpan(1, 2, 3, 4, 500).ToShortString();
        Check.That(result).IsEqualTo("1j, 2h, 3m, 4s, 500ms");
    }

    [TestMethod]
    public void ToShortString_SecondsAndMilliseconds()
    {
        var result = new TimeSpan(0, 0, 0, 10, 250).ToShortString();
        Check.That(result).IsEqualTo("10s, 250ms");
    }
}
