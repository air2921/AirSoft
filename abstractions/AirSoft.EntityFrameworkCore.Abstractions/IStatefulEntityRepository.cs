using AirSoft.EntityFrameworkCore.Abstractions.Builders.State.Add;
using AirSoft.EntityFrameworkCore.Abstractions.Builders.State.Remove;
using AirSoft.EntityFrameworkCore.Abstractions.Builders.State.Restore;
using AirSoft.EntityFrameworkCore.Abstractions.Builders.State.Update;
using AirSoft.EntityFrameworkCore.Abstractions.Entities;
using AirSoft.Exceptions;

namespace AirSoft.EntityFrameworkCore.Abstractions
{
    /// <summary>
    /// Generic repository interface providing state management operations (CRUD) for entities.
    /// Supports both synchronous and asynchronous methods for adding, updating, removing and restoring entities.
    /// Includes builder pattern for flexible operation configuration.
    /// All operations throw <see cref="EntityException"/> on failures.
    /// </summary>
    /// <typeparam name="TEntity">Entity type, must inherit from <see cref="EntityBase"/></typeparam>
    public interface IStatefulEntityRepository<TEntity> where TEntity : EntityBase
    {
        /// <summary>
        /// Asynchronously adds an entity using a configured builder.
        /// </summary>
        /// <param name="builder">Configured create builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the number of added entities (1 if successful, 0 otherwise).</returns>
        /// <exception cref="EntityException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<int> AddAsync(AddSingleBuilder<TEntity> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously adds an entity by configuring the builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the create builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the number of added entities (1 if successful, 0 otherwise).</returns>
        /// <exception cref="EntityException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<int> AddAsync(Action<AddSingleBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds an entity using a configured builder.
        /// </summary>
        /// <param name="builder">Configured create builder</param>
        /// <returns>The number of added entities (1 if successful, 0 otherwise).</returns>
        /// <exception cref="EntityException">Thrown when database operation fails</exception>
        public int Add(AddSingleBuilder<TEntity> builder);

        /// <summary>
        /// Adds an entity by configuring the builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the create builder</param>
        /// <returns>The number of added entities (1 if successful, 0 otherwise).</returns>
        /// <exception cref="EntityException">Thrown when database operation fails</exception>
        public int Add(Action<AddSingleBuilder<TEntity>> builderAction);

        /// <summary>
        /// Asynchronously adds multiple entities using a configured builder.
        /// </summary>
        /// <param name="builder">Configured create builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the number of added entities.</returns>
        /// <exception cref="EntityException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<int> AddRangeAsync(AddRangeBuilder<TEntity> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously adds multiple entities by configuring the builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the create builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the number of added entities.</returns>
        /// <exception cref="EntityException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<int> AddRangeAsync(Action<AddRangeBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds multiple entities using a configured builder.
        /// </summary>
        /// <param name="builder">Configured create builder</param>
        /// <returns>The number of added entities.</returns>
        /// <exception cref="EntityException">Thrown when database operation fails</exception>
        public int AddRange(AddRangeBuilder<TEntity> builder);

        /// <summary>
        /// Adds multiple entities by configuring the builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the create builder</param>
        /// <returns>The number of added entities.</returns>
        /// <exception cref="EntityException">Thrown when database operation fails</exception>
        public int AddRange(Action<AddRangeBuilder<TEntity>> builderAction);

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
