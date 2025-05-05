using Krosoft.Extensions.Core.Converters;
using Krosoft.Extensions.Samples.Library.Models;
using Newtonsoft.Json;

namespace Krosoft.Extensions.Core.Tests.Converters;

[TestClass]
public class NullableIntegerConverterTests
{
    [TestMethod]
    public void ConvertEmptyTest()
    {
        var obj = JsonConvert.DeserializeObject<Item>(string.Empty, new NullableIntegerConverter());
        Check.That(obj).IsNull();
    }

    [TestMethod]
    public void ConvertJsonArrayEmptyTest()
    {
        var json = "[]";
        var obj = JsonConvert.DeserializeObject<IEnumerable<Item>>(json, new NullableIntegerConverter());
        Check.That(obj).IsEmpty();
    }

    [TestMethod]
    public void ConvertJsonEmptyTest()
    {
        var json = "{}";
        var obj = JsonConvert.DeserializeObject<Item>(json, new NullableIntegerConverter());
        Check.That(obj!.GetType()).IsEqualTo(typeof(Item));
        Check.That(obj).IsNotNull();
    }

    [DataTestMethod]
    [DataRow("           ", null)]
    [DataRow("null", null)]
    [DataRow("999", 999)]
    [DataRow("0,24", 24)]
    [DataRow("0.12", 12)]
    [DataRow("test", null)]
    [DataRow("0", 0)]
    public void ConvertJsonTest(string input, int? expected)
    {
        var json = $$""" { "ValeurInt": "{{input}}" } """;

        var obj = JsonConvert.DeserializeObject<Item>(json, new NullableIntegerConverter());
        Check.That(obj).IsNotNull();
        Check.That(obj!.ValeurInt).IsEqualTo(expected);
    }
}