using AirSoft.DistributedCache.Abstractions;
using AirSoft.DistributedCache.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace AirSoft.DistributedCache
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddDistributedCache(this IServiceCollection services, Action<DistributedCacheConfigureOptions> action)
        {
            var options = new DistributedCacheConfigureOptions();
            action.Invoke(options);

            services.AddStackExchangeRedisCache(cache =>
            {
                cache.Configuration = options.Connection;
            });

            services.AddSingleton(options);
            services.AddScoped<ICacheClient, CacheClient>();

            return services;
        }
    }
}
