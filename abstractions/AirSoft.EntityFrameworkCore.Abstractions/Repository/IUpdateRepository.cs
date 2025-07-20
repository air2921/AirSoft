using AirSoft.EntityFrameworkCore.Abstractions.Builders.Abstractions.State.Update;
using AirSoft.Exceptions;

namespace AirSoft.EntityFrameworkCore.Abstractions.Repository
{
    public interface IUpdateRepository<TEntity> where TEntity : IEntityBase
    {
        /// <summary>
        /// Asynchronously updates an entity using a configured builder.
        /// </summary>
        /// <param name="builder">Configured update builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the updated entity.</returns>
        public Task<TEntity> UpdateAsync(IUpdateSingleBuilder<TEntity> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously updates an entity by configuring the update builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the update builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the updated entity.</returns>
        public Task<TEntity> UpdateAsync(Action<IUpdateSingleBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an entity using a configured builder.
        /// </summary>
        /// <param name="builder">Configured update builder</param>
        /// <returns>The updated entity.</returns>
        public TEntity Update(IUpdateSingleBuilder<TEntity> builder);

        /// <summary>
        /// Updates an entity by configuring the update builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the update builder</param>
        /// <returns>The updated entity.</returns>
        public TEntity Update(Action<IUpdateSingleBuilder<TEntity>> builderAction);

        /// <summary>
        /// Asynchronously updates multiple entities using a configured builder.
        /// </summary>
        /// <param name="builder">Configured update builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns a collection of updated entities.</returns>
        public Task<IEnumerable<TEntity>> UpdateRangeAsync(IUpdateRangeBuilder<TEntity> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously updates multiple entities by configuring the update builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the update builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns a collection of updated entities.</returns>
        public Task<IEnumerable<TEntity>> UpdateRangeAsync(Action<IUpdateRangeBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates multiple entities using a configured builder.
        /// </summary>
        /// <param name="builder">Configured update builder</param>
        /// <returns>A collection of updated entities.</returns>
        public IEnumerable<TEntity> UpdateRange(IUpdateRangeBuilder<TEntity> builder);

        /// <summary>
        /// Updates multiple entities by configuring the update builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the update builder</param>
        /// <returns>A collection of updated entities.</returns>
        public IEnumerable<TEntity> UpdateRange(Action<IUpdateRangeBuilder<TEntity>> builderAction);
    }
}
