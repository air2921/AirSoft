using AirSoft.EntityFrameworkCore.Abstractions;
using AirSoft.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AirSoft.EntityFrameworkCore.Implementations
{
    /// <summary>
    /// Represents a unit of work implementation that coordinates the writing of changes to the underlying database context.
    /// </summary>
    /// <typeparam name="TDbContext">The type of the database context, which must inherit from DbContext.</typeparam>
    /// <param name="context">The database context instance that this unit of work will operate on.</param>
    public class UnitOfWork<TDbContext>(TDbContext context) : IUnitOfWork where TDbContext : DbContext
    {
        /// <inheritdoc/>
        public int SaveChanges()
        {
            try
            {
                return context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new EntityException("Unable to save changes", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new EntityException("Unable to save changes", ex);
            }
        }
    }
}
