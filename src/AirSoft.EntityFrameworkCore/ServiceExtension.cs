using AirSoft.EntityFrameworkCore.Abstractions;
using AirSoft.EntityFrameworkCore.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Immutable;

namespace AirSoft.EntityFrameworkCore
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddEntityFrameworkCoreRepository<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
        {
            services.AddScoped<IUnitOfWork, UnitOfWork<TDbContext>>();
            services.AddScoped<IUnitOfWork<TDbContext>, UnitOfWork<TDbContext>>();

            services.AddScoped<ITransactionFactory, TransactionFactory<TDbContext>>();
            services.AddScoped<ITransactionFactory<TDbContext>, TransactionFactory<TDbContext>>();

            var dbContextType = typeof(TDbContext);
            var entityTypes = dbContextType.GetProperties()
                .Where(prop => prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                .Select(prop => prop.PropertyType.GetGenericArguments()[0])
                .ToImmutableArray();

            foreach (var entityType in entityTypes)
            {
                var repositoryType = typeof(Repository<,>).MakeGenericType(entityType, dbContextType);

                var interfaceType = typeof(IRepository<>).MakeGenericType(entityType);
                var repositoryWithContextInterfaceType = typeof(IRepository<,>).MakeGenericType(entityType, dbContextType);
                services.AddScoped(repositoryWithContextInterfaceType, repositoryType);
                services.AddScoped(interfaceType, repositoryType);
            }

            return services;
        }
    }
}
