using AirSoft.EntityFrameworkCore.Abstractions;
using AirSoft.EntityFrameworkCore.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Immutable;

namespace AirSoft.EntityFrameworkCore
{
    /// <summary>
    /// Provides extension methods for configuring services in <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceExtension
    {
        /// <summary>
        /// Registers Entity Framework Core repository pattern services for the specified DbContext.
        /// </summary>
        /// <typeparam name="TDbContext">The DbContext type to register repositories for.</typeparam>
        /// <param name="services">The service collection to add the services to.</param>
        /// <returns>The <see cref="IServiceCollection"/> for chaining.</returns>
        /// <remarks>
        /// Automatically registers the following services:
        /// - Unit of Work implementations <see cref="IUnitOfWork"/>
        /// - Transaction factories <see cref="ITransactionFactory"/>
        /// - Repository implementations for all entity types <see cref="IRepository{TEntity}"/>)
        /// 
        /// Scans the <typeparamref name="TDbContext"/> for all <see cref="DbSet{TEntity}"/> properties
        /// and registers corresponding repositories with scoped lifetime.
        /// </remarks>
        public static IServiceCollection AddEntityFrameworkCoreRepository<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
        {
            services.AddScoped<IUnitOfWork, UnitOfWork<TDbContext>>();
            services.AddScoped<ITransactionFactory, TransactionFactory<TDbContext>>();

            var dbContextType = typeof(TDbContext);
            var entityTypes = dbContextType.GetProperties()
                .Where(prop => prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                .Select(prop => prop.PropertyType.GetGenericArguments()[0])
                .ToImmutableArray();

            foreach (var entityType in entityTypes)
            {
                var repositoryType = typeof(Repository<,>).MakeGenericType(entityType, dbContextType);

                var interfaceType = typeof(IRepository<>).MakeGenericType(entityType);
                services.AddScoped(interfaceType, repositoryType);
            }

            return services;
        }
    }
}
