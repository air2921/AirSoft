using AirSoft.S3.Abstractions;
using AirSoft.S3.Implementations;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.Extensions.DependencyInjection;

namespace AirSoft.S3
{
    /// <summary>
    /// Provides extension methods for configuring services in <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceExtension
    {
        /// <summary>
        /// Adds Amazon S3 client services to the dependency injection container.
        /// </summary>
        /// <param name="services">The service collection to add the services to.</param>
        /// <param name="action">Configuration action for <see cref="S3ConfigureOptions"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/> for chaining.</returns>
        /// <remarks>
        /// Configures and registers the following services:
        /// - <see cref="IAmazonS3"/> client configured with provided credentials and endpoint
        /// - <see cref="IS3Client"/> implementation as scoped service
        /// </remarks>
        public static IServiceCollection AddS3Client(this IServiceCollection services, Action<S3ConfigureOptions> action)
        {
            var options = new S3ConfigureOptions();
            action.Invoke(options);

            var s3Config = new AmazonS3Config
            {
                ServiceURL = options.Endpoint,
                ForcePathStyle = true,
                UseHttp = options.Endpoint?.StartsWith("http://") == true,
            };

            var awsCredentials = new BasicAWSCredentials(options.AccessKey, options.SecretKey);

            services.AddSingleton<IAmazonS3>(_ => new AmazonS3Client(awsCredentials, s3Config));
            services.AddScoped<IS3Client, S3Client>();

            return services;
        }
    }
}
