using System.Globalization;
using Krosoft.Extensions.Core.Converters;
using Krosoft.Extensions.Samples.Library.Models;
using Newtonsoft.Json;

namespace Krosoft.Extensions.Core.Tests.Converters;

[TestClass]
public class NullableDecimalConverterTests
{
    private const string CultureFr = "fr-FR";
    private const string CultureEn = "en-US";

    [TestMethod]
    public void ConvertEmptyTest()
    {
        var obj = JsonConvert.DeserializeObject<Item>(string.Empty, new NullableDecimalConverter());
        Check.That(obj).IsNull();
    }

    [TestMethod]
    public void ConvertJsonArrayEmptyTest()
    {
        var json = "[]";
        var obj = JsonConvert.DeserializeObject<IEnumerable<Item>>(json, new NullableDecimalConverter());
        Check.That(obj).IsEmpty();
    }

    [TestMethod]
    public void ConvertJsonEmptyTest()
    {
        var json = "{}";
        var obj = JsonConvert.DeserializeObject<Item>(json, new NullableDecimalConverter());
        Check.That(obj!.GetType()).IsEqualTo(typeof(Item));
        Check.That(obj).IsNotNull();
    }

    [DataTestMethod]
    [DataRow("           ", null, CultureFr)]
    [DataRow("null", null, CultureFr)]
    [DataRow("999,45462", 999.45462, CultureFr)]
    [DataRow("999,45462", 999.45462, CultureFr)]
    [DataRow("5.45462", 5.45462, CultureFr)]
    [DataRow("0,07", 0.07, CultureFr)]
    [DataRow("0,42", 0.42, CultureFr)]
    [DataRow("0.12", 0.12, CultureFr)]
    [DataRow("test", null, CultureFr)]
    [DataRow("0", 0.0, CultureFr)]
    [DataRow("           ", null, CultureEn)]
    [DataRow("null", null, CultureEn)]
    [DataRow("999,45462", 999.45462, CultureEn)]
    [DataRow("999,45462", 999.45462, CultureEn)]
    [DataRow("5.45462", 5.45462, CultureEn)]
    [DataRow("0,07", 0.07, CultureEn)]
    [DataRow("0,42", 0.42, CultureEn)]
    [DataRow("0.12", 0.12, CultureEn)]
    [DataRow("test", null, CultureEn)]
    [DataRow("0", 0.0, CultureEn)]
    public void ConvertJsonTest(string input, double? expected, string cultureName)
    {
        var json = $$""" { "ValeurDecimal": "{{input}}" } """;

        var obj = JsonConvert.DeserializeObject<Item>(json, new NullableDecimalConverter(new CultureInfo(cultureName)));
        Check.That(obj).IsNotNull();
        Check.That(obj!.ValeurDecimal).IsEqualTo((decimal?)expected);
    }
}