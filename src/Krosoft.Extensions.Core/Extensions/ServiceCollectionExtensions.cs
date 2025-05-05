using Krosoft.Extensions.Core.Interfaces;
using Krosoft.Extensions.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDateTimeService(this IServiceCollection services)
    {
        services.AddTransient<IDateTimeService, DateTimeService>();

        return services;
    }
}