using System.Net;
using Krosoft.Extensions.Core.Converters;
using Krosoft.Extensions.Core.Models.Exceptions;
using Newtonsoft.Json;

namespace Krosoft.Extensions.Core.Tests.Converters;

[TestClass]
public class KrosoftFunctionalExceptionConverterTests
{
    [TestMethod]
    public void ConvertEmptyTest()
    {
        var obj = JsonConvert.DeserializeObject<KrosoftFunctionalException>(string.Empty, new KrosoftFunctionalExceptionConverter());
        Check.That(obj).IsNull();
    }

    [TestMethod]
    public void ConvertJsonArrayEmptyTest()
    {
        var json = "[]";
        var obj = JsonConvert.DeserializeObject<IEnumerable<KrosoftFunctionalException>>(json, new KrosoftFunctionalExceptionConverter());
        Check.That(obj).IsEmpty();
    }

    [TestMethod]
    public void ConvertJsonEmptyNoErreursTest()
    {
        const string json = @"{
            ""Code"": 400,
            ""Message"": ""BadRequest"",
            ""Erreurs"": []
        }";
        var obj = JsonConvert.DeserializeObject<KrosoftFunctionalException>(json, new KrosoftFunctionalExceptionConverter());
        Check.That(obj).IsNotNull();
        Check.That(obj!.Code).IsEqualTo(HttpStatusCode.BadRequest);
        Check.That(obj.Errors).IsEmpty();
    }

    [TestMethod]
    public void ConvertJsonEmptyTest()
    {
        var json = "{}";
        var obj = JsonConvert.DeserializeObject<KrosoftFunctionalException>(json, new KrosoftFunctionalExceptionConverter());
        Check.That(obj!.GetType()).IsEqualTo(typeof(KrosoftFunctionalException));
        Check.That(obj).IsNotNull();
    }

    [TestMethod]
    public void ConvertJsonTest()
    {
        const string json = @"{
            ""Code"": 400,
            ""Message"": ""BadRequest"",
            ""Errors"": [
                ""An ean13 must contain 13 digits."",
                ""Un message là.""
                ]

             }";
        var obj = JsonConvert.DeserializeObject<KrosoftFunctionalException>(json, new KrosoftFunctionalExceptionConverter());
        Check.That(obj).IsNotNull();
        Check.That(obj!.Code).IsEqualTo(HttpStatusCode.BadRequest);
        Check.That(obj.Message).IsEqualTo("An ean13 must contain 13 digits.");
        Check.That(obj.Errors).HasSize(2);
        Check.That(obj.Errors.First()).IsEqualTo("An ean13 must contain 13 digits.");
        Check.That(obj.Errors.Last()).IsEqualTo("Un message là.");
    }
}