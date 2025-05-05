using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Samples.Library.Factories;

namespace Krosoft.Extensions.Core.Tests.Extensions;

[TestClass]
public class TaskEnumerableExtensionsTests
{
    [TestMethod]
    public async Task DistinctByFromListTest()
    {
        var adresses = AddresseFactory.GetAdresses();
        var task = Task.FromResult(adresses.ToList());
        var adressesUnique = await task.DistinctBy(x => x.Ville);

        var list = adressesUnique.ToList();
        Check.That(list).HasSize(5);
        Check.That(list.Select(x => x.Ville)).ContainsExactly("city3", "city4", "city", "city1", "city2");
    }

    [TestMethod]
    public async Task DistinctByTest()
    {
        var adresses = AddresseFactory.GetAdresses();
        var task = Task.FromResult(adresses);
        var adressesUnique = await task!.DistinctBy(x => x.Ville).ToList();

        Check.That(adressesUnique).HasSize(5);
        Check.That(adressesUnique.Select(x => x.Ville)).ContainsExactly("city3", "city4", "city", "city1", "city2");
    }

    [TestMethod]
    public async Task FirstOrDefaultDefaultTest()
    {
        var adresses = AddresseFactory.GetAdresses();
        var task = Task.FromResult(adresses);
        var address = await task!.FirstOrDefault(x => x.Ville == "test");

        Check.That(address).IsNull();
    }

    [TestMethod]
    public async Task FirstOrDefaultTest()
    {
        var adresses = AddresseFactory.GetAdresses();
        var task = Task.FromResult(adresses);
        var address = await task!.FirstOrDefault(x => x.Ville == "city3");

        Check.That(address).IsNotNull();
        Check.That(address!.Ville).IsEqualTo("city3");
    }

    [TestMethod]
    public async Task ToDictionaryDistinctFromListTest()
    {
        var adresses = AddresseFactory.GetAdresses();
        var task = Task.FromResult(adresses.ToList());
        var adressesParX = await task!.ToDictionary(x => x.Ville, true);

        Check.That(adressesParX).HasSize(5);
        Check.That(adressesParX.Select(x => x.Key)).ContainsExactly("city3", "city4", "city", "city1", "city2");
    }

    [TestMethod]
    public async Task ToDictionaryDistinctTest()
    {
        var adresses = AddresseFactory.GetAdresses();
        var task = Task.FromResult(adresses);
        var adressesParX = await task!.ToDictionary(x => x.Ville, true);

        Check.That(adressesParX).HasSize(5);
        Check.That(adressesParX.Select(x => x.Key)).ContainsExactly("city3", "city4", "city", "city1", "city2");
    }

    [TestMethod]
    public async Task ToDictionaryElementSelectorTest()
    {
        var adresses = AddresseFactory.GetAdresses();
        var task = Task.FromResult(adresses);
        var adressesParX = await task!.ToDictionary(x => x.Ligne1, x => x.Ville);

        Check.That(adressesParX.Keys).HasSize(6);
        Check.That(adressesParX.Keys).ContainsExactly("street3Line1", "street4Line1", "street5Line1", "street1Line1", "street2Line1", "street6Line1");
    }

    [TestMethod]
    public async Task ToDictionaryFromListElementSelectorTest()
    {
        var adresses = AddresseFactory.GetAdresses();
        var task = Task.FromResult(adresses.ToList());
        var adressesParX = await task!.ToDictionary(x => x.Ligne1, x => x.Ville);

        Check.That(adressesParX.Keys).HasSize(6);
        Check.That(adressesParX.Keys).ContainsExactly("street3Line1", "street4Line1", "street5Line1", "street1Line1", "street2Line1", "street6Line1");
    }

    [TestMethod]
    public async Task ToDictionaryFromListTest()
    {
        var adresses = AddresseFactory.GetAdresses();
        var task = Task.FromResult(adresses.ToList());
        var adressesParX = await task!.ToDictionary(x => x.Ligne1);

        Check.That(adressesParX.Keys).HasSize(6);
        Check.That(adressesParX.Keys).ContainsExactly("street3Line1", "street4Line1", "street5Line1", "street1Line1", "street2Line1", "street6Line1");
    }

    [TestMethod]
    public async Task ToDictionaryTest()
    {
        var adresses = AddresseFactory.GetAdresses();
        var task = Task.FromResult(adresses);
        var adressesParX = await task!.ToDictionary(x => x.Ligne1);

        Check.That(adressesParX.Keys).HasSize(6);
        Check.That(adressesParX.Keys).ContainsExactly("street3Line1", "street4Line1", "street5Line1", "street1Line1", "street2Line1", "street6Line1");
    }

    [TestMethod]
    public async Task ToLookupFromListTest()
    {
        var adresses = AddresseFactory.GetAdresses();
        var task = Task.FromResult(adresses.ToList());
        var adressesParX = await task!.ToLookup(x => x.Ligne1);

        Check.That(adressesParX).HasSize(6);
        Check.That(adressesParX.Select(x => x.Key)).ContainsExactly("street3Line1", "street4Line1", "street5Line1", "street1Line1", "street2Line1", "street6Line1");
    }

    [TestMethod]
    public async Task ToLookupTest()
    {
        var adresses = AddresseFactory.GetAdresses();
        var task = Task.FromResult(adresses);
        var adressesParX = await task!.ToLookup(x => x.Ligne1);

        Check.That(adressesParX).HasSize(6);
        Check.That(adressesParX.Select(x => x.Key)).ContainsExactly("street3Line1", "street4Line1", "street5Line1", "street1Line1", "street2Line1", "street6Line1");
    }
}