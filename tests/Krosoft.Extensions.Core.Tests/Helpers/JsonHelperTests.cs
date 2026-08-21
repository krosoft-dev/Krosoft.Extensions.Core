using Krosoft.Extensions.Core.Helpers;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Samples.Library.Models;
using Newtonsoft.Json.Linq;

namespace Krosoft.Extensions.Core.Tests.Helpers;

[TestClass]
public class JsonHelperTests
{
    private const char NullChar = (char)0;

    [TestMethod]
    public void GetTest()
    {
        var items = JsonHelper.Get<Item>(typeof(JsonHelperTests).Assembly).ToList();
        Check.That(items).IsNotNull();
        Check.That(items).HasSize(3);
        Check.That(items.Select(g => g.Libelle)).ContainsExactly("Item 001", "item 002 dolor consectetur", "item trois");
    }

    [TestMethod]
    public void IsValidTest()
    {
        var json = AssemblyHelper.ReadAsString(typeof(JsonHelperTests).Assembly, $"{nameof(Item)}.json", EncodingHelper.GetEuropeOccidentale());

        Check.That(JsonHelper.IsValid(json)).IsEqualTo(true);
    }

    [TestMethod]
    public void Normalize_WhenArray_ShouldReturnCompactJson()
    {
        var result = JsonHelper.Normalize("[ 1, 2, { 'name': 'test' } ]");

        Check.That(result).IsEqualTo("[1,2,{\"name\":\"test\"}]");
    }

    [TestMethod]
    public void Normalize_WhenInputIsNotJson_ShouldReturnNull()
    {
        Check.That(JsonHelper.Normalize("un message texte")).IsNull();
        Check.That(JsonHelper.Normalize("{ \"name\": }")).IsNull();
        Check.That(JsonHelper.Normalize(null)).IsNull();
        Check.That(JsonHelper.Normalize(string.Empty)).IsNull();
    }

    [TestMethod]
    public void Normalize_WhenLooseJson_ShouldReturnStrictJson()
    {
        var result = JsonHelper.Normalize("{ 'name': 'test', /* commentaire */ 'age': 25, }");

        Check.That(result).IsEqualTo("{\"name\":\"test\",\"age\":25}");
    }

    [TestMethod]
    public void Normalize_WhenNullCharInValue_ShouldRemoveIt()
    {
        var result = JsonHelper.Normalize("{ \"name\": \"te" + NullChar + "st\" }");

        Check.That(result).IsEqualTo("{\"name\":\"test\"}");
    }

    [TestMethod]
    public void Normalize_WhenUnescapedControlCharInValue_ShouldEscapeIt()
    {
        var result = JsonHelper.Normalize("{ \"name\": \"li\ngne\" }");

        Check.That(result).IsEqualTo("{\"name\":\"li\\ngne\"}");
    }

    [TestMethod]
    public void RemoveNullChars_WhenNullCharInString_ShouldRemoveIt()
    {
        Check.That(JsonHelper.RemoveNullChars("te" + NullChar + "st")).IsEqualTo("test");
        Check.That(JsonHelper.RemoveNullChars(null)).IsNull();
    }

    [TestMethod]
    public void ReplacePath_WhenNewValueIsNull_ShouldReturnOriginalObject()
    {
        var root = JObject.Parse("{ 'name': 'test' }");

        var result = root.ReplacePath<object>("$.name", null);

        Check.That(result).IsEqualTo(root);
    }

    [TestMethod]
    public void ReplacePath_WhenPathDoesNotExist_ShouldReturnUnmodifiedObject()
    {
        var root = JObject.Parse("{ 'name': 'test' }");
        var original = root.ToString();

        var result = root.ReplacePath("$.nonexistent", "newValue");

        Check.That(result.ToString()).IsEqualTo(original);
    }

    [TestMethod]
    public void ReplacePath_WhenPathExists_ShouldReplaceValue()
    {
        var root = JObject.Parse("{ 'name': 'test', 'age': 25 }");

        var result = root.ReplacePath("$.name", "newTest");

        Check.That(result["name"]!.Value<string>()).IsEqualTo("newTest");
        Check.That(result["age"]!.Value<int>()).IsEqualTo(25);
    }

    [TestMethod]
    public void ReplacePath_WhenPathIsNull_ShouldThrowArgumentException()
    {
        var root = JObject.Parse("{ 'name': 'test' }");

        Check.ThatCode(() => root.ReplacePath(null!, "newValue"))
             .Throws<KrosoftTechnicalException>()
             .WithMessage("La variable 'path' est vide ou non renseignée.");
    }

    [TestMethod]
    public void ReplacePath_WhenReplacingArrayElement_ShouldReplaceCorrectly()
    {
        var root = JObject.Parse("{ 'users': [{ 'name': 'test1' }, { 'name': 'test2' }] }");

        var result = root.ReplacePath("$.users[0].name", "newTest");

        Check.That(result["users"]?[0]?["name"]?.Value<string>()).IsEqualTo("newTest");
        Check.That(result["users"]?[1]?["name"]?.Value<string>()).IsEqualTo("test2");
    }

    [TestMethod]
    public void ReplacePath_WhenReplacingNestedProperty_ShouldReplaceCorrectly()
    {
        var root = JObject.Parse("{ 'user': { 'name': 'test', 'age': 25 } }");

        var result = root.ReplacePath("$.user.name", "newTest");

        Check.That(result["user"]!["name"]!.Value<string>()).IsEqualTo("newTest");
        Check.That(result["user"]!["age"]!.Value<int>()).IsEqualTo(25);
    }

    [TestMethod]
    public void ReplacePath_WhenRootIsNotJObject_ShouldThrowInvalidOperationException()
    {
        var root = JArray.Parse("[1, 2, 3]");

        Check.ThatCode(() => root.ReplacePath("$[0]", 4))
             .Throws<KrosoftTechnicalException>()
             .WithMessage("Impossible de convertir en JObject.");
    }

    [TestMethod]
    public void ReplacePath_WhenRootIsNull_ShouldThrowArgumentNullException()
    {
        JToken? root = null;
        Check.ThatCode(() => root!.ReplacePath("$.path", "newValue"))
             .Throws<KrosoftTechnicalException>()
             .WithMessage("La variable 'root' n'est pas renseignée.");
    }

    [TestMethod]
    public void ToBase64NoObjectTest()
    {
        var base64 = JsonHelper.ToBase64(1);
        Check.That(base64).IsEqualTo("MQ==");
    }

    [TestMethod]
    public void ToBase64NullTest()
    {
        Check.ThatCode(() => JsonHelper.ToBase64(null))
             .Throws<KrosoftTechnicalException>()
             .WithMessage("La variable 'obj' n'est pas renseignée.");
    }

    [TestMethod]
    public void ToBase64Test()
    {
        var data = new
        {
            id = "requestId",
            jwt = "jwtToken"
        };

        var base64 = JsonHelper.ToBase64(data);

        Check.That(base64).IsEqualTo("eyJpZCI6InJlcXVlc3RJZCIsImp3dCI6Imp3dFRva2VuIn0=");
    }
}