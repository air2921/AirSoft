using AirSoft.S3.Abstractions;
using AirSoft.S3.Implementations;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.Extensions.DependencyInjection;

namespace AirSoft.S3
{
    public static class ServiceExtension
    {
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
