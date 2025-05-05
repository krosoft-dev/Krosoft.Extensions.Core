using Krosoft.Extensions.Core.Converters;
using Krosoft.Extensions.Samples.Library.Models;
using Newtonsoft.Json;

namespace Krosoft.Extensions.Core.Tests.Converters;

[TestClass]
public class BooleanConverterTests
{
    [TestMethod]
    public void ConvertEmptyTest()
    {
        var obj = JsonConvert.DeserializeObject<Item>(string.Empty, new BooleanConverter());
        Check.That(obj).IsNull();
    }

    [TestMethod]
    public void ConvertJsonArrayEmptyTest()
    {
        var json = "[]";
        var obj = JsonConvert.DeserializeObject<IEnumerable<Item>>(json, new BooleanConverter());
        Check.That(obj).IsEmpty();
    }

    [TestMethod]
    public void ConvertJsonEmptyTest()
    {
        var json = "{}";
        var obj = JsonConvert.DeserializeObject<Item>(json, new BooleanConverter());
        Check.That(obj!.GetType()).IsEqualTo(typeof(Item));
        Check.That(obj).IsNotNull();
    }

    [TestMethod]
    public void ConvertJsonErrorTest()
    {
        const string json = @"{
            ""IsActif"": ""test""
        }";

        Check.ThatCode(() => JsonConvert.DeserializeObject<Item>(json, new BooleanConverter()))
             .Throws<JsonSerializationException>()
             .WithMessage("Error converting value \"test\" to type 'System.Boolean'. Path 'IsActif', line 2, position 29.");
    }

    [DataTestMethod]
    [DataRow("faux", false)]
    [DataRow("false", false)]
    [DataRow("no", false)]
    [DataRow("f", false)]
    [DataRow("n", false)]
    [DataRow("0", false)]
    public void ConvertJsonTest(string input, bool expected)
    {
        var json = $$""" { "IsActif": "{{input}}" } """;

        var obj = JsonConvert.DeserializeObject<Item>(json, new BooleanConverter());
        Check.That(obj).IsNotNull();
        Check.That(obj!.IsActif).IsEqualTo(expected);
    }
}