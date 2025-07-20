using AirSoft.EntityFrameworkCore.Abstractions.Builders.State.Remove;
using AirSoft.Exceptions;

namespace AirSoft.EntityFrameworkCore.Abstractions.Repository
{
    /// <summary>
    /// Provides repository pattern operations for removing entities from the data store.
    /// Supports both single and batch removal operations through builder pattern configuration.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to remove, must implement IEntityBase</typeparam>
    public interface IRemoveRepository<TEntity> where TEntity : IEntityBase
    {
        /// <summary>
        /// Asynchronously removes an entity using a configured builder.
        /// </summary>
        /// <param name="builder">Configured remove builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the number of removed entities (1 if successful, 0 otherwise).</returns>
        /// <exception cref="EntityException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<int> RemoveAsync(RemoveSingleBuilder<TEntity> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously removes an entity by configuring the remove builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the remove builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the number of removed entities (1 if successful, 0 otherwise).</returns>
        /// <exception cref="EntityException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<int> RemoveAsync(Action<RemoveSingleBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes an entity using a configured builder.
        /// </summary>
        /// <param name="builder">Configured remove builder</param>
        /// <returns>The number of removed entities (1 if successful, 0 otherwise).</returns>
        /// <exception cref="EntityException">Thrown when database operation fails</exception>
        public int Remove(RemoveSingleBuilder<TEntity> builder);

        /// <summary>
        /// Removes an entity by configuring the remove builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the remove builder</param>
        /// <returns>The number of removed entities (1 if successful, 0 otherwise).</returns>
        /// <exception cref="EntityException">Thrown when database operation fails</exception>
        public int Remove(Action<RemoveSingleBuilder<TEntity>> builderAction);

        /// <summary>
        /// Asynchronously removes multiple entities using a configured builder.
        /// </summary>
        /// <param name="builder">Configured remove builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the number of removed entities.</returns>
        /// <exception cref="EntityException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<int> RemoveRangeAsync(RemoveRangeBuilder<TEntity> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously removes multiple entities by configuring the remove builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the remove builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the number of removed entities.</returns>
        /// <exception cref="EntityException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<int> RemoveRangeAsync(Action<RemoveRangeBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes multiple entities using a configured builder.
        /// </summary>
        /// <param name="builder">Configured remove builder</param>
        /// <returns>The number of removed entities.</returns>
        /// <exception cref="EntityException">Thrown when database operation fails</exception>
        public int RemoveRange(RemoveRangeBuilder<TEntity> builder);

        /// <summary>
        /// Removes multiple entities by configuring the remove builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the remove builder</param>
        /// <returns>The number of removed entities.</returns>
        /// <exception cref="EntityException">Thrown when database operation fails</exception>
        public int RemoveRange(Action<RemoveRangeBuilder<TEntity>> builderAction);
    }
}
