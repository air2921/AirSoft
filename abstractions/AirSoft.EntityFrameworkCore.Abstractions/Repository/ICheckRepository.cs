using AirSoft.EntityFrameworkCore.Abstractions.Entities;
using AirSoft.Exceptions;
using System.Linq.Expressions;

namespace AirSoft.EntityFrameworkCore.Abstractions.Repository
{
    /// <summary>
    /// Provides repository operations for checking entity existence in the data store.
    /// Supports both synchronous and asynchronous operations with cancellation support.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity. It must inherit from <see cref="EntityBase"/>.</typeparam>
    public interface ICheckRepository<TEntity> where TEntity : EntityBase
    {
        /// <summary>
        /// Asynchronously checks if any records match the specified filter.
        /// </summary>
        /// <param name="filter">The filter expression to apply to the entity set.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>True if matching record exists; otherwise false.</returns>
        /// <exception cref="EntityException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<bool> IsExistsAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if any records match the specified filter.
        /// </summary>
        /// <param name="filter">The filter expression to apply to the entity set.</param>
        /// <returns>True if matching record exists; otherwise false.</returns>
        /// <exception cref="EntityException">Thrown when database operation fails</exception>
        public bool IsExists(Expression<Func<TEntity, bool>> filter);
    }
}
