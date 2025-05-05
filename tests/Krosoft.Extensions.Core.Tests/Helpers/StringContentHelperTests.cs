using System.Text.Json.Serialization;
using Krosoft.Extensions.Core.Helpers;
using Newtonsoft.Json;

namespace Krosoft.Extensions.Core.Tests.Helpers;

[TestClass]
public class StringContentHelperTests
{
    [TestMethod]
    public async Task SerializeAsJson_Anonyme()
    {
        var data = new { Test = "Hello" };
        await CheckSerializationAsync(data, @"{""Test"":""Hello""}");
    }

    [TestMethod]
    public async Task SerializeAsJson_NoAttribute()
    {
        var data = new TestNoAttribute { Message = "Hello" };
        await CheckSerializationAsync(data, @"{""Message"":""Hello""}");
    }

    [TestMethod]
    public async Task SerializeAsJson_Newtonsoft()
    {
        var data = new TestNewtonsoft { Message = "Hello" };
        await CheckSerializationAsync(data, @"{""Test"":""Hello""}");
    }

    [TestMethod]
    public async Task SerializeAsJson_SystemTextJson()
    {
        //SystemTextJson n'est pas supporté, c'est normal.
        var data = new TestSystemTextJson { Message = "Hello" };
        await CheckSerializationAsync(data, @"{""Message"":""Hello""}");
    }

    private static async Task CheckSerializationAsync(object data, string expectedJson)
    {
        var httpContent = StringContentHelper.SerializeAsJson(data);

        Check.That(httpContent).IsNotNull();
        Check.That(httpContent.Headers).HasSize(1);
        Check.That(httpContent.Headers.ContentType?.ToString()).IsEqualTo("application/json; charset=utf-8");
        Check.That(await httpContent.ReadAsStringAsync(CancellationToken.None)).IsEqualTo(expectedJson);
    }

    private class TestNoAttribute
    {
        public string? Message { get; set; }
    }

    private class TestSystemTextJson
    {
        [JsonPropertyName("Test")]
        public string? Message { get; set; }
    }

    private class TestNewtonsoft
    {
        [JsonProperty("Test")]
        public string? Message { get; set; }
    }
}