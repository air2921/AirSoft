using AirSoft.Cryptography.Abstractions;
using AirSoft.Cryptography.Cipher;
using AirSoft.Cryptography.Hasher;
using Microsoft.Extensions.DependencyInjection;

namespace AirSoft.Cryptography
{
    /// <summary>
    /// Provides extension methods for configuring services in <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceExtension
    {
        /// <summary>
        /// Adds cryptographic services to the dependency injection container.
        /// </summary>
        /// <param name="services">The service collection to add the services to.</param>
        /// <returns>The <see cref="IServiceCollection"/> for chaining.</returns>
        /// <remarks>
        /// Registers the following cryptographic services:
        /// - <see cref="IHasher"/> implemented by <see cref="BCryptHasher"/> (scoped)
        /// - <see cref="ICipher"/> implemented by <see cref="AesCipher"/> (scoped)
        /// </remarks>
        public static IServiceCollection AddCryptography(this IServiceCollection services)
        {
            services.AddScoped<IHasher, BCryptHasher>();
            services.AddScoped<ICipher, AesCipher>();

            return services;
        }
    }
}
