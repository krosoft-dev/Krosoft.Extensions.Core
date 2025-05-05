using Krosoft.Extensions.Core.Helpers;
using Krosoft.Extensions.Samples.Library.Factories;

namespace Krosoft.Extensions.Core.Tests.Helpers;

[TestClass]
public class ActionHelperTests
{
    [TestMethod]
    public async Task ApplyWithAsyncTest()
    {
        var cities = AddresseFactory.GetAdresses()
                                    .Select(c => c.Ville);

        var adresses = await ActionHelper.ApplyWithAsync(cities, CompteFactory.ToCompteAsync);

        var list = adresses.ToList();
        Check.That(list).HasSize(5);
        Check.That(list.Select(x => x.Name)).ContainsExactly("city3", "city4", "city", "city1", "city2");
    }
}