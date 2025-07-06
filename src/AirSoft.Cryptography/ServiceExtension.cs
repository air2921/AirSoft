using AirSoft.Cryptography.Abstractions;
using AirSoft.Cryptography.Cipher;
using AirSoft.Cryptography.Hasher;
using Microsoft.Extensions.DependencyInjection;

namespace AirSoft.Cryptography
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddCryptography(this IServiceCollection services)
        {
            services.AddScoped<IHasher, BCryptHasher>();
            services.AddScoped<ICipher, AesCipher>();

            return services;
        }
    }
}
