using AirSoft.DistributedCache.Abstractions;
using AirSoft.DistributedCache.Implementations;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace AirSoft.DistributedCache
{
    /// <summary>
    /// Provides extension methods for configuring services in <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceExtension
    {
        /// <summary>
        /// Adds distributed Redis cache services to the dependency injection container.
        /// </summary>
        /// <param name="services">The service collection to add the services to.</param>
        /// <param name="action">Configuration action for <see cref="DistributedCacheConfigureOptions"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/> for chaining.</returns>
        /// <remarks>
        /// Configures and registers the following services:
        /// - StackExchange.Redis with provided connection string
        /// - <see cref="DistributedCacheConfigureOptions"/> as singleton
        /// - <see cref="IDistributedCache"/> as scoped
        /// - <see cref="ICacheClient"/> implemented by <see cref="CacheClient"/> as scoped
        /// </remarks>
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
