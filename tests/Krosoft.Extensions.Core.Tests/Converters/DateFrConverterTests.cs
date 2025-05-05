using Krosoft.Extensions.Core.Converters;
using Krosoft.Extensions.Samples.Library.Models;
using Newtonsoft.Json;

namespace Krosoft.Extensions.Core.Tests.Converters;

[TestClass]
public class DateFrConverterTests
{
    [TestMethod]
    public void ConvertEmptyTest()
    {
        var obj = JsonConvert.DeserializeObject<Item>(string.Empty, new DateFrConverter());
        Check.That(obj).IsNull();
    }

    [TestMethod]
    public void ConvertJsonArrayEmptyTest()
    {
        var json = "[]";
        var obj = JsonConvert.DeserializeObject<IEnumerable<Item>>(json, new DateFrConverter());
        Check.That(obj).IsEmpty();
    }

    [TestMethod]
    public void ConvertJsonEmptyTest()
    {
        var json = "{}";
        var obj = JsonConvert.DeserializeObject<Item>(json, new DateFrConverter());
        Check.That(obj!.GetType()).IsEqualTo(typeof(Item));
        Check.That(obj).IsNotNull();
    }

    [TestMethod]
    public void ConvertJsonTest()
    {
        const string json = @"{
            ""Date"": ""12/01/1988""
        }";
        var obj = JsonConvert.DeserializeObject<Item>(json, new DateFrConverter());
        Check.That(obj).IsNotNull();
        Check.That(obj!.Date).IsEqualTo(new DateTime(1988, 1, 12));
    }
}