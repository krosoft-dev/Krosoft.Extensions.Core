using Krosoft.Extensions.Core.Interfaces;
using Krosoft.Extensions.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Core.Extensions;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddDateTimeService() => services.AddTransient<IDateTimeService, DateTimeService>();

        public IServiceCollection AddXmlLoaderService() => services.AddTransient<IXmlLoaderService, XmlLoaderService>();
    }
}