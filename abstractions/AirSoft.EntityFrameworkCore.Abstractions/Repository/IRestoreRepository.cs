using AirSoft.EntityFrameworkCore.Abstractions.Builders.State.Restore;
using AirSoft.Exceptions;

namespace AirSoft.EntityFrameworkCore.Abstractions.Repository
{
    /// <summary>
    /// Provides repository pattern operations for restoring soft-deleted entities in the data store.
    /// Supports both single and batch restoration operations through builder pattern configuration.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to restore, must implement IEntityBase</typeparam>
    public interface IRestoreRepository<TEntity> where TEntity : IEntityBase
    {
        /// <summary>
        /// Asynchronously restores an entity using a configured builder.
        /// </summary>
        /// <param name="builder">Configured restore builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the number of restored entities (1 if successful, 0 otherwise).</returns>
        /// <exception cref="EntityException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<int> RestoreAsync(RestoreSingleBuilder<TEntity> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously restores an entity by configuring the restore builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the restore builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the number of restored entities (1 if successful, 0 otherwise).</returns>
        /// <exception cref="EntityException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<int> RestoreAsync(Action<RestoreSingleBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Restores an entity using a configured builder.
        /// </summary>
        /// <param name="builder">Configured restore builder</param>
        /// <returns>The number of restored entities (1 if successful, 0 otherwise).</returns>
        /// <exception cref="EntityException">Thrown when database operation fails</exception>
        public int Restore(RestoreSingleBuilder<TEntity> builder);

        /// <summary>
        /// Restores an entity by configuring the restore builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the restore builder</param>
        /// <returns>The number of restored entities (1 if successful, 0 otherwise).</returns>
        /// <exception cref="EntityException">Thrown when database operation fails</exception>
        public int Restore(Action<RestoreSingleBuilder<TEntity>> builderAction);

        /// <summary>
        /// Asynchronously restores multiple entities using a configured builder.
        /// </summary>
        /// <param name="builder">Configured restore builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the number of restored entities.</returns>
        /// <exception cref="EntityException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<int> RestoreRangeAsync(RestoreRangeBuilder<TEntity> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously restores multiple entities by configuring the restore builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the restore builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the number of restored entities.</returns>
        /// <exception cref="EntityException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<int> RestoreRangeAsync(Action<RestoreRangeBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Restores multiple entities using a configured builder.
        /// </summary>
        /// <param name="builder">Configured restore builder</param>
        /// <returns>The number of restored entities.</returns>
        /// <exception cref="EntityException">Thrown when database operation fails</exception>
        public int RestoreRange(RestoreRangeBuilder<TEntity> builder);

        /// <summary>
        /// Restores multiple entities by configuring the restore builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the restore builder</param>
        /// <returns>The number of restored entities.</returns>
        /// <exception cref="EntityException">Thrown when database operation fails</exception>
        public int RestoreRange(Action<RestoreRangeBuilder<TEntity>> builderAction);
    }
}
