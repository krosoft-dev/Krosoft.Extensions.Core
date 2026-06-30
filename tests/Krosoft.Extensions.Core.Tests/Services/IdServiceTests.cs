using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Core.Tests.Services;

[TestClass]
public class IdServiceTests
{
    private IIdService _idService = null!;

    [TestInitialize]
    public void SetUp()
    {
        var services = new ServiceCollection();
        services.AddIdService();
        _idService = services.BuildServiceProvider().GetRequiredService<IIdService>();
    }

    [TestMethod]
    public void NewGuid_Returns_NonEmpty()
    {
        var result = _idService.NewGuid();

        Check.That(result).IsNotEqualTo(Guid.Empty);
    }

    [TestMethod]
    public void NewGuid_Returns_UniqueValues()
    {
        var first = _idService.NewGuid();
        var second = _idService.NewGuid();

        Check.That(first).IsNotEqualTo(second);
    }

    [TestMethod]
    public void NewGuidVersion7_Returns_NonEmpty()
    {
        var result = _idService.NewGuidVersion7();

        Check.That(result).IsNotEqualTo(Guid.Empty);
    }

    [TestMethod]
    public void NewGuidVersion7_Returns_UniqueValues()
    {
        var first = _idService.NewGuidVersion7();
        var second = _idService.NewGuidVersion7();

        Check.That(first).IsNotEqualTo(second);
    }

    [TestMethod]
    public void NewGuidVersion7_IsTimeOrdered()
    {
        var first = _idService.NewGuidVersion7();
        Thread.Sleep(1000);
        var second = _idService.NewGuidVersion7();

        Check.That(first.CompareTo(second)).IsStrictlyLessThan(0);
    }
}
