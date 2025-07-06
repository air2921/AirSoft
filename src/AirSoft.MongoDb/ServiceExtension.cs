using AirSoft.MongoDb.Abstractions;
using AirSoft.MongoDb.Implementations;
using AirSoft.MongoDb.MongoContexts;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Collections.Immutable;

namespace AirSoft.MongoDb
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddMongoRepository<TMongoContext>(this IServiceCollection services, Action<MongoConfigureOptions> action) where TMongoContext : MongoContext
        {
            var options = new MongoConfigureOptions();
            action.Invoke(options);

            services.AddSingleton(options);
            services.AddScoped<TMongoContext>();

            services.AddScoped<IMongoSessionFactory, MongoSessionFactory<TMongoContext>>();
            services.AddScoped<IMongoSessionFactory<TMongoContext>, MongoSessionFactory<TMongoContext>>();

            var mongoContextType = typeof(TMongoContext);
            var documentTypes = mongoContextType.GetProperties()
                .Where(prop => prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(IMongoCollection<>))
                .Select(prop => prop.PropertyType.GetGenericArguments()[0])
                .ToImmutableArray();

            foreach (var documentType in documentTypes)
            {
                var repositoryType = typeof(MongoRepository<,>).MakeGenericType(documentType, mongoContextType);

                var interfaceType = typeof(IMongoRepository<>).MakeGenericType(documentType);
                var repositoryWithContextInterfaceType = typeof(IMongoRepository<,>).MakeGenericType(documentType, mongoContextType);
                services.AddScoped(repositoryWithContextInterfaceType, repositoryType);
                services.AddScoped(interfaceType, repositoryType);
                services.AddScoped(documentType);
            }

            return services;
        }
    }
}
