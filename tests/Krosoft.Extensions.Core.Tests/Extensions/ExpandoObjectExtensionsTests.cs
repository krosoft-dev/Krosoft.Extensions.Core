using System.Dynamic;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Krosoft.Extensions.Core.Tests.Extensions;

[TestClass]
public class ExpandoObjectExtensionsTests
{
    [TestMethod]
    public void GetStringTest()
    {
        var json = AssemblyHelper.ReadAsString(typeof(ExpandoObjectExtensionsTests).Assembly, "en.json", EncodingHelper.GetEuropeOccidentale());
        var expandoObject = JsonConvert.DeserializeObject<ExpandoObject>(json, new ExpandoObjectConverter());
        var titre = expandoObject!.GetString("titre");

        Check.That(titre).IsEqualTo("Forgotten password");
    }
}