using AirSoft.EntityFrameworkCore.Abstractions.Builders.State.Remove;
using AirSoft.Exceptions;

namespace AirSoft.EntityFrameworkCore.Abstractions.Repository
{
    public interface IRemoveRepository<TEntity> where TEntity : IEntityBase
    {
        /// <summary>
        /// Asynchronously removes an entity using a configured builder.
        /// </summary>
        /// <param name="builder">Configured remove builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the removed entity or null.</returns>
        /// <exception cref="EntityException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<TEntity?> RemoveAsync(RemoveSingleBuilder<TEntity> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously removes an entity by configuring the remove builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the remove builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the removed entity or null.</returns>
        /// <exception cref="EntityException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<TEntity?> RemoveAsync(Action<RemoveSingleBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes an entity using a configured builder.
        /// </summary>
        /// <param name="builder">Configured remove builder</param>
        /// <returns>The removed entity or null.</returns>
        /// <exception cref="EntityException">Thrown when database operation fails</exception>
        public TEntity? Remove(RemoveSingleBuilder<TEntity> builder);

        /// <summary>
        /// Removes an entity by configuring the remove builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the remove builder</param>
        /// <returns>The removed entity or null.</returns>
        /// <exception cref="EntityException">Thrown when database operation fails</exception>
        public TEntity? Remove(Action<RemoveSingleBuilder<TEntity>> builderAction);

        /// <summary>
        /// Asynchronously removes multiple entities using a configured builder.
        /// </summary>
        /// <param name="builder">Configured remove builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns a collection of removed entities.</returns>
        /// <exception cref="EntityException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<IEnumerable<TEntity>> RemoveRangeAsync(RemoveRangeBuilder<TEntity> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously removes multiple entities by configuring the remove builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the remove builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns a collection of removed entities.</returns>
        /// <exception cref="EntityException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<IEnumerable<TEntity>> RemoveRangeAsync(Action<RemoveRangeBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes multiple entities using a configured builder.
        /// </summary>
        /// <param name="builder">Configured remove builder</param>
        /// <returns>A collection of removed entities.</returns>
        /// <exception cref="EntityException">Thrown when database operation fails</exception>
        public IEnumerable<TEntity> RemoveRange(RemoveRangeBuilder<TEntity> builder);

        /// <summary>
        /// Removes multiple entities by configuring the remove builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the remove builder</param>
        /// <returns>A collection of removed entities.</returns>
        /// <exception cref="EntityException">Thrown when database operation fails</exception>
        public IEnumerable<TEntity> RemoveRange(Action<RemoveRangeBuilder<TEntity>> builderAction);
    }
}
