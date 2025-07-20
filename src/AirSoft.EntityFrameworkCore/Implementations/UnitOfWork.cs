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
        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>
        /// The number of state entries written to the database.
        /// </returns>
        /// <exception cref="EntityException">Thrown when an error occurs while saving changes to the database.</exception>
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

        /// <summary>
        /// Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A <see cref="Task"/> that represents the asynchronous save operation. 
        /// The task result contains the number of state entries written to the database.
        /// </returns>
        /// <exception cref="EntityException">Thrown when an error occurs while saving changes to the database.</exception>
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
