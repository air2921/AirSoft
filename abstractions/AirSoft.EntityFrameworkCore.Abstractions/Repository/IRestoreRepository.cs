using AirSoft.EntityFrameworkCore.Abstractions.Builders.State.Restore;

namespace AirSoft.EntityFrameworkCore.Abstractions.Repository
{
    public interface IRestoreRepository<TEntity> where TEntity : IEntityBase
    {
        /// <summary>
        /// Asynchronously restores an entity using a configured builder.
        /// </summary>
        /// <param name="builder">Configured restore builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the restored entity.</returns>
        public Task<TEntity> RestoreAsync(RestoreSingleBuilder<TEntity> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously restores an entity by configuring the restore builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the restore builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the restored entity.</returns>
        public Task<TEntity> RestoreAsync(Action<RestoreSingleBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Restores an entity using a configured builder.
        /// </summary>
        /// <param name="builder">Configured restore builder</param>
        /// <returns>The restored entity.</returns>
        public TEntity Restore(RestoreSingleBuilder<TEntity> builder);

        /// <summary>
        /// Restores an entity by configuring the restore builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the restore builder</param>
        /// <returns>The restored entity.</returns>
        public TEntity Restore(Action<RestoreSingleBuilder<TEntity>> builderAction);

        /// <summary>
        /// Asynchronously restores multiple entities using a configured builder.
        /// </summary>
        /// <param name="builder">Configured restore builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns a collection of restored entities.</returns>
        public Task<IEnumerable<TEntity>> RestoreRangeAsync(RestoreRangeBuilder<TEntity> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously restores multiple entities by configuring the restore builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the restore builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns a collection of restored entities.</returns>
        public Task<IEnumerable<TEntity>> RestoreRangeAsync(Action<RestoreRangeBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Restores multiple entities using a configured builder.
        /// </summary>
        /// <param name="builder">Configured restore builder</param>
        /// <returns>A collection of restored entities.</returns>
        public IEnumerable<TEntity> RestoreRange(RestoreRangeBuilder<TEntity> builder);

        /// <summary>
        /// Restores multiple entities by configuring the restore builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the restore builder</param>
        /// <returns>A collection of restored entities.</returns>
        public IEnumerable<TEntity> RestoreRange(Action<RestoreRangeBuilder<TEntity>> builderAction);
    }
}
