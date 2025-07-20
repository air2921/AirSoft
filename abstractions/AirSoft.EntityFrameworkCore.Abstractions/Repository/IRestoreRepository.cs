using AirSoft.EntityFrameworkCore.Abstractions.Builders.Abstractions.State.Restore;
using AirSoft.Exceptions;

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
        public Task<TEntity> RestoreAsync(IRestoreSingleBuilder<TEntity> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously restores an entity by configuring the restore builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the restore builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the restored entity.</returns>
        public Task<TEntity> RestoreAsync(Action<IRestoreSingleBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Restores an entity using a configured builder.
        /// </summary>
        /// <param name="builder">Configured restore builder</param>
        /// <returns>The restored entity.</returns>
        public TEntity Restore(IRestoreSingleBuilder<TEntity> builder);

        /// <summary>
        /// Restores an entity by configuring the restore builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the restore builder</param>
        /// <returns>The restored entity.</returns>
        public TEntity Restore(Action<IRestoreSingleBuilder<TEntity>> builderAction);

        /// <summary>
        /// Asynchronously restores multiple entities using a configured builder.
        /// </summary>
        /// <param name="builder">Configured restore builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns a collection of restored entities.</returns>
        public Task<IEnumerable<TEntity>> RestoreRangeAsync(IRestoreRangeBuilder<TEntity> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously restores multiple entities by configuring the restore builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the restore builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns a collection of restored entities.</returns>
        public Task<IEnumerable<TEntity>> RestoreRangeAsync(Action<IRestoreRangeBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Restores multiple entities using a configured builder.
        /// </summary>
        /// <param name="builder">Configured restore builder</param>
        /// <returns>A collection of restored entities.</returns>
        public IEnumerable<TEntity> RestoreRange(IRestoreRangeBuilder<TEntity> builder);

        /// <summary>
        /// Restores multiple entities by configuring the restore builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the restore builder</param>
        /// <returns>A collection of restored entities.</returns>
        public IEnumerable<TEntity> RestoreRange(Action<IRestoreRangeBuilder<TEntity>> builderAction);
    }
}
