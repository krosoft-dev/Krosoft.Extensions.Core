using System.Collections.ObjectModel;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Samples.Library.Factories;
using Krosoft.Extensions.Samples.Library.Models;

namespace Krosoft.Extensions.Core.Tests.Extensions;

[TestClass]
public class ReadOnlyDictionaryExtensionsTests
{
    [TestMethod]
    public void GetValueOrDefaultIReadOnlyDictionaryDefaultValueDictionaryEmptyTest()
    {
        IReadOnlyDictionary<string, Dictionary<string, Dictionary<string, decimal>>> valeursParKey = new ReadOnlyDictionary<string, Dictionary<string, Dictionary<string, decimal>>>(new Dictionary<string, Dictionary<string, Dictionary<string, decimal>>>());

        var key1 = "";
        var key2 = "";
        var key3 = "";

        var valeurPourKey1 = valeursParKey.GetValueOrDefault(key1, new Dictionary<string, Dictionary<string, decimal>>());

        Check.That(valeurPourKey1).IsNotNull();
        Check.That(valeurPourKey1).HasSize(0);

        var valeurPourKey2 = DictionaryExtensions.GetValueOrDefault(valeurPourKey1, key2, new Dictionary<string, decimal>());
        Check.That(valeurPourKey2).IsNotNull();
        Check.That(valeurPourKey2).HasSize(0);

        var valeurPourKey3 = valeurPourKey2!.GetValueOrDefault(key3);
        Check.That(valeurPourKey3).IsEqualTo(0);
    }

    [TestMethod]
    public void GetValueOrDefaultIReadOnlyDictionaryDefaultValueDictionaryTest()
    {
        var dictionary = new Dictionary<string, Dictionary<string, Dictionary<string, decimal>>>
        {
            {
                "test1", new Dictionary<string, Dictionary<string, decimal>>
                {
                    {
                        "test2", new Dictionary<string, decimal>
                        {
                            { "test3", 42 }
                        }
                    }
                }
            }
        };

        IReadOnlyDictionary<string, Dictionary<string, Dictionary<string, decimal>>> valeursParKey = new ReadOnlyDictionary<string, Dictionary<string, Dictionary<string, decimal>>>(dictionary);

        var key1 = "test1";
        var key2 = "test2";
        var key3 = "test3";

        var valeurPourKey1 = valeursParKey.GetValueOrDefault(key1, new Dictionary<string, Dictionary<string, decimal>>());
        Check.That(valeurPourKey1).IsNotNull();
        Check.That(valeurPourKey1).HasSize(1);

        var valeurPourKey2 = DictionaryExtensions.GetValueOrDefault(valeurPourKey1, key2, new Dictionary<string, decimal>());
        Check.That(valeurPourKey2).IsNotNull();
        Check.That(valeurPourKey2).HasSize(1);

        var valeurPourKey3 = valeurPourKey2!.GetValueOrDefault(key3);
        Check.That(valeurPourKey3).IsEqualTo(42);
    }

    [TestMethod]
    public void GetValueOrDefaultIReadOnlyDictionaryDefaultValueTest()
    {
        var adressesParStreetLine1 = AddresseFactory.GetAdresses().ToReadOnlyDictionary(x => x.Ligne1);

        var address = adressesParStreetLine1.GetValueOrDefault("test");
        Check.That(address).IsNull();
    }

    [TestMethod]
    public void GetValueOrDefaultIReadOnlyDictionaryTest()
    {
        var adressesParStreetLine1 = AddresseFactory.GetAdresses().ToReadOnlyDictionary(x => x.Ligne1);

        var address = adressesParStreetLine1.GetValueOrDefault("street1Line1");
        Check.That(address).IsNotNull();
        Check.That(address!.Ligne2).IsEqualTo("street1Line2");
    }

    [TestMethod]
    public void GetValueOrDefaultReadOnlyDictionaryDefaultValueTest()
    {
        var adressesParStreetLine1 = new
            ReadOnlyDictionary<string, Addresse>(AddresseFactory.GetAdresses().ToDictionary(x => x.Ligne1));

        var address = adressesParStreetLine1.GetValueOrDefault("test");
        Check.That(address).IsNull();
    }

    [TestMethod]
    public void GetValueOrDefaultReadOnlyDictionaryTest()
    {
        var adressesParStreetLine1 = new
            ReadOnlyDictionary<string, Addresse>(AddresseFactory.GetAdresses().ToDictionary(x => x.Ligne1));

        var address = adressesParStreetLine1.GetValueOrDefault("street1Line1");
        Check.That(address).IsNotNull();
        Check.That(address!.Ligne2).IsEqualTo("street1Line2");
    }
}