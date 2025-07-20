using AirSoft.EntityFrameworkCore.Abstractions.Builders.Abstractions.State.Remove;

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
        public Task<TEntity?> RemoveAsync(IRemoveSingleBuilder<TEntity> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously removes an entity by configuring the remove builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the remove builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the removed entity or null.</returns>
        public Task<TEntity?> RemoveAsync(Action<IRemoveSingleBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes an entity using a configured builder.
        /// </summary>
        /// <param name="builder">Configured remove builder</param>
        /// <returns>The removed entity or null.</returns>
        public TEntity? Remove(IRemoveSingleBuilder<TEntity> builder);

        /// <summary>
        /// Removes an entity by configuring the remove builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the remove builder</param>
        /// <returns>The removed entity or null.</returns>
        public TEntity? Remove(Action<IRemoveSingleBuilder<TEntity>> builderAction);

        /// <summary>
        /// Asynchronously removes multiple entities using a configured builder.
        /// </summary>
        /// <param name="builder">Configured remove builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns a collection of removed entities.</returns>
        public Task<IEnumerable<TEntity>> RemoveRangeAsync(IRemoveRangeBuilder<TEntity> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously removes multiple entities by configuring the remove builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the remove builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns a collection of removed entities.</returns>
        public Task<IEnumerable<TEntity>> RemoveRangeAsync(Action<IRemoveRangeBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes multiple entities using a configured builder.
        /// </summary>
        /// <param name="builder">Configured remove builder</param>
        /// <returns>A collection of removed entities.</returns>
        public IEnumerable<TEntity> RemoveRange(IRemoveRangeBuilder<TEntity> builder);

        /// <summary>
        /// Removes multiple entities by configuring the remove builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the remove builder</param>
        /// <returns>A collection of removed entities.</returns>
        public IEnumerable<TEntity> RemoveRange(Action<IRemoveRangeBuilder<TEntity>> builderAction);
    }
}
