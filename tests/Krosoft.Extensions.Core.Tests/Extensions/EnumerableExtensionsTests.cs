using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Samples.Library.Factories;

namespace Krosoft.Extensions.Core.Tests.Extensions;

[TestClass]
public class EnumerableExtensionsTests
{
    [TestMethod]
    public async Task ApplyWithAsyncTest()
    {
        var adresses = await AddresseFactory.GetAdresses()
                                            .Select(c => c.Ville)
                                            .ApplyWithAsync(CompteFactory.ToCompteAsync);
        var list = adresses.ToList();
        Check.That(list).HasSize(5);
        Check.That(list.Select(x => x.Name)).ContainsExactly("city3", "city4", "city", "city1", "city2");
    }

    [TestMethod]
    public void ChunkBy_MoreData()
    {
        var chunks = CompteFactory.GetRandom(100000).ChunkBy(9999);
        Check.That(chunks).HasSize(11);
        Check.That(chunks.Select(a => a.Count)).ContainsExactly(9999, 9999, 9999, 9999, 9999, 9999, 9999, 9999, 9999, 9999, 10);
    }

    [TestMethod]
    public void ChunkBy_Ok()
    {
        var adresses = AddresseFactory.GetAdresses().ToList();
        Check.That(adresses.Select(a => a.CodePostal)).ContainsExactly("zipcode3", "zipcode4", "zipcode5", "zipcode1", "zipcode2", "zipcode6");

        var chunks = adresses.ChunkBy(2);
        Check.That(chunks).HasSize(3);
        Check.That(chunks.Select(a => a.Count())).ContainsExactly(2, 2, 2);

        Check.That(chunks.Select(a => a.Select(x => x.CodePostal)))
             .ContainsExactly(new List<IEnumerable<string>>
             {
                 new List<string> { "zipcode3", "zipcode4" },
                 new List<string> { "zipcode5", "zipcode1" },
                 new List<string> { "zipcode2", "zipcode6" }
             });
    }

    [TestMethod]
    public void ToDictionaryDistinctEnumerableTest()
    {
        var adresses = AddresseFactory.GetAdresses().ToList();
        var adressesCity = adresses.Where(x => x.Ville == "city");
        Check.That(adressesCity).HasSize(2);

        var adressesParCity = adresses.AsEnumerable().ToDictionary(x => x.Ville, true);
        Check.That(adressesParCity).HasSize(5);
    }

    [TestMethod]
    public void ToDictionaryDistinctListTest()
    {
        var adresses = AddresseFactory.GetAdresses().ToList();
        var adressesCity = adresses.Where(x => x.Ville == "city");
        Check.That(adressesCity).HasSize(2);

        var adressesParCity = adresses.ToList().ToDictionary(x => x.Ville, true);
        Check.That(adressesParCity).HasSize(5);
    }

    [TestMethod]
    public void ToDictionaryDistinctModificatorEnumerableTest()
    {
        var adresses = AddresseFactory.GetAdresses().ToList();
        var adressesCity = adresses.Where(x => x.Ville == "city");
        Check.That(adressesCity).HasSize(2);

        var adressesParCity = adresses.AsEnumerable().ToDictionary(x => x.Ville, x => x.ToUpper(), true);
        Check.That(adressesParCity).HasSize(5);
    }

    [TestMethod]
    public void ToDictionaryDistinctModificatorListTest()
    {
        var adresses = AddresseFactory.GetAdresses().ToList();
        var adressesCity = adresses.Where(x => x.Ville == "city");
        Check.That(adressesCity).HasSize(2);

        var adressesParCity = adresses.ToList().ToDictionary(x => x.Ville, x => x.ToUpper(), true);
        Check.That(adressesParCity).HasSize(5);
    }
}