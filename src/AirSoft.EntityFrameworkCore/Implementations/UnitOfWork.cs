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
    /// <param name="logger">The enhanced logger instance used for logging operations and errors.</param>
    public class UnitOfWork<TDbContext>(TDbContext context) : IUnitOfWork, IUnitOfWork<TDbContext> where TDbContext : DbContext
    {
        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <exception cref="EntityException">Thrown when an error occurs while saving changes to the database.</exception>
        public void SaveChanges()
        {
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new EntityException("Unable to save changes", ex);
            }
        }

        /// <summary>
        /// Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        /// <exception cref="EntityException">Thrown when an error occurs while saving changes to the database.</exception>
        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new EntityException("Unable to save changes", ex);
            }
        }
    }
}
