using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Interfaces;
using Krosoft.Extensions.Testing.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Core.Tests.Services;

[TestClass]
public class DateTimeServiceTests
{
    [TestMethod]
    public void NowMockTest()
    {
        var services = new ServiceCollection();
        services.AddDateTimeService();
        var mockDateTimeService = new Mock<IDateTimeService>();
        mockDateTimeService.Setup(x => x.Now)
                           .Returns(new DateTime(2012, 1, 3));
        services.SwapTransient(_ => mockDateTimeService.Object);
        var buildServiceProvider = services.BuildServiceProvider();
        var service = buildServiceProvider.GetRequiredService<IDateTimeService>();

        Check.That(service.Now.DateTime).IsEqualTo(new DateTime(2012, 1, 3));
    }

    [TestMethod]
    public void NowTest()
    {
        var services = new ServiceCollection();
        services.AddDateTimeService();
        var buildServiceProvider = services.BuildServiceProvider();
        var service = buildServiceProvider.GetRequiredService<IDateTimeService>();

        var delta = DateTime.Now - service.Now;
        var threshold = new TimeSpan(0, 0, 0, 0, 1);

        Console.WriteLine($"delta : {delta}");
        Console.WriteLine($"threshold : {threshold}");

        Check.That(delta).IsLessThan(threshold);
    }
}