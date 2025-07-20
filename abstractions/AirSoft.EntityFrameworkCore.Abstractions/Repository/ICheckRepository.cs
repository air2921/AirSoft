using System.Linq.Expressions;
using AirSoft.Exceptions;

namespace AirSoft.EntityFrameworkCore.Abstractions.Repository
{
    public interface ICheckRepository<TEntity> where TEntity : IEntityBase
    {
        /// <summary>
        /// Asynchronously checks existing record that match the specified filter.
        /// </summary>
        /// <param name="filter">The filter expression to apply to the entity set.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>The true if record is exits otherwise false.</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during the check exists operation.</exception>
        public Task<bool> IsExistsAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks existing record that match the specified filter.
        /// </summary>
        /// <param name="filter">The filter expression to apply to the entity set.</param>
        /// <returns>The true if record is exits otherwise false.</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during the check exists operation.</exception>
        public bool IsExists(Expression<Func<TEntity, bool>> filter);
    }
}
