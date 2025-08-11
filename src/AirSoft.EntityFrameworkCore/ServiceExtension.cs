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
        /// Registers default entity repositories (both read and write operations) for all DbSet&lt;T&gt; entities
        /// in the specified DbContext as scoped services implementing IEntityRepository&lt;T&gt;
        /// </summary>
        /// <typeparam name="TDbContext">Type of the DbContext to scan for entities</typeparam>
        /// <param name="services">The IServiceCollection to add services to</param>
        /// <returns>The same service collection so that multiple calls can be chained</returns>
        public static IServiceCollection AddDefaultEntityFrameworkRepository<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
        {
            var dbContextType = typeof(TDbContext);
            var entityTypes = dbContextType.GetProperties()
                .Where(prop => prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                .Select(prop => prop.PropertyType.GetGenericArguments()[0])
                .ToImmutableArray();

            foreach (var entityType in entityTypes)
            {
                var repositoryType = typeof(Repository<,>).MakeGenericType(entityType, dbContextType);

                var interfaceType = typeof(IEntityRepository<>).MakeGenericType(entityType);
                services.AddScoped(interfaceType, repositoryType);
            }

            return services;
        }

        /// <summary>
        /// Registers read-only entity repositories for all DbSet&lt;T&gt; entities in the specified DbContext
        /// as scoped services implementing IReadonlyEntityRepository&lt;T&gt;
        /// </summary>
        /// <typeparam name="TDbContext">Type of the DbContext to scan for entities</typeparam>
        /// <param name="services">The IServiceCollection to add services to</param>
        /// <returns>The same service collection so that multiple calls can be chained</returns>
        public static IServiceCollection AddReadonlyEntityFrameworkRepository<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
        {
            var dbContextType = typeof(TDbContext);
            var entityTypes = dbContextType.GetProperties()
                .Where(prop => prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                .Select(prop => prop.PropertyType.GetGenericArguments()[0])
                .ToImmutableArray();

            foreach (var entityType in entityTypes)
            {
                var repositoryType = typeof(Repository<,>).MakeGenericType(entityType, dbContextType);

                var interfaceType = typeof(IReadonlyEntityRepository<>).MakeGenericType(entityType);
                services.AddScoped(interfaceType, repositoryType);
            }

            return services;
        }

        /// <summary>
        /// Registers stateful (write) entity repositories for all DbSet&lt;T&gt; entities in the specified DbContext
        /// as scoped services implementing IStatefulEntityRepository&lt;T&gt;
        /// </summary>
        /// <typeparam name="TDbContext">Type of the DbContext to scan for entities</typeparam>
        /// <param name="services">The IServiceCollection to add services to</param>
        /// <returns>The same service collection so that multiple calls can be chained</returns>
        public static IServiceCollection AddStatefulEntityFrameworkRepository<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
        {
            var dbContextType = typeof(TDbContext);
            var entityTypes = dbContextType.GetProperties()
                .Where(prop => prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                .Select(prop => prop.PropertyType.GetGenericArguments()[0])
                .ToImmutableArray();

            foreach (var entityType in entityTypes)
            {
                var repositoryType = typeof(Repository<,>).MakeGenericType(entityType, dbContextType);

                var interfaceType = typeof(IStatefulEntityRepository<>).MakeGenericType(entityType);
                services.AddScoped(interfaceType, repositoryType);
            }

            return services;
        }

        /// <summary>
        /// Registers Unit of Work pattern implementations for the specified DbContext
        /// including IUnitOfWork and ITransactionFactory as scoped services
        /// </summary>
        /// <typeparam name="TDbContext">Type of the DbContext to use with Unit of Work</typeparam>
        /// <param name="services">The IServiceCollection to add services to</param>
        /// <returns>The same service collection so that multiple calls can be chained</returns>
        public static IServiceCollection AddUnitOfWork<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
        {
            services.AddScoped<IUnitOfWork, UnitOfWork<TDbContext>>();
            services.AddScoped<ITransactionFactory, TransactionFactory<TDbContext>>();

            return services;
        }
    }
}
