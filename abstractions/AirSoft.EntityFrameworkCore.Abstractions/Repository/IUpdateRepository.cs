using AirSoft.EntityFrameworkCore.Abstractions.Builders.State.Update;
using AirSoft.Exceptions;

namespace AirSoft.EntityFrameworkCore.Abstractions.Repository
{
    /// <summary>
    /// Provides repository pattern operations for updating entities in the data store.
    /// Supports both single and batch updates through builder pattern configuration.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to update, must implement IEntityBase</typeparam>
    public interface IUpdateRepository<TEntity> where TEntity : IEntityBase
    {
        /// <summary>
        /// Asynchronously updates an entity using a configured builder.
        /// </summary>
        /// <param name="builder">Configured update builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the number of updated entities (1 if successful, 0 otherwise).</returns>
        /// <exception cref="EntityException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<int> UpdateAsync(UpdateSingleBuilder<TEntity> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously updates an entity by configuring the update builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the update builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the number of updated entities (1 if successful, 0 otherwise).</returns>
        /// <exception cref="EntityException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<int> UpdateAsync(Action<UpdateSingleBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an entity using a configured builder.
        /// </summary>
        /// <param name="builder">Configured update builder</param>
        /// <returns>The number of updated entities (1 if successful, 0 otherwise).</returns>
        /// <exception cref="EntityException">Thrown when database operation fails</exception>
        public int Update(UpdateSingleBuilder<TEntity> builder);

        /// <summary>
        /// Updates an entity by configuring the update builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the update builder</param>
        /// <returns>The number of updated entities (1 if successful, 0 otherwise).</returns>
        /// <exception cref="EntityException">Thrown when database operation fails</exception>
        public int Update(Action<UpdateSingleBuilder<TEntity>> builderAction);

        /// <summary>
        /// Asynchronously updates multiple entities using a configured builder.
        /// </summary>
        /// <param name="builder">Configured update builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the number of updated entities.</returns>
        /// <exception cref="EntityException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<int> UpdateRangeAsync(UpdateRangeBuilder<TEntity> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously updates multiple entities by configuring the update builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the update builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the number of updated entities.</returns>
        /// <exception cref="EntityException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<int> UpdateRangeAsync(Action<UpdateRangeBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates multiple entities using a configured builder.
        /// </summary>
        /// <param name="builder">Configured update builder</param>
        /// <returns>The number of updated entities.</returns>
        /// <exception cref="EntityException">Thrown when database operation fails</exception>
        public int UpdateRange(UpdateRangeBuilder<TEntity> builder);

        /// <summary>
        /// Updates multiple entities by configuring the update builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the update builder</param>
        /// <returns>The number of updated entities.</returns>
        /// <exception cref="EntityException">Thrown when database operation fails</exception>
        public int UpdateRange(Action<UpdateRangeBuilder<TEntity>> builderAction);
    }
}
