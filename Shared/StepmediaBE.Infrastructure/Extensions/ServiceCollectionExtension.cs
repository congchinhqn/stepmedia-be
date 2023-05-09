using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using StackExchange.Redis;
using StepmediaBE.Infrastructure;

namespace Metatrade.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    #region Fields

    private static readonly LoggerFactory LoggerFactory =
        new LoggerFactory(new[] {new DebugLoggerProvider()});

    #endregion
    
    public static void AddDataAccessLayer(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<StepmediaContext>(builder => builder
                .UseLazyLoadingProxies()
                .UseSqlServer(connectionString)
        );
    }

    private static T GetService<T>(this IServiceCollection services)
    {
        using var provider = services.BuildServiceProvider();
        return provider.GetService<T>();
    }
}