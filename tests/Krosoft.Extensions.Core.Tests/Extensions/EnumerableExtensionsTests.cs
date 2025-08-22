using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Samples.Library.Factories;
using Krosoft.Extensions.Samples.Library.Models;

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

    [TestMethod]
    public void ToPagination_Null()
    {
        Check.ThatCode(() => AddresseFactory.GetAdresses()
                                            .ToPagination(null!))
             .Throws<KrosoftTechnicalException>()
             .WithMessage("La variable 'paginationRequest' n'est pas renseignée.");
    }

    [TestMethod]
    public void ToPagination_Ok()
    {
        var paginationRequest = new PaginationRequest
        {
            PageNumber = 1,
            PageSize = 2
        };
        var adresses = AddresseFactory.GetAdresses().ToPagination(paginationRequest);
        Check.That(adresses.PageNumber).IsEqualTo(1);
        Check.That(adresses.PageSize).IsEqualTo(2);
        Check.That(adresses.TotalCount).IsEqualTo(6);
        Check.That(adresses.TotalPages).IsEqualTo(3);
        Check.That(adresses.Items).HasSize(2);
    }

    [TestMethod]
    public void Search_Empty()
    {
        var adresses = AddresseFactory.GetAdresses().Search("1");
        Check.That(adresses).IsEmpty();
    }

    [TestMethod]
    public void Search_Ok()
    {
        var adresses1 = AddresseFactory.GetAdresses().Search("1", x => x.CodePostal).ToList();
        Check.That(adresses1).HasSize(1);
        Check.That(adresses1.First().Ligne1).IsEqualTo("street1Line1");

        var adresses2 = AddresseFactory.GetAdresses().Search("2", x => x.Ligne1, x => x.Ligne2).ToList();
        Check.That(adresses2).HasSize(6);
        Check.That(adresses2.Select(x => x.Ligne1))
             .ContainsExactly("street3Line1", "street4Line1", "street5Line1", "street1Line1", "street2Line1", "street6Line1");
    }

    [TestMethod]
    public void SortBy_Empty()
    {
        var paginationRequest = new PaginationRequest();
        var adresses = AddresseFactory.GetAdresses().SortBy(paginationRequest).ToList();
        Check.That(adresses).HasSize(6);
        Check.That(adresses.Select(x => x.Ligne1))
             .ContainsExactly("street3Line1", "street4Line1", "street5Line1", "street1Line1", "street2Line1", "street6Line1");
    }

    [TestMethod]
    public void SortBy_Ok_Asc()
    {
        var paginationRequest = new PaginationRequest
        {
            SortBy = new HashSet<string> { $"{nameof(Addresse.Ligne1)}:asc" }
        };
        var adresses = AddresseFactory.GetAdresses().SortBy(paginationRequest).ToList();
        Check.That(adresses).HasSize(6);
        Check.That(adresses.Select(x => x.Ligne1))
             .ContainsExactly("street1Line1", "street2Line1", "street3Line1", "street4Line1", "street5Line1", "street6Line1");
    }

    [TestMethod]
    public void SortBy_Ok_Desc()
    {
        var paginationRequest = new PaginationRequest
        {
            SortBy = new HashSet<string> { $"{nameof(Addresse.Ligne1)}:desc" }
        };
        var adresses = AddresseFactory.GetAdresses().SortBy(paginationRequest).ToList();
        Check.That(adresses).HasSize(6);
        Check.That(adresses.Select(x => x.Ligne1))
             .ContainsExactly("street6Line1", "street5Line1", "street4Line1", "street3Line1", "street2Line1", "street1Line1");
    }

    [TestMethod]
    public void SortBy_NoDirection()
    {
        var paginationRequest = new PaginationRequest
        {
            SortBy = new HashSet<string> { $"{nameof(Addresse.Ligne1)}:aaaa" }
        };
        var adresses = AddresseFactory.GetAdresses().SortBy(paginationRequest).ToList();
        Check.That(adresses).HasSize(6);
        Check.That(adresses.Select(x => x.Ligne1))
             .ContainsExactly("street6Line1", "street5Line1", "street4Line1", "street3Line1", "street2Line1", "street1Line1");
    }
}