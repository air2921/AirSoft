using AirSoft.MongoDb.Abstractions;
using AirSoft.MongoDb.Implementations;
using AirSoft.MongoDb.MongoContexts;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Collections.Immutable;

namespace AirSoft.MongoDb
{
    /// <summary>
    /// Provides extension methods for configuring services in <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceExtension
    {
        /// <summary>
        /// Registers MongoDB repository pattern services for the specified MongoContext.
        /// </summary>
        /// <typeparam name="TMongoContext">The MongoContext type to register repositories for.</typeparam>
        /// <param name="services">The service collection to add the services to.</param>
        /// <param name="action">Configuration action for <see cref="MongoConfigureOptions"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/> for chaining.</returns>
        /// <remarks>
        /// Configures and registers the following services:
        /// - <see cref="MongoConfigureOptions"/> as singleton
        /// - <typeparamref name="TMongoContext"/> as scoped
        /// - Session factories (<see cref="IMongoSessionFactory"/>)
        /// - Repository implementations for all document types (<see cref="IMongoRepository{TDocument}"/>)
        /// 
        /// Scans the <typeparamref name="TMongoContext"/> for all <see cref="IMongoCollection{TDocument}"/> properties
        /// and registers corresponding repositories with scoped lifetime.
        /// </remarks>
        public static IServiceCollection AddMongoRepository<TMongoContext>(this IServiceCollection services, Action<MongoConfigureOptions> action) where TMongoContext : MongoContext
        {
            var options = new MongoConfigureOptions();
            action.Invoke(options);

            services.AddSingleton(options);
            services.AddScoped<TMongoContext>();

            services.AddScoped<IMongoSessionFactory, MongoSessionFactory<TMongoContext>>();

            var mongoContextType = typeof(TMongoContext);
            var documentTypes = mongoContextType.GetProperties()
                .Where(prop => prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(IMongoCollection<>))
                .Select(prop => prop.PropertyType.GetGenericArguments()[0])
                .ToImmutableArray();

            foreach (var documentType in documentTypes)
            {
                var repositoryType = typeof(MongoRepository<,>).MakeGenericType(documentType, mongoContextType);

                var interfaceType = typeof(IMongoRepository<>).MakeGenericType(documentType);
                services.AddScoped(interfaceType, repositoryType);
                services.AddScoped(documentType);
            }

            return services;
        }
    }
}
