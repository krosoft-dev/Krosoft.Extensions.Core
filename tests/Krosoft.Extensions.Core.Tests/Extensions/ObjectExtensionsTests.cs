using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Samples.Library.Factories;
using Krosoft.Extensions.Samples.Library.Models;

namespace Krosoft.Extensions.Core.Tests.Extensions;

[TestClass]
public class ObjectExtensionsTests
{
    [TestMethod]
    public void DeepCopyTest()
    {
        var compte = new Compte
        {
            Id = "001",
            Name = "Test 001"
        };

        var cloned = compte.DeepCopy();
        Check.That(cloned).IsNotNull();
        Check.That(cloned!.Id).IsEqualTo("001");
        Check.That(cloned.Name).IsEqualTo("Test 001");
    }

    [TestMethod]
    public void SetPropertyValueTest()
    {
        var compte = new Compte
        {
            Id = "001", Name = "Test 001"
        };

        Check.That(compte.Name).IsEqualTo("Test 001");

        compte.SetPropertyValue(p => p.Name, "value");

        Check.That(compte.Name).IsEqualTo("value");
    }

    [TestMethod]
    public void ToCsvTest()
    {
        var adresses = AddresseFactory.GetAdresses();
        var csv = adresses.ToCsv(";");

        Check.That(csv).IsNotNull();
        Check.That(csv).IsNotEmpty();
    }
}