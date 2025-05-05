using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Samples.Library.Factories;
using Krosoft.Extensions.Samples.Library.Models;

namespace Krosoft.Extensions.Core.Tests.Extensions;

[TestClass]
public class DictionaryExtensionsTests
{
    [TestMethod]
    public void GetValueOrDefaultDictionaryDefaultValueTest()
    {
        var adressesParStreetLine1 = AddresseFactory.GetAdresses().ToDictionary(x => x.Ligne1);

        var address = adressesParStreetLine1.GetValueOrDefault("test");
        Check.That(address).IsNull();
    }

    [TestMethod]
    public void GetValueOrDefaultDictionaryTest()
    {
        var adressesParStreetLine1 = AddresseFactory.GetAdresses().ToDictionary(x => x.Ligne1);

        var address = adressesParStreetLine1.GetValueOrDefault("street1Line1");
        Check.That(address).IsNotNull();
        Check.That(address!.Ligne2).IsEqualTo("street1Line2");
    }

    [TestMethod]
    public void GetValueOrDefaultIDictionaryDefaultValueTest()
    {
        IDictionary<string, Addresse> adressesParStreetLine1 = AddresseFactory.GetAdresses().ToDictionary(x => x.Ligne1);

        var address = adressesParStreetLine1.GetValueOrDefault("test");
        Check.That(address).IsNull();
    }

    [TestMethod]
    public void GetValueOrDefaultIDictionaryTest()
    {
        IDictionary<string, Addresse> adressesParStreetLine1 = AddresseFactory.GetAdresses().ToDictionary(x => x.Ligne1);

        var address = adressesParStreetLine1.GetValueOrDefault("street1Line1");
        Check.That(address).IsNotNull();
        Check.That(address!.Ligne2).IsEqualTo("street1Line2");
    }

    [TestMethod]
    public void AddOrReplace_WhenKeyDoesNotExist_ShouldAddNewEntry()
    {
        var dictionary = new Dictionary<string, int>();

        dictionary.AddOrReplace("key1", 1);

        Check.That(dictionary["key1"]).IsEqualTo(1);
        Check.That(dictionary).HasSize(1);
    }

    [TestMethod]
    public void AddOrReplace_WhenKeyExists_ShouldReplaceValue()
    {
        var dictionary = new Dictionary<string, int> { { "key1", 1 } };

        dictionary.AddOrReplace("key1", 2);

        Check.That(dictionary["key1"]).IsEqualTo(2);
        Check.That(dictionary).HasSize(1);
    }

    [TestMethod]
    public void AddOrUpdate_Decimal_WhenKeyDoesNotExist_ShouldAddNewEntry()
    {
        var dictionary = new Dictionary<string, decimal>();

        dictionary.AddOrUpdate("key1", 10.5m);

        Check.That(dictionary["key1"]).IsEqualTo(10.5m);
    }

    [TestMethod]
    public void AddOrUpdate_Decimal_WhenKeyExists_ShouldAddToExistingValue()
    {
        var dictionary = new Dictionary<string, decimal> { { "key1", 10.5m } };

        dictionary.AddOrUpdate("key1", 5.5m);

        Check.That(dictionary["key1"]).IsEqualTo(16.0m);
    }

    [TestMethod]
    public void AddOrUpdate_List_WhenKeyDoesNotExist_ShouldAddNewList()
    {
        var dictionary = new Dictionary<string, List<int>>();

        dictionary.AddOrUpdate("key1", 1);

        Check.That(dictionary["key1"]).ContainsExactly(1);
    }

    [TestMethod]
    public void AddOrUpdate_List_WhenKeyExists_ShouldAddToList()
    {
        var dictionary = new Dictionary<string, List<int>> { { "key1", new List<int> { 1 } } };

        dictionary.AddOrUpdate("key1", 2);

        Check.That(dictionary["key1"]).ContainsExactly(1, 2);
    }

    [TestMethod]
    public void AddOrUpdate_List_WhenIgnoreDuplicate_ShouldNotAddDuplicate()
    {
        var dictionary = new Dictionary<string, List<int>> { { "key1", new List<int> { 1 } } };

        dictionary.AddOrUpdate("key1", 1, true);

        Check.That(dictionary["key1"]).ContainsExactly(1);
    }

    [TestMethod]
    public void AddRange_WithoutReplace_ShouldAddNewEntries()
    {
        var target = new Dictionary<string, int> { { "key1", 1 } };
        var source = new Dictionary<string, int> { { "key2", 2 } };

        target.AddRange(source);

        Check.That(target).HasSize(2);
        Check.That(target["key1"]).IsEqualTo(1);
        Check.That(target["key2"]).IsEqualTo(2);
    }

    [TestMethod]
    public void AddRange_WithReplace_ShouldReplaceExistingEntries()
    {
        var target = new Dictionary<string, int> { { "key1", 1 } };
        var source = new Dictionary<string, int> { { "key1", 2 } };

        target.AddRange(source, true);

        Check.That(target).HasSize(1);
        Check.That(target["key1"]).IsEqualTo(2);
    }

    [TestMethod]
    public void AddRange_WithoutReplace_WhenKeyExists_ShouldThrowException()
    {
        var target = new Dictionary<string, int> { { "key1", 1 } };
        var source = new Dictionary<string, int> { { "key1", 2 } };

        Check.ThatCode(() => target.AddRange(source))
             .Throws<ArgumentException>();
    }
}