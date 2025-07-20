using AirSoft.EntityFrameworkCore.Abstractions.Builders.State.Update;
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
        /// <exception cref="EntityException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<TEntity> UpdateAsync(UpdateSingleBuilder<TEntity> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously updates an entity by configuring the update builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the update builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the updated entity.</returns>
        /// <exception cref="EntityException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<TEntity> UpdateAsync(Action<UpdateSingleBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an entity using a configured builder.
        /// </summary>
        /// <param name="builder">Configured update builder</param>
        /// <returns>The updated entity.</returns>
        /// <exception cref="EntityException">Thrown when database operation fails</exception>
        public TEntity Update(UpdateSingleBuilder<TEntity> builder);

        /// <summary>
        /// Updates an entity by configuring the update builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the update builder</param>
        /// <returns>The updated entity.</returns>
        /// <exception cref="EntityException">Thrown when database operation fails</exception>
        public TEntity Update(Action<UpdateSingleBuilder<TEntity>> builderAction);

        /// <summary>
        /// Asynchronously updates multiple entities using a configured builder.
        /// </summary>
        /// <param name="builder">Configured update builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns a collection of updated entities.</returns>
        /// <exception cref="EntityException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<IEnumerable<TEntity>> UpdateRangeAsync(UpdateRangeBuilder<TEntity> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously updates multiple entities by configuring the update builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the update builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns a collection of updated entities.</returns>
        /// <exception cref="EntityException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<IEnumerable<TEntity>> UpdateRangeAsync(Action<UpdateRangeBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates multiple entities using a configured builder.
        /// </summary>
        /// <param name="builder">Configured update builder</param>
        /// <returns>A collection of updated entities.</returns>
        /// <exception cref="EntityException">Thrown when database operation fails</exception>
        public IEnumerable<TEntity> UpdateRange(UpdateRangeBuilder<TEntity> builder);

        /// <summary>
        /// Updates multiple entities by configuring the update builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the update builder</param>
        /// <returns>A collection of updated entities.</returns>
        /// <exception cref="EntityException">Thrown when database operation fails</exception>
        public IEnumerable<TEntity> UpdateRange(Action<UpdateRangeBuilder<TEntity>> builderAction);
    }
}
